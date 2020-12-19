using LTD.Towers;
using System;
using UnityEngine;

namespace LTD.EnemyUnits
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private ParticleSystem hitParticleSystem;

        public static event EventHandler OnEnemyScored;
        public static event EventHandler<OnEnemyDeathEventArgs> OnEnemyDeath;
        public class OnEnemyDeathEventArgs : EventArgs
        {
            public EnemyMovement enemyMovement;
        }

        int health;
        Unit _unit;

        public void Init(Unit unit)
        {
            health = unit.health;
            _unit = unit;
        }

        public void TakeDamage(int damage)
        {
            ProcessHit(damage);
            if (health <= 0)
            {
                Die();
            }
        }

        private void ProcessHit(int damage)
        {
            health = Mathf.Max(health - damage, 0);
            _unit.health = Mathf.Max(_unit.health - damage, 0);
            hitParticleSystem.Play();
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

}
