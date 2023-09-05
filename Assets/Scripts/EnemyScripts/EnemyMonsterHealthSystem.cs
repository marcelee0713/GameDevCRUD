using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMonsterHealthSystem : MonoBehaviour
{
    public int health = 10;
    private Animator anim;
    private Rigidbody2D rb;
    private EnemyBunnyBehavior bunnyBehavior;
    private EnemyPatrol enemyPatrol;
    [SerializeField] private BoxCollider2D boxCollider;

    public bool enemyHurt = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        bunnyBehavior = GetComponent<EnemyBunnyBehavior>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            anim.SetTrigger("die");
            rb.bodyType = RigidbodyType2D.Static;
            boxCollider.enabled = false;
            Destroy(this.gameObject, 1f);
            enemyPatrol.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO Soon: Add a way to compute the attack damage like:
        // AttackSlash + AttackStats
        if (collision.gameObject.CompareTag("PlayerHitBox"))
        {
            StartCoroutine(TakeDamage(1));

        }

    }

    private IEnumerator TakeDamage(int attack)
    {
        enemyHurt = true;
        if (bunnyBehavior.PlayerInSight())
        {

            if (transform.localScale.x >= 1)
            {
                rb.velocity = new Vector2(-5f, 5f);

            }
            else if (transform.localScale.x <= -1)
            {
                rb.velocity = new Vector2(5f, 5f);

            }
        }
        else
        {
            if (transform.localScale.x >= 1)
            {
                rb.velocity = new Vector2(5f, 5f);
                enemyPatrol.MoveInDirection(-1, 2);
            }
            else if (transform.localScale.x <= -1)
            {
                rb.velocity = new Vector2(-5f, 5f);
                enemyPatrol.MoveInDirection(1, 2);
            }
            enemyPatrol.DirectionChange();
        }
        health -= attack;
        anim.SetTrigger("hurt");
        yield return new WaitForSeconds(0.4f);
        enemyHurt = false;
    }
}
