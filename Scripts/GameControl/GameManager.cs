using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    TileController tileContoller;
    EnemySpawner enemySpawner;
    List<Wave> waves = new List<Wave>();
    UnitDatabase unitDatabase;

    int startingWaveIdx = 0;
    float timeBetweenWaves = 10f;
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
        isWaveInProgress = true;
    }

    private void Start()
    {
        SpawnEnemies(startingWaveIdx);
    }

    private void Update()
    {
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
            waves.Add(new Wave(10));
        }
    }
}
