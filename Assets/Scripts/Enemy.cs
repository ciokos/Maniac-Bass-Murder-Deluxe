using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Transform firePoint;
    public Text healthText;
    private Transform target;
    private Conductor conductor;
    public int[] beatsToShoot = { 3, 7, 11, 15 };
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    public AudioSource audioSource;

    private readonly float maxHealth = 100f;
    private float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        conductor = (Conductor)GameObject.FindObjectOfType<Conductor>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        conductor.Beat.AddListener(onBeat);
        RefreshHealthText();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target.position - this.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
    }

    private void onBeat(float beatValue)
    {
        int beat = (int)beatValue;
        if(beatsToShoot.Contains(beat))
        {
            Shoot();
        }

    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        bullet.gameObject.tag = "EnemyBullet";
        bullet.GetComponent<Bullet>().SetDamage(10f);
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        audioSource.Play();
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        RefreshHealthText();
    }

    private void RefreshHealthText()
    {
        healthText.text = ((int)currentHealth).ToString() + "/" + ((int)maxHealth).ToString();
    }
}
