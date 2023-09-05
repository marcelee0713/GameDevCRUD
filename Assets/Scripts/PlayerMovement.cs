using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    public Joystick joystick;

    private float dirX = 0f;
    [Header("Movements")]
    public float runningSpeed = 5f;
    public float jumpPower = 10f;

    [Header("Dash Parameters")]
    public float dashSpeed = 5f;
    public float dashingTime = 0.2f;
    public float dashCooldown = 1f;
    private bool canDash = true;
    private bool isDashing = false;

    [Header("Attacks")]
    public bool isAttacking = false;
    private int maxCombo = 3;
    private int combo = 0;

    public float comboTime = 1.5f;
    public float comboTimeStamp = 0f;

    public float attackCd = .5f;
    private float timeStamp = 0f;

    private GameObject hitBoxObj;
    private Transform hitBoxCollider;

    [Header("Knockback")]
    public bool isKnockedBack = false;
    public float knockBackForce = 10.0f;

    [Header("Effects and Essentials")]
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private TrailRenderer tr;
    public ParticleSystem dust;

    private enum MovementState
    {
        idle, running, jumping, falling, attack, secondAttack, thirdAttack
    }
    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        hitBoxObj = GameObject.FindWithTag("PlayerHitBox");
        hitBoxCollider = hitBoxObj.GetComponent<Transform>();
        isKnockedBack = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDashing || isKnockedBack || healthSystem.health <= 0) return;

        if (comboTimeStamp <= Time.time) combo = 0;

        dirX = joystick.Horizontal;
        rb.velocity = new Vector2(dirX * runningSpeed, rb.velocity.y);

        UpdateAnimationState();


    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (combo == 1)
        {
            state = MovementState.attack;
            anim.SetInteger("state", (int)state);
            if (dirX > 0f)
            {
                state = MovementState.running;
                transform.localScale = new Vector3(1, 1, 1);


            }
            else if (dirX < 0f)
            {
                state = MovementState.running;
                transform.localScale = new Vector3(-1, 1, 1);

            }
            return;
        } else if (combo == 2)
        {
            state = MovementState.secondAttack;
            anim.SetInteger("state", (int)state);
            if (dirX > 0f)
            {
                state = MovementState.running;
                transform.localScale = new Vector3(1, 1, 1);


            }
            else if (dirX < 0f)
            {
                state = MovementState.running;
                transform.localScale = new Vector3(-1, 1, 1);

            }
            return;
        }
        else if (combo == 3)
        {
            state = MovementState.thirdAttack;
            anim.SetInteger("state", (int)state);
            if (dirX > 0f)
            {
                state = MovementState.running;
                transform.localScale = new Vector3(1, 1, 1);


            }
            else if (dirX < 0f)
            {
                state = MovementState.running;
                transform.localScale = new Vector3(-1, 1, 1);

            }

            return;
        }

        if (dirX > 0f)
        {
            state = MovementState.running;
            transform.localScale = new Vector3(1, 1, 1);


        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            transform.localScale = new Vector3(-1, 1, 1);

        }
        else
        {
            state = MovementState.idle;

        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            
            if(!IsGrounded()) state = MovementState.falling;
        }


        anim.SetInteger("state", (int)state);
    }

    public void Jump()
    {
        if (IsGrounded() && healthSystem.health > 0)
        {
            createDust();
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

    }

    private IEnumerator DashLogic()
    {

        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        rb.velocity = new Vector2(transform.localScale.x * dashSpeed, 0f);

        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

    }

    public void Attack()
    {
        if(isKnockedBack || healthSystem.health <= 0)
        {
            combo = 0;
            return;
        }
        if (timeStamp < Time.time && combo < maxCombo)
        {
            combo++;
            UnityEngine.Debug.Log("ATTACK:  " + combo);
            timeStamp = Time.time + attackCd;
            comboTimeStamp = Time.time + comboTime;
        }

        if ((Time.time - timeStamp) > attackCd)
        {
            combo = 0;
        }

    }

    public void Dash()
    {
        if (healthSystem.health <= 0) return;
        if (canDash)
        {
            StartCoroutine(DashLogic());
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    public void KnockBack()
    {
        StartCoroutine(KnockbackLogic());
    }

    private IEnumerator KnockbackLogic()
    {
        isKnockedBack = true;
        rb.velocity = new Vector2(-transform.localScale.x * knockBackForce, knockBackForce);
        yield return new WaitForSeconds(.2f);
        isKnockedBack = false;
    }

    void createDust()
    {
        dust.Play();
    }
}
