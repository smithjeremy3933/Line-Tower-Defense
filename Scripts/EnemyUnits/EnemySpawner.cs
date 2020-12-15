using System;
using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyViewGO;
    float secsBetweenSpawnTime = 1f;

    public IEnumerator Init(Node startNode, int numberOfEnemies)
    {
        UnitDatabase unitDatabase = FindObjectOfType<UnitDatabase>();

        while (numberOfEnemies != 0)
        {
            if (enemyViewGO != null && startNode.position != null)
            {
                GameObject instance = Instantiate(enemyViewGO, startNode.position, Quaternion.identity, this.transform);
                Unit unit = new Unit(startNode);
                EnemyController enemyController = instance.GetComponent<EnemyController>();
                EnemyMovement enemyMovement = instance.GetComponent<EnemyMovement>();
                enemyController.Init(unit);
                enemyMovement.Init(unit);
                unitDatabase.AddNewEnemy(enemyMovement, startNode, unit);
                numberOfEnemies--;
            }
            yield return new WaitForSeconds(secsBetweenSpawnTime);
        }      
    }
}
