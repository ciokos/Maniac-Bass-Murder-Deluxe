using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public Transform firePoint;
    private Transform target;
    private Conductor conductor;
    public int[] beatsToShoot = { 3, 7, 11, 15 };
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    public int bulletDamage = 1;
    public AudioSource audioSource;
    public float viewDistance = 10f;
    public UnityEvent ActivateEvent;
    public LayerMask IgnoreInVision;

    public int maxHealth = 2;
    private int currentHealth;
    private NavMeshAgent agent;
    private EnemyMovement movementComponent;
    private bool isActive = false;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        conductor = (Conductor)GameObject.FindObjectOfType<Conductor>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        conductor.Beat.AddListener(onBeat);

        if (ActivateEvent == null)
            ActivateEvent = new UnityEvent();
    }

    // Update is called once per frame
    void Update()
    {
        checkFieldOfView();
        if(isActive)
        {
            Vector3 direction = target.position - this.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
        }

    }

    private void onBeat(float beatValue)
    {
        if(isActive)
        {
            int beat = (int)beatValue;
            if (beatsToShoot.Contains(beat))
            {
                Shoot();
            }
        }
    }

    private void checkFieldOfView()
    {
        Vector3 direction = (target.position - this.transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, viewDistance, ~IgnoreInVision);
        if(hit.collider != null)
        {
            if(hit.collider.gameObject != gameObject)
            {
                if(hit.collider.gameObject.name == "Player")
                {
                    if (!isActive)
                        ActivateEvent.Invoke();
                    isActive = true;
                }
            }
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        bullet.gameObject.tag = "EnemyBullet";
        bullet.GetComponent<Bullet>().SetDamage(bulletDamage);
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        audioSource.Play();
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
