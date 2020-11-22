using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject regularBulletPrefab;
    public GameObject powerBulletPrefab;
    public int bulletRegularForce = 1;
    public int bulletPowerForce = 2;
    public float delay = 1f;
    public float shot_accuracy = 0.2f;
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
        // check if the player can shoot already
        if (!canShoot)
            return;
        // play the sound
        audioSource.Play();

        // get shot precision from the conductor
        var power = GetShootPower();
        GameObject chosenBulletPrefab;
        int damage;

        // is the shot empowered?
        if(1 - power < shot_accuracy)
        {
            chosenBulletPrefab = powerBulletPrefab;
            damage = bulletPowerForce;
        }
        else
        {
            chosenBulletPrefab = regularBulletPrefab;
            damage = bulletRegularForce;
        }

        // spawn the bullet
        GameObject bullet = Instantiate(chosenBulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // set tag
        bullet.gameObject.tag = "PlayerBullet";
        // set damage
        bullet.GetComponent<Bullet>().SetDamage(damage);
        // add force
        rb.AddForce(firePoint.up * bulletRegularForce, ForceMode2D.Impulse);
        // set delay
        canShoot = false;
        StartCoroutine(ShootDelay());
    }

    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }

    // get power of the shot - float between 1 and 0
    private float GetShootPower()
    {
        var beat = conductor.getBeatValue();
        var diff = Mathf.Abs(beat - Mathf.Round(beat)) * 2;
        var power = 1 - diff;
        return power;
    }
}
