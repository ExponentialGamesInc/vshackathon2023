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
    int health;
    public float speed;
    public float damage;
    public EnemyState state = EnemyState.Idle;

    public GameObject healthBar;
    public GameObject healthBarBase;
    public float healthBarSize;
    public float healthBarHeight;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.GetComponent<SpriteRenderer>().enabled = health != maxHealth;
        healthBarBase.GetComponent<SpriteRenderer>().enabled = health != maxHealth;

        healthBar.transform.localPosition = new Vector3(healthBarSize * (1 - (float)health/maxHealth) / -2, healthBarHeight, -1);
        healthBarBase.transform.localPosition = new Vector3(0, healthBarHeight, 0);

        healthBar.transform.localScale = new Vector3(healthBarSize * ((float)health /maxHealth), healthBar.transform.localScale.y, 0);
        healthBarBase.transform.localScale = new Vector3(healthBarSize, healthBar.transform.localScale.y, 0);

        if (state == EnemyState.Idle && Vector3.Distance(transform.position, new Vector3(0, 0, 0)) > 5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 0, 0), speed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            health -= collision.gameObject.GetComponent<Bullet>().damage;
            Destroy(collision.gameObject);
        }
    }
}
