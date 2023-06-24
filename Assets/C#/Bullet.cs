using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 15;
    GameObject player;
    public LayerMask enemyMask;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > 50)
        {
            Destroy(this.gameObject);
        }

        var enemies = Physics2D.OverlapCircleAll(transform.position, 5, enemyMask);
        foreach (var enemy in enemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < 0.7f)
            {
                var enemyEnnemy = enemy.GetComponent<Enemy>();
                enemyEnnemy.health -= damage;
                enemyEnnemy.state = EnemyState.Chase;
                enemyEnnemy.Alert(1);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
