using System;
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
    public bool attacking;
    public bool chomping;
    public int maxHealth;
    public int health;
    public float speed;
    public int damage;
    public int score;
    public EnemyState state = EnemyState.Idle;

    public GameObject healthBar;
    public GameObject healthBarBase;
    public float healthBarSize;
    public float healthBarHeight;
    public Rigidbody2D rigidbody2D;
    public float attackDelay = 0.4f;
    public ParticleSystem particleSystem;

    public float enemyIdleRadius;
    public LayerMask playerMask;
    public LayerMask enemyMask;
    public GameObject scrap;
    public int minDrop;
    public int maxDrop;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        rigidbody2D = GetComponent<Rigidbody2D>();
        state = EnemyState.Idle;
    }

    // Update is called once per frame
    void Update()
    {   if (!UI.gamePaused)
        {
            if (health < 1)
            {
                var particle = Instantiate(particleSystem.gameObject);
                particle.transform.position = transform.position;
                particle.GetComponent<ParticleSystem>().Play();

                FindObjectOfType<Player>().enemyScore += score;
                for (int i = 0; i < UnityEngine.Random.Range(minDrop, maxDrop + 1); i++)
                {
                    var newScrap = Instantiate(scrap);
                    newScrap.transform.position = new Vector3(transform.position.x + UnityEngine.Random.Range(-1.0f, 1.0f), transform.position.y + UnityEngine.Random.Range(-1.0f, 1.0f), transform.position.z);
                }

                Splitter[] splitters = GetComponents<Splitter>();
                foreach (Splitter splitter in splitters)
                {
                    splitter.Split();
                }

                Destroy(particle, 3);
                Destroy(gameObject);
            }

            var hit = Physics2D.OverlapCircle(transform.position, enemyIdleRadius, playerMask.value);

            if (hit == null || Vector3.Distance(transform.position, FindObjectOfType<Player>().transform.position) > Vector3.Distance(transform.position, new Vector3()))
            {
                state = EnemyState.Idle;
            }


            healthBar.GetComponent<SpriteRenderer>().enabled = health != maxHealth;
            healthBarBase.GetComponent<SpriteRenderer>().enabled = health != maxHealth;

            healthBar.transform.localPosition = new Vector3(healthBarSize * (1 - (float)health / maxHealth) / -2, healthBarHeight, -1);
            healthBarBase.transform.localPosition = new Vector3(0, healthBarHeight, 0);

            healthBar.transform.localScale = new Vector3(healthBarSize * ((float)health / maxHealth), healthBar.transform.localScale.y, 0);
            healthBarBase.transform.localScale = new Vector3(healthBarSize, healthBar.transform.localScale.y, 0);

            if (state == EnemyState.Idle && Vector2.Distance(rigidbody2D.position, new Vector2(0, 0)) > 0)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && !attacking)
        {
            StartCoroutine(Chomp());
            state = EnemyState.Chase;
            Alert(1);
        }

        if (collision.collider.CompareTag("Base") && !chomping)
        {
            StartCoroutine(BaseChomp());
        }
    }

    public void Alert(int ch)
    {
        if (ch > 2)
            return;
        var hits = Physics2D.OverlapCircleAll(transform.position, 3, enemyMask);

        foreach (var hit in hits)
        {
            var hitEnemy = hit.GetComponent<Enemy>();
            hitEnemy.state = EnemyState.Chase;
            hitEnemy.Alert(ch + 1);
        }
    }
    IEnumerator BaseChomp()
    {
        chomping = true;
        while (Vector3.Distance(transform.position, FindObjectOfType<Base>().transform.position) < 3.2f)
        {
            yield return new WaitForSeconds(attackDelay);
            if (Vector3.Distance(transform.position, FindObjectOfType<Base>().transform.position) < 3.2f)
            {
                FindObjectOfType<Base>().health -= damage;
                FindAnyObjectByType<Player>().totalDamage += damage;
            }
        }
        chomping = false;
    }

    IEnumerator Chomp()
    {
        attacking = true;

        while (Vector3.Distance(transform.position, FindObjectOfType<Player>().transform.position) < 2)
        {
            yield return new WaitForSeconds(attackDelay);
            if (Vector3.Distance(transform.position, FindObjectOfType<Player>().transform.position) < 2)
            {
                FindObjectOfType<Player>().health -= damage;
                FindObjectOfType<Player>().lastDamage = DateTime.Now;
                FindAnyObjectByType<Player>().totalDamage += damage;
            }
        }
        
        attacking = false;
    }
}
