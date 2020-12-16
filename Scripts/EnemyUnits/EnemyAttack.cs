using LTD.Towers;
using UnityEngine;

namespace LTD.EnemyUnits
{
    public class EnemyAttack : MonoBehaviour
    {
        TowerHealth target = null;

        int damage = 5;
        float timeBetweenAttacks = 1f;
        float timeSinceLastAttack = Mathf.Infinity;

        public TowerHealth Target { get => target; }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        public void Attack(TowerHealth target)
        {
            if (target != null)
            {
                GetComponent<Animator>().SetBool("punch", true);
                if (timeSinceLastAttack > timeBetweenAttacks)
                {
                    timeSinceLastAttack = 0;
                    target.TakeDamage(damage);
                }
            }
        }

        public void SetTarget(TowerHealth towerHealth)
        {
            if (towerHealth != null)
            {
                target = towerHealth;
            }
        }

        public bool IsTarget()
        {
            if (target != null)
            {
                return true;
            }
            return false;
        }
    }

}
