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
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (!isTuned)
            return;

    }
}
