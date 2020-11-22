﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage;
    public void SetDamage(int dmg)
    {
        damage = dmg;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && gameObject.CompareTag("PlayerBullet"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("EnemyBullet"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(damage);
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
}
