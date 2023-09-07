using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public float health;
    public float maxHealth;

    private Animator anim;
    private Rigidbody2D rb;
    public PlayerMovement playerMovement;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (health <= 0)
        {
            Die();
            return; 
        }

        if (collision.gameObject.CompareTag("Spikes") || collision.gameObject.CompareTag("Enemy"))
        {
            Hurt(1);
        }
    }

    public void Hurt(int damage)
    {
        health -= damage;
        anim.SetTrigger("hurt");
        playerMovement.KnockBack();

        if (health <= 0)
        {
            Die();
            return;
        }

    }

    private async void Die()
    {
        anim.SetTrigger("death");
        health = 0;
        rb.bodyType = RigidbodyType2D.Dynamic;
        await Task.Delay(2000);
        RestartLevel();
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }
}
