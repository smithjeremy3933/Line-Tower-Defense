using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    TowerHealth target = null;

    int damage = 5;
    float timeBetweenAttacks = 1f;
    float timeSinceLastAttack = Mathf.Infinity;

    public TowerHealth Target { get => target; set => target = value; }

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
}
