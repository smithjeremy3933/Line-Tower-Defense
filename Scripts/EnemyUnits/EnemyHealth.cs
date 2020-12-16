using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public static event EventHandler OnEnemyScored;
    public static event EventHandler<OnEnemyDeathEventArgs> OnEnemyDeath;
    public class OnEnemyDeathEventArgs : EventArgs
    {
        public EnemyMovement enemyMovement;
    }

    int health = 100;

    public void TakeDamage(int damage)
    {
        health = Mathf.Max(health - damage, 0);
        if (health <= 0)
        {
            Die();
        }
    }

    public void ReachedGoal()
    {
        OnEnemyDeath?.Invoke(this, new OnEnemyDeathEventArgs { enemyMovement = gameObject.GetComponent<EnemyMovement>() });
        OnEnemyScored?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }

    void Die()
    {
        OnEnemyDeath?.Invoke(this, new OnEnemyDeathEventArgs { enemyMovement = gameObject.GetComponent<EnemyMovement>() });
        Destroy(gameObject);
    }

    private void OnParticleCollision(GameObject particle)
    {
        BulletsView bulletsView = particle.GetComponent<BulletsView>();
        if (bulletsView != null)
        {
            TakeDamage(bulletsView.damage);
        }      
    }
}
