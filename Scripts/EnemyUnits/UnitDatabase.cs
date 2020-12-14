using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDatabase : MonoBehaviour
{
    Dictionary<EnemyMovement, Node> enemymovementNodeMap = new Dictionary<EnemyMovement, Node>();
    Dictionary<Node, List<EnemyMovement>> nodeEnemymovementsMap = new Dictionary<Node, List<EnemyMovement>>();

    private void Start()
    {
        EnemyMovement.OnEnemyMoved += EnemyMovement_OnEnemyMoved;
    }

    public void AddNewEnemy(EnemyMovement enemyMovement, Node startNode)
    {
        if (!nodeEnemymovementsMap.ContainsKey(startNode))
        {
            enemymovementNodeMap.Add(enemyMovement, startNode);

            List<EnemyMovement> enemyMovements = new List<EnemyMovement>();
            enemyMovements.Add(enemyMovement);
            nodeEnemymovementsMap.Add(startNode, enemyMovements);
        }
        else
        {
            nodeEnemymovementsMap[startNode].Add(enemyMovement);
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

    private void EnemyMovement_OnEnemyMoved(object sender, EnemyMovement.OnEnemyMovedEventArgs e)
    {
        UpdateMovement(e.enemyMovement, e.oldNode, e.newNode);
    }

    void UpdateMovement(EnemyMovement enemyMovement,Node oldNode, Node newNode)
    {
        if (enemyMovement != null || oldNode != null || newNode != null)
        {
            enemymovementNodeMap.Remove(enemyMovement);
            enemymovementNodeMap[enemyMovement] = newNode;
            
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
