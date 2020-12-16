using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    [SerializeField] GameObject turret;
    [SerializeField] ParticleSystem projectileParticle;

    Tower m_tower;
    UnitDatabase unitDatabase;

    float attackRange;
    int damage;
    readonly float rotateTime = 0.4f;
    float delay = 0f;
    iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;
    Transform target;

    private void Start()
    {
        unitDatabase = FindObjectOfType<UnitDatabase>();
    }

    private void Update()
    {
        SetTargetEnemy();
        if (target)
        {
            RotateToEnemy(target);
        }
    }

    private void SetTargetEnemy()
    {
        var enemies = FindObjectsOfType<EnemyMovement>();
        if (enemies.Length < 1) return;

        Transform closestEnemy = enemies[0].transform;
        foreach (EnemyMovement enemy in enemies)
        {
            closestEnemy = GetClosest(closestEnemy, enemy.transform);
        }
        target = closestEnemy;
    }

    public void Init(Tower tower)
    {
        this.attackRange = tower.attackRange;
        this.damage = tower.damage;
        m_tower = tower;
    }

    public void RotateToEnemy(Transform target)
    {
        Vector3 relativePos = target.position - transform.position;
        Quaternion newRot = Quaternion.LookRotation(relativePos, Vector3.up);
        float newY = newRot.eulerAngles.y;

        iTween.RotateTo(turret, iTween.Hash(
            "y", newY,
            "delay", delay,
            "easetype", easeType,
            "time", rotateTime
        ));
    }

    private Transform GetClosest(Transform transformA, Transform transformB)
    {
        var disToA = Vector3.Distance(transform.position, transformA.position);
        var disToB = Vector3.Distance(transform.position, transformB.position);

        if (disToA < disToB)
        {
            return transformA;
        }
        return transformB;
    }
}
