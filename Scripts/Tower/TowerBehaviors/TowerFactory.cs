using LTD.Controller;
using LTD.Database;
using LTD.Map;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LTD.Towers
{
    public class TowerFactory : MonoBehaviour
    {
        [SerializeField] GameObject fireTower;
        [SerializeField] List<GameObject> towerList = new List<GameObject>();

        public static event EventHandler OnTowerSpawned;

        GameObject _selectedTower = null;
        TowerDatabase _towerDatabase;
        GameManager _gameManager;
        int _fireTowerIdx = 0;
        int _iceTowerIdx = 1;
        bool _isFTSelected = true;
        bool _isITSelected = false;

        private void Start()
        {
            _towerDatabase = FindObjectOfType<TowerDatabase>();
            _gameManager = FindObjectOfType<GameManager>();
        }

        private void Update()
        {
            if (SetTowerOne())
            {
                Debug.Log("Set tower one.");
                _selectedTower = towerList[_fireTowerIdx];
                _isFTSelected = true;
                _isITSelected = false;
            }
            else if (SetTowerTwo())
            {
                Debug.Log("Set Tower two.");
                _selectedTower = towerList[_iceTowerIdx];
                _isFTSelected = false;
                _isITSelected = true;
            }
        }

        public void SpawnTower(Node node)
        {
            if (node.nodeType != NodeType.Blocked)
            {
                GameObject instance = Instantiate(_selectedTower == null ? fireTower : _selectedTower, node.position, Quaternion.identity, this.transform);
                TowerView towerView = instance.GetComponent<TowerView>();
                TowerHealth towerHealth = instance.GetComponent<TowerHealth>();
                TowerAttack towerAttack = instance.GetComponent<TowerAttack>();
                BulletsView bulletsView = instance.GetComponentInChildren<BulletsView>();

                if (towerView != null)
                {
                    Tower tower = GetTowerData(node);
                    Bullets bullets = GetBulletsData(tower);
                    towerView.Init(tower);
                    towerHealth.Init(tower);
                    towerAttack.Init(tower);
                    bulletsView.Init(bullets);
                    tower.name = towerView.name;
                    _towerDatabase.AddTowerHealth(towerHealth);
                    _towerDatabase.AddTower(node, tower);
                    _gameManager.DecreaseMoney(towerView);
                }
                node.nodeType = NodeType.Blocked;

                OnTowerSpawned?.Invoke(this, EventArgs.Empty);
            }
        }

        private Bullets GetBulletsData(Tower tower)
        {
            Bullets bullets = null;
            if (_isFTSelected)
            {
                bullets = new FireBullets(tower);
            }
            else
            {
                bullets = new IceBullets(tower);
            }

            return bullets;
        }

        private Tower GetTowerData(Node node)
        {
            Tower tower = null;
            if (_isFTSelected)
            {
                tower = new FireTower(node);
            }
            else
            {
                tower = new IceTower(node);
            }

            return tower;
        }

        private static bool SetTowerOne()
        {
            return Keyboard.current[Key.Digit1].wasPressedThisFrame;
        }

        private static bool SetTowerTwo()
        {
            return Keyboard.current[Key.Digit2].wasPressedThisFrame;
        }

    }

}
