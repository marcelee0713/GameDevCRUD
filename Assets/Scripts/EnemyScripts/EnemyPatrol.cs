using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;

    [Header("Chasing Section")]
    public Transform playerTransform;
    public SpriteRenderer playerSpriteRenderer;
    public Rigidbody2D playerRb;
    public bool isChasing;
    public float chaseDistance;
    private void Awake()
    {
        initScale = enemy.localScale;
    
    }
    private void OnDisable()
    {
        if (anim != null) anim.SetBool("moving", false);
    }

    private void Update()
    {
        if (enemy == null) return;


        if(isChasing && (playerTransform.position.x <= leftEdge.position.x || playerTransform.position.x >= rightEdge.position.x))
        {
            isChasing = false;
        }
        if (isChasing)
        {
            if (enemy.position.x > playerTransform.position.x)
            {
                MoveInDirection(-1, 0);
            }
            else if (enemy.position.x < playerTransform.position.x)
            {
                MoveInDirection(1, 0);
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
            {
                isChasing = true;
            }
            if (movingLeft)
            {
                if (enemy.position.x >= leftEdge.position.x)
                    MoveInDirection(-1, 0);
                else
                    DirectionChange();
            }
            else
            {
                if (enemy.position.x <= rightEdge.position.x)
                    MoveInDirection(1, 0);
                else
                    DirectionChange();
            }
        }
       
    }

    public void DirectionChange()
    {
        if (anim != null)
        {
            anim.SetBool("moving", false);
            idleTimer += Time.deltaTime;

            if (idleTimer > idleDuration)
                movingLeft = !movingLeft;
        }

    }

    public void MoveInDirection(int _direction, int timer)
    {
        if (anim != null)
        {
            idleTimer = timer;
            anim.SetBool("moving", true);

            //Make enemy face direction
            enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction,
                initScale.y, initScale.z);

            //Move in that direction
            enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
                enemy.position.y, enemy.position.z);
        }


    }
}