using LTD.EnemyUnits;
using System.Collections.Generic;
using UnityEngine;

namespace LTD.Database
{
    public class UnitDatabase : MonoBehaviour
    {
        Dictionary<Node, List<EnemyMovement>> nodeEnemymovementsMap = new Dictionary<Node, List<EnemyMovement>>();
        List<Unit> unitsList = new List<Unit>();

        private void Start()
        {
            EnemyMovement.OnEnemyMoved += EnemyMovement_OnEnemyMoved;
            EnemyHealth.OnEnemyDeath += EnemyHealth_OnEnemyDeath;
        }

        public List<Node> GetNodesWithEnemies()
        {
            List<Node> nodesWithEnemies = new List<Node>();
            foreach (Node node in nodeEnemymovementsMap.Keys)
            {
                nodesWithEnemies.Add(node);
            }
            return nodesWithEnemies;
        }

        public void AddNewEnemy(EnemyMovement enemyMovement, Node startNode, Unit enemyUnit)
        {
            if (enemyMovement != null && startNode != null && enemyUnit != null)
            {
                unitsList.Add(enemyUnit);

                if (!nodeEnemymovementsMap.ContainsKey(startNode))
                {
                    List<EnemyMovement> enemyMovements = new List<EnemyMovement>();
                    enemyMovements.Add(enemyMovement);
                    nodeEnemymovementsMap.Add(startNode, enemyMovements);
                }
                else
                {
                    nodeEnemymovementsMap[startNode].Add(enemyMovement);
                }
            }
        }

        public bool IsEnemyInArea(Node node)
        {
            foreach (Node neighbor in node.neighbors)
            {
                if (nodeEnemymovementsMap.ContainsKey(neighbor) || nodeEnemymovementsMap.ContainsKey(node))
                {
                    Debug.Log("Enemy in area");
                    return true;
                }
            }
            Debug.Log("Enemy not in area");
            return false;
        }

        public bool IsEmpty()
        {
            if (unitsList.Count < 1)
            {
                return true;
            }
            return false;
        }

        private void EnemyHealth_OnEnemyDeath(object sender, EnemyHealth.OnEnemyDeathEventArgs e)
        {
            RemoveEnemy(e.enemyMovement);
        }

        private void RemoveEnemy(EnemyMovement enemyMovement)
        {
            if (enemyMovement == null) return;
            Unit unit = GetUnitFromEnemyMovement(enemyMovement);
            Node currentNode = unit.node;
            foreach (Node node in nodeEnemymovementsMap.Keys)
            {
                if (currentNode == node)
                {
                    foreach (EnemyMovement enemy in nodeEnemymovementsMap[node])
                    {
                        if (enemyMovement == enemy)
                        {
                            nodeEnemymovementsMap[node].Remove(enemy);
                            unitsList.Remove(unit);
                            Debug.Log("Removed enemy from map after death");
                            return;
                        }
                    }
                }
            }
        }

        private Unit GetUnitFromEnemyMovement(EnemyMovement enemyMovement)
        {
            if (enemyMovement != null)
            {
                foreach (Unit unit in unitsList)
                {
                    if (enemyMovement.Unit == unit)
                    {
                        return unit;
                    }
                }
            }
            return null;
        }

        private void EnemyMovement_OnEnemyMoved(object sender, EnemyMovement.OnEnemyMovedEventArgs e)
        {
            UpdateMovement(e.enemyMovement, e.oldNode, e.newNode);
        }

        private void UpdateMovement(EnemyMovement enemyMovement, Node oldNode, Node newNode)
        {
            if (enemyMovement != null || oldNode != null || newNode != null)
            {
                Unit unit = GetUnitFromEnemyMovement(enemyMovement);
                unit.node = newNode;
                unit.position = newNode.position;
                nodeEnemymovementsMap[oldNode].Remove(enemyMovement);
                if (nodeEnemymovementsMap[oldNode].Count < 1)
                {
                    nodeEnemymovementsMap.Remove(oldNode);
                }

                if (nodeEnemymovementsMap.ContainsKey(newNode))
                {
                    nodeEnemymovementsMap[newNode].Add(enemyMovement);
                }
                else
                {
                    List<EnemyMovement> enemyMovements = new List<EnemyMovement>();
                    enemyMovements.Add(enemyMovement);
                    nodeEnemymovementsMap.Add(newNode, enemyMovements);
                }
            }
        }
    }
}

