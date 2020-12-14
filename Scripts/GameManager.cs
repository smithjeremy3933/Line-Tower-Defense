using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    TileController tileContoller;
    EnemySpawner enemySpawner;

    private void Awake()
    {
        tileContoller = FindObjectOfType<TileController>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        tileContoller.Init();
    }

    private void Start()
    {
        StartCoroutine(enemySpawner.Init(tileContoller.StartNode));
    }
}
