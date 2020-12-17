using LTD.Database;
using LTD.EnemyUnits;
using LTD.Map;
using System.Collections;
using UnityEngine;

namespace LTD.Controller
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] GameObject enemyViewGO;
        float secsBetweenSpawnTime = 1f;
        bool isSpawning = false;

        public IEnumerator Init(Node startNode, int numberOfEnemies)
        {
            UnitDatabase unitDatabase = FindObjectOfType<UnitDatabase>();
            isSpawning = true;
            while (numberOfEnemies != 0)
            {
                if (enemyViewGO != null && startNode.position != null)
                {
                    GameObject instance = Instantiate(enemyViewGO, startNode.position, Quaternion.identity, this.transform);
                    Unit unit = new Unit(startNode);
                    EnemyMovement enemyMovement = InitComponents(instance, unit);
                    unitDatabase.AddNewEnemy(enemyMovement, startNode, unit);
                    numberOfEnemies--;
                }
                yield return new WaitForSeconds(secsBetweenSpawnTime);
            }
            isSpawning = false;
        }

        public bool IsSpawning()
        {
            if (isSpawning)
            {
                return true;
            }
            return false;
        }

        private static EnemyMovement InitComponents(GameObject instance, Unit unit)
        {
            EnemyController enemyController = instance.GetComponent<EnemyController>();
            EnemyMovement enemyMovement = instance.GetComponent<EnemyMovement>();
            EnemyHealth enemyHealth = instance.GetComponent<EnemyHealth>();
            UnitView unitView = instance.GetComponent<UnitView>();
            enemyController.Init(unit);
            enemyMovement.Init(unit);
            enemyHealth.Init(unit);
            unitView.Init(unit);
            return enemyMovement;
        }
    }

}
