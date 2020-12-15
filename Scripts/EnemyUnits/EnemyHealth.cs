using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    int health = 100;

    public void TakeDamage(int damage)
    {
        health = Mathf.Max(health - damage, 0);
        Debug.Log(health);
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
