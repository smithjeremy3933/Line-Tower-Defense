using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    EnemyAttack enemyAttack;
    EnemyMovement enemyMovement;
    EnemyPathfinder enemyPathfinder;

    TileController tileController;
    TowerDatabase towerDatabase;
    Graph m_graph;

    public Node m_goalNode;
    public Node currentNode;

    public bool isMoving = false;
    public bool needsPathRecalc = false;
    public bool isPathToGoal = true;
    public bool isAttacking = false;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (enemyAttack.Target != null)
        {
            enemyAttack.Attack(enemyAttack.Target);
            return;
        }

        if (enemyAttack.Target == null && isAttacking)
        {
            Debug.Log("There is no target but i am still in attack mode.");
            GetComponent<Animator>().SetBool("punch", false);
            isPathToGoal = true;
            needsPathRecalc = true;
            isAttacking = false;
        }

        if (isAttacking) return;

        if (isPathToGoal && needsPathRecalc && !isMoving && !isAttacking)
        {
            enemyPathfinder.RecalcPath(m_goalNode);
            return;
        }

        if (!isPathToGoal && needsPathRecalc && !isMoving && !isAttacking)
        {
            Debug.Log("No path to goal");
            isAttacking = true;
            Tower closestTower = towerDatabase.FindClosestTower(currentNode);
            Node closestNodeToTower = FindClosestNodeToTower(closestTower);
            if (closestTower != null && closestNodeToTower != null)
            {
                enemyPathfinder.RecalcPath(closestNodeToTower);
                isPathToGoal = false;
                needsPathRecalc = false;
                enemyMovement.FaceDestination(closestTower.node);
                enemyAttack.Target = towerDatabase.GetTowerHealth(closestTower);
            }
        }
    }

    private void Init()
    {
        enemyAttack = GetComponent<EnemyAttack>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyPathfinder = GetComponent<EnemyPathfinder>(); 
        tileController = FindObjectOfType<TileController>();
        towerDatabase = FindObjectOfType<TowerDatabase>();
        m_graph = FindObjectOfType<Graph>();
        m_goalNode = tileController.GoalNode;
        currentNode = tileController.StartNode;
        enemyPathfinder.InitForUnitPathfinding();
        enemyPathfinder.SearchRoutine(m_goalNode);
        TowerFactory.OnTowerSpawned += TowerFactory_OnTowerSpawned;
    }

    private void TowerFactory_OnTowerSpawned(object sender, System.EventArgs e)
    {
        if (!isAttacking)
        {
            needsPathRecalc = true;
        }
    }

    Node FindClosestNodeToTower(Tower closestTower)
    {
        if (closestTower != null)
        {
            Node closestNode = null;
            float shortestNodeDist = Mathf.Infinity;

            foreach (Node node in closestTower.node.neighbors)
            {
                float nodeDist = m_graph.GetNodeDistance(currentNode, node);
                if (nodeDist < shortestNodeDist && !towerDatabase.ContainsTowers(node))
                {
                    shortestNodeDist = nodeDist;
                    closestNode = node;
                }
            }
            return closestNode;
        }
        return null;
    }
}
