﻿using LTD.Controller;
using LTD.Database;
using LTD.Map;
using System;
using UnityEngine;

namespace LTD.Towers
{
    public class TowerFactory : MonoBehaviour
    {
        [SerializeField] GameObject fireTower;
        public static event EventHandler OnTowerSpawned;
        TowerDatabase towerDatabase;
        GameManager gameManager;

        private void Start()
        {
            towerDatabase = FindObjectOfType<TowerDatabase>();
            gameManager = FindObjectOfType<GameManager>();
        }

        public void SpawnTower(Node node)
        {
            if (node.nodeType != NodeType.Blocked)
            {
                GameObject instance = Instantiate(fireTower, node.position, Quaternion.identity, this.transform);
                TowerView towerView = instance.GetComponent<TowerView>();
                TowerHealth towerHealth = instance.GetComponent<TowerHealth>();
                TowerAttack towerAttack = instance.GetComponent<TowerAttack>();
                BulletsView bulletsView = instance.GetComponentInChildren<BulletsView>();
                if (towerView != null)
                {
                    Tower tower = new Tower(node);
                    Bullets bullets = new Bullets(tower);
                    towerView.Init(tower);
                    towerHealth.Init(tower);
                    towerAttack.Init(tower);
                    bulletsView.Init(bullets);
                    tower.name = towerView.name;
                    towerDatabase.AddTowerHealth(towerHealth);
                    towerDatabase.AddTower(node, tower);
                    gameManager.DecreaseMoney(towerView);
                }
                node.nodeType = NodeType.Blocked;

                OnTowerSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

}
