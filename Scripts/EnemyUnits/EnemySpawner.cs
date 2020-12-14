using System;
using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyViewGO;
    float secsBetweenSpawnTime = 1f;
    int numberOfEnemies = 10;

    public IEnumerator Init(Node startNode)
    {
        UnitDatabase unitDatabase = FindObjectOfType<UnitDatabase>();

        while (numberOfEnemies != 0)
        {
            if (enemyViewGO != null && startNode.position != null)
            {
                GameObject instance = Instantiate(enemyViewGO, startNode.position, Quaternion.identity, this.transform);
                EnemyMovement enemyMovement = instance.GetComponent<EnemyMovement>();
                unitDatabase.AddNewEnemy(enemyMovement, startNode);
                numberOfEnemies--;
            }
            yield return new WaitForSeconds(secsBetweenSpawnTime);
        }      
    }
}
