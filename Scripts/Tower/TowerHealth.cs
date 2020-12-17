using System;
using UnityEngine;

namespace LTD.Towers
{
    public class TowerHealth : MonoBehaviour
    {
        public int health;
        Tower m_tower;

        public Tower Tower { get => m_tower; }

        public static event EventHandler<OnTowerDeathEventArgs> OnTowerDeath;
        public class OnTowerDeathEventArgs : EventArgs
        {
            public Tower tower;
        }

        public void Init(Tower tower)
        {
            health = tower.health;
            m_tower = tower;
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
            m_tower.health = Mathf.Max(m_tower.health - damage, 0);
        }

        private void Die()
        {
            OnTowerDeath?.Invoke(this, new OnTowerDeathEventArgs { tower = m_tower });
            Destroy(gameObject);
        }
    }
}

