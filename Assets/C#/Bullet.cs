using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 15;
    public float explodeRadius = 3;
    public LayerMask enemyMask;
    GameObject player;
    public GameObject sound;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > 40)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, explodeRadius, enemyMask.value).ToList();
            hits.Remove(collision.collider);
            var enemyEnnemy = collision.collider.GetComponent<Enemy>();

            foreach (var enemy in hits)
            {
                enemyEnnemy = enemy.GetComponent<Enemy>();
                enemyEnnemy.health -= damage;

            }

            enemyEnnemy = collision.collider.GetComponent<Enemy>();
            enemyEnnemy.health -= damage;
            enemyEnnemy.state = EnemyState.Chase;
            enemyEnnemy.Alert(1);
            var no = Instantiate(sound);
            no.transform.position = transform.position;
            no.GetComponent<AudioSource>().Play();
            Destroy(no, 5);
        }

        Destroy(this.gameObject);
    }
}
