using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Texture
{
    public SpriteRenderer renderer;
    public int x;
    public int y;
}

public class Player : MonoBehaviour
{
    public Animator animator;
    public float speed = 1;
    public int scrap = 0;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    public Gun gun;
    public int maxHealth;
    public int health;
    public int points;
    public int enemyScore = 0;
    public int upgrades = 0;
    public int totalScrap = 0;
    public int totalDamage = 0;
    public DateTime lastDamage;

    public int healthUpgradeLevel = -1;
    public List<int> healthUpgrades = new List<int> { 150, 200, 450, 700, 1500};

    public int damageUpgradeLevel = -1;
    public List<int> damageUpgrades = new List<int> { 23, 35, 85, 190, 450 };

    public int attackDelayUpgradeLevel = -1;
    public List<float> attackDelayUpgrades = new List<float> { 0.23f, 0.2f, 0.18f, 0.12f, 0.08f};


    public List<Texture> textures = new List<Texture>();

    Vector2 movement;

    float facing;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gun = GetComponentInChildren<Gun>();
        health = maxHealth;
        lastDamage = DateTime.Now;

    }
    private void Update()
    {
        if (DateTime.Now >= lastDamage.AddSeconds(10))
        {
            HealPlayer();
        }

        if (health <= 0)
        {
            UI.gamePaused = true;
            var ui = FindObjectOfType<UI>();
            ui.deathScreen.SetActive(true);
            ui.deathText.text = "You died :/";
            ui.scoreText.text = string.Format("Score: {0}", Mathf.Clamp(((upgrades * ui.count) + enemyScore + totalScrap) - totalDamage, 0, int.MaxValue));
        }
        else if (FindObjectOfType<Base>().health <=  0)
        {
            UI.gamePaused = true;
            var ui = FindObjectOfType<UI>();
            ui.deathScreen.SetActive(true);
            ui.deathText.text = "Your base was killed :/";
            ui.scoreText.text = string.Format("Score: {0}", Mathf.Clamp(((upgrades * ui.count) + enemyScore + totalScrap) - totalDamage, 0, int.MaxValue));
        }
       
        if (!UI.gamePaused)
        {
            movement.x = Input.GetAxisRaw("Horizontal");


            if (movement.x == 1)
            {

                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (movement.x == -1)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            movement.y = Input.GetAxisRaw("Vertical");

            moveDirection = new Vector2(movement.x, movement.y).normalized;

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }


        if (movement.x != 0)
        {
            facing = 1;
        }
        if (movement.y > 0.01)
        {
            facing = 2;
        }
        if (movement.y < -0.01)
        {
            facing = 3;
        }

        animator.SetFloat("Idle", facing);
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!UI.gamePaused)
        {
            rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void HealPlayer()
    {
        health += (int)(health * 0.1f);
        health = Mathf.Min(health, maxHealth); // Ensure health doesn't exceed maxHealth
        lastDamage = DateTime.Now;
    }
}
