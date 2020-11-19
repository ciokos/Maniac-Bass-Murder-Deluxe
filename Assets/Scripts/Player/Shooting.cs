using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    public float delay = 1f;
    private bool canShoot = true;
    public AudioSource audioSource;

    private Conductor conductor;
    private void Start()
    {
        conductor = FindObjectOfType<Conductor>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (!canShoot)
            return;
        audioSource.Play();
        var power = GetShootPower();
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.transform.localScale += new Vector3(power, power, 0);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        bullet.gameObject.tag = "PlayerBullet";
        bullet.GetComponent<Bullet>().SetDamage(20f * power);
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        canShoot = false;
        StartCoroutine(ShootDelay());
    }

    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }

    private float GetShootPower()
    {
        var beat = conductor.getBeatValue();
        var diff = Mathf.Abs(beat - Mathf.Round(beat)) * 2;
        var power = 1 - diff;
        return power;
    }
}
