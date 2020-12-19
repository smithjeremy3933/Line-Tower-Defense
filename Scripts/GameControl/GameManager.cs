using LTD.Database;
using LTD.EnemyUnits;
using LTD.Map;
using LTD.Towers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LTD.Controller
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] Text livesNum;
        [SerializeField] Text cashNum;

        TileController tileContoller;
        EnemySpawner enemySpawner;
        List<Wave> waves = new List<Wave>();
        UnitDatabase unitDatabase;

        int _lives = 10;
        int money = 500;
        int startingWaveIdx = 0;
        float timeBetweenWaves = 30f;
        int numberOfWaves = 10;
        bool isWaveInProgress = false;
        float intermissionTimer = 0;

        private void Awake()
        {
            tileContoller = FindObjectOfType<TileController>();
            enemySpawner = FindObjectOfType<EnemySpawner>();
            unitDatabase = FindObjectOfType<UnitDatabase>();
            tileContoller.Init();
            CreateWaves();
        }

        private void Start()
        {
            EnemyHealth.OnEnemyScored += EnemyHealth_OnEnemyScored;
            EnemyHealth.OnEnemyDeath += EnemyHealth_OnEnemyDeath;
        }

        private void Update()
        {
            DisplayResources();
            if (isWaveInProgress && unitDatabase.IsEmpty() && !enemySpawner.IsSpawning())
            {
                Debug.Log("No more enemies. Spawn next wave after time between waves.");
                isWaveInProgress = false;
            }
            else if (!isWaveInProgress)
            {
                intermissionTimer += Time.deltaTime;
                if (intermissionTimer >= timeBetweenWaves)
                {
                    CycleWave();
                    RestartTimers();
                }
            }
        }

        private void DisplayResources()
        {
            int lives = _lives;
            int cash = money;
            livesNum.text = lives.ToString();
            cashNum.text = cash.ToString();
        }

        public void DecreaseMoney(TowerView tower)
        {
            if (tower == null) return;
            money -= tower.Cost;
        }

        private void EnemyHealth_OnEnemyDeath(object sender, EnemyHealth.OnEnemyDeathEventArgs e)
        {
            money += 100;
        }

        private void EnemyHealth_OnEnemyScored(object sender, EventArgs e)
        {
            _lives--;
        }

        private void SpawnEnemies(int waveIdx)
        {
            StartCoroutine(enemySpawner.Init(tileContoller.StartNode, waves[waveIdx].numberOfEnemies));
            waveIdx++;
        }

        private void RestartTimers()
        {
            intermissionTimer = 0;
        }

        private void CycleWave()
        {
            isWaveInProgress = true;
            SpawnEnemies(startingWaveIdx);
            Debug.Log("Cycling waves");
        }

        private void CreateWaves()
        {
            for (int i = 0; i < numberOfWaves; i++)
            {
                waves.Add(new Wave(2));
            }
        }
    }
}
