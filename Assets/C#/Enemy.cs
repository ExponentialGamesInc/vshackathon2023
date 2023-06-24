using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public enum EnemyState
{
    Idle,
    Chase,
}
public class Enemy : MonoBehaviour
{
    public int maxHealth;
    public int health;
    public float speed;
    public float damage;
    public EnemyState state = EnemyState.Idle;

    public GameObject healthBar;
    public GameObject healthBarBase;
    public float healthBarSize;
    public float healthBarHeight;
    public Rigidbody2D rigidbody2D;

    public float enemyIdleRadius;
    public LayerMask playerMask;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        rigidbody2D = GetComponent<Rigidbody2D>();
        state = EnemyState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 1)
        {
            Destroy(gameObject);
        }

        var hit = Physics2D.OverlapCircle(transform.position, enemyIdleRadius, playerMask.value);

        if (hit == null)
        {
            state = EnemyState.Idle;
        }

        healthBar.GetComponent<SpriteRenderer>().enabled = health != maxHealth;
        healthBarBase.GetComponent<SpriteRenderer>().enabled = health != maxHealth;

        healthBar.transform.localPosition = new Vector3(healthBarSize * (1 - (float)health / maxHealth) / -2, healthBarHeight, -1);
        healthBarBase.transform.localPosition = new Vector3(0, healthBarHeight, 0);

        healthBar.transform.localScale = new Vector3(healthBarSize * ((float)health / maxHealth), healthBar.transform.localScale.y, 0);
        healthBarBase.transform.localScale = new Vector3(healthBarSize, healthBar.transform.localScale.y, 0);

        if (state == EnemyState.Idle && Vector2.Distance(rigidbody2D.position, new Vector2(0, 0)) > 5f)
        {
            Vector2 newPosition = Vector2.MoveTowards(rigidbody2D.position, new Vector2(0, 0), speed * Time.deltaTime);
            rigidbody2D.MovePosition(newPosition);
        }

        if (state == EnemyState.Chase)
        {
            Vector2 newPosition = Vector2.MoveTowards(rigidbody2D.position, FindObjectOfType<Player>().GetComponent<Player>().transform.position, speed * Time.deltaTime);
            rigidbody2D.MovePosition(newPosition);
        }
    }
}
