using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Gun : MonoBehaviour
{
    public GameObject bullet;
    public GameObject bulletSpawn;
    public float bulletSpeed = 10;
    public float fireDelay;
    private float lastFired;
    public int maxAmmo;
    public float reloadTime;
    public int ammo;
    public bool reloading;
    public int damage = 35;
    public float explodeRadius = 3;
    public AudioSource shoot;
    public GameObject flipSpawn;
    // Start is called before the first frame update
    void Start()
    {
        lastFired = Time.realtimeSinceStartup;
        ammo = maxAmmo;
        reloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        explodeRadius = 3 + damage / 50;
        if (!UI.gamePaused)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            Vector2 direction = mousePosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (Vector2.Distance(mousePosition, transform.position) >= 0.05f)
                transform.rotation = Quaternion.Euler(0f, 0f, angle);


            GetComponentInChildren<SpriteRenderer>().flipY = !(angle < 90 && angle > -90);


            if (Input.GetMouseButton(0) && Time.realtimeSinceStartup - lastFired >= fireDelay && ammo > 0 && !reloading)
            {
                lastFired = Time.realtimeSinceStartup;
                ammo -= 1;
                shoot.Play();
                GameObject newBullet = Instantiate(bullet);
                Rigidbody2D bulletRb = newBullet.GetComponent<Rigidbody2D>();

                if (angle < 90 && angle > -90)
                    newBullet.transform.position = bulletSpawn.transform.position;
                else
                    newBullet.transform.position = flipSpawn.transform.position;
                newBullet.transform.rotation = transform.rotation;

                newBullet.GetComponent<Bullet>().damage = damage;
                newBullet.GetComponent<Bullet>().explodeRadius = explodeRadius;



                bulletRb.AddForce(bulletSpeed * (mousePosition - transform.position).normalized, ForceMode2D.Impulse);
            }

            if (ammo != maxAmmo && (Input.GetKeyDown(KeyCode.R) || (ammo == 0 && !reloading)))
            {
                reloading = true;
                StartCoroutine(Reload());
            }
        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
        ammo = maxAmmo;
    }
}
