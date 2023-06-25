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
    public float speed = 1;
    public int scrap = 0;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    public Gun gun;
    public int maxHealth;
    public int health;
    public int points;

    public int healthUpgradeLevel;
    public List<int> healthUpgrades = new List<int> {100, 150, 450, 700, 1500};


    public List<Texture> textures = new List<Texture>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gun = GetComponentInChildren<Gun>();
        health = maxHealth;
    }
    private void Update()
    {
        if (!UI.gamePaused)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            moveDirection = new Vector2(moveX, moveY).normalized;
        }

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
}
