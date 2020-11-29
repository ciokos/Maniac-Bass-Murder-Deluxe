using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject regularBulletPrefab;
    public GameObject powerBulletPrefab;
    public float bulletSpeed = 2f;
    public int bulletRegularForce = 1;
    public int bulletPowerForce = 2;
    public float delay = 1f;
    public float shot_accuracy = 0.2f;
    private bool canShoot = true;
    public AudioSource audioSource;
    

    private Conductor conductor;
    private List<IModifier> modifiers;
    private void Start()
    {
        conductor = FindObjectOfType<Conductor>();
        modifiers = new List<IModifier>();
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
        bool isEmpowered;

        // is the shot empowered?
        if(1 - power < shot_accuracy)
        {
            chosenBulletPrefab = powerBulletPrefab; ;
            damage = bulletPowerForce;
            isEmpowered = true;
        }
        else
        {
            chosenBulletPrefab = regularBulletPrefab;
            damage = bulletRegularForce;
            isEmpowered = false;
        }

        // spawn the bullet
        GameObject bullet = Instantiate(chosenBulletPrefab, firePoint.position, firePoint.rotation);
        List<GameObject> bullets = new List<GameObject>();
        bullets.Add(bullet);

        // set tag
        bullet.gameObject.tag = "PlayerBullet";
        // set damage and empowered status
        bullet.GetComponent<Bullet>().SetBulletParameters(damage, isEmpowered);
        // subscribe to enemy hit event
        bullet.GetComponent<Bullet>().enemyHitEvent.AddListener(onEnemyHit);

        // apply modifiers
        foreach(IModifier modifier in modifiers)
        {
            modifier.Modify(bullets, isEmpowered);
        }

        foreach(GameObject b in bullets)
        {
            // add force
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.up * bulletSpeed, ForceMode2D.Impulse);
        }

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

    private void onEnemyHit(bool isEmpowered)
    {
       foreach(IModifier modifier in modifiers)
        {
            modifier.EnemyHit(isEmpowered);
        }
    }

    public void AddModifier(IModifier modifier)
    {
        modifiers.Add(modifier);
    }
}
