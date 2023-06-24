using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bullet;
    public float bulletSpeed = 10;
    public float fireDelay;
    private float lastFired;
    // Start is called before the first frame update
    void Start()
    {
        lastFired = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector2 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (Vector2.Distance(mousePosition, transform.position) >= 0.05f)
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (Input.GetMouseButton(0) && Time.realtimeSinceStartup - lastFired >= fireDelay)
        {
            lastFired = Time.realtimeSinceStartup;
            GameObject newBullet = Instantiate(bullet);
            Rigidbody2D bulletRb = newBullet.GetComponent<Rigidbody2D>();
            newBullet.transform.position = transform.position + transform.right * 1f;
            newBullet.transform.rotation = transform.rotation;
            bulletRb.AddForce(bulletSpeed * (mousePosition - transform.position).normalized, ForceMode2D.Impulse);
        }
    }
}