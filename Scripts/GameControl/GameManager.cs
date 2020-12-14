using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    TileController tileContoller;
    EnemySpawner enemySpawner;
    List<Wave> waves = new List<Wave>();
    UnitDatabase unitDatabase;

    float timeBetweenWaves = 10f;
    int numberOfWaves = 10;
    bool isWaveInProgress = false;

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
        StartCoroutine(enemySpawner.Init(tileContoller.StartNode, waves[0].numberOfEnemies));      
    }

    private void Update()
    {
        if (isWaveInProgress && unitDatabase.IsEmpty())
        {
            Debug.Log("No more enemies. Spawn next wave after time between waves.");
        }
    }

    private void CreateWaves()
    {
        for (int i = 0; i < numberOfWaves; i++)
        {
            waves.Add(new Wave(10));
        }
    }
}
