using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 15;
    GameObject player;
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            var enemy = collision.collider.GetComponent<Enemy>();
            enemy.health -= damage;
            enemy.state = EnemyState.Chase;
            enemy.Alert(1);
        }
        Destroy(this.gameObject);
    }
}
