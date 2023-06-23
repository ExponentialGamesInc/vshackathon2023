using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 force = new Vector2();
        if (Input.GetKey(KeyCode.W))
        {
            force.y = speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            force.y = -speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            force.x = speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            force.x = -speed;
        }

        rb.AddForce(force);
    }
}
