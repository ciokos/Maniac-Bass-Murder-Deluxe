using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class HitEvent : UnityEvent<bool>{}
public class Bullet : MonoBehaviour
{
    public int damage;
    private bool isEmpowered = false;
    private bool isTuned = false;
    public HitEvent enemyHitEvent;
    private readonly float tuneFactor = 0.1f;

    void Start()
    {
        if (enemyHitEvent == null)
            enemyHitEvent = new HitEvent();
    }
    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    public void SetBulletParameters(int dmg, bool isEmpowered, bool isTuned)
    {
        this.damage = dmg;
        this.isEmpowered = isEmpowered;
        this.isTuned = isTuned;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && gameObject.CompareTag("PlayerBullet"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
            enemyHitEvent.Invoke(isEmpowered);
        }
        else if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("EnemyBullet"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("PlayerBullet"))
            return;
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (!isTuned || !gameObject.CompareTag("PlayerBullet"))
            return;
        GameObject closestEnemy = FindClosestEnemy();
        if (closestEnemy == null)
            return;
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        Vector3 dir = closestEnemy.transform.position - gameObject.transform.position;
        rb.AddForce(dir * tuneFactor / dir.sqrMagnitude, ForceMode2D.Impulse);
        rb.velocity.Normalize();
    }

    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
}
