using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    [SerializeField] Transform turret;

    Tower m_tower;

    float attackRange;
    int damage;

    public void Init(Tower tower)
    {
        this.attackRange = tower.attackRange;
        this.damage = tower.damage;
        m_tower = tower;
    }
}
