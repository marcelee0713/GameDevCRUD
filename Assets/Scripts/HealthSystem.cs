using System.Collections;
using System.Collections.Generic;
using System.Data;
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

    public GameObject GameOverScene;
    public GameObject WinScene;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(UpdateUI.userGems == 8)
        {
            GameOverScene.SetActive(false);
            WinScene.SetActive(true);
        }
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

    private void Die()
    {
        anim.SetTrigger("death");
        health = 0;
        rb.bodyType = RigidbodyType2D.Dynamic;
        GameOverScene.SetActive(true);
        WinScene.SetActive(false);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
        PlayerPrefs.DeleteAll();
    }
}
