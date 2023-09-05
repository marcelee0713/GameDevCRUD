using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBunnyBehavior : MonoBehaviour
{
    [Header("Attack Parameters")]
    private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    //References
    private Animator anim;
    private HealthSystem playerHealth;
    private EnemyPatrol enemyPatrol;
    private EnemyMonsterHealthSystem enemyHealthSystem;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
        boxCollider = GetComponent<BoxCollider2D>();
        enemyHealthSystem = GetComponent<EnemyMonsterHealthSystem>();
    }

    private void Update()
    {
        if(enemyHealthSystem.enemyHurt)
        {
            enemyPatrol.enabled = false;
            return;
        }
        cooldownTimer += Time.deltaTime;

        //Attack only when player in sight
        if (PlayerInSight() && playerHealth.health >= 1)
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("attack");
            }
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();

    }

    public bool PlayerInSight()
    {
        // Wag mo na ito intindihin, tignan mo na lang yung mga Section niya doon sa Game Object na ito.
        // Pero ang ginagawa nito is mag rereturn ng boolean type value. 
        // Pag merong dumaan sa pulang box, mag rereturn ng true.
        // Feel free to adjust some values DOON SA Unity para malaman mo.
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<HealthSystem>();

        return hit.collider != null;
    }

    // Para kumulay lang yung detection
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
            playerHealth.Hurt(damage);
    }
}
