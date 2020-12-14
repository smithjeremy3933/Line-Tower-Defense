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
        if (enemyAttack.IsTarget())
        {
            AttackBehavior();
            return;
        }
        else if (!enemyAttack.IsTarget() && isAttacking)
        {
            CancelAttackBehavior();
        }
        else if (isAttacking)
        {
            return;
        }
        else if (isPathToGoal && needsPathRecalc && !isMoving && !isAttacking)
        {
            PathfindToGoalBehavior();
            return;
        }
        else if (!isPathToGoal && needsPathRecalc && !isMoving && !isAttacking)
        {
            PathfindToClosestTowerBehavior();
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

    private void AttackBehavior()
    {
        enemyAttack.Attack(enemyAttack.Target);
    }

    private void CancelAttackBehavior()
    {
        Debug.Log("There is no target but i am still in attack mode.");
        GetComponent<Animator>().SetBool("punch", false);
        isPathToGoal = true;
        needsPathRecalc = true;
        isAttacking = false;
    }

    private void PathfindToClosestTowerBehavior()
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
            enemyAttack.SetTarget(towerDatabase.GetTowerHealth(closestTower));
        }
    }

    private void PathfindToGoalBehavior()
    {
        enemyPathfinder.RecalcPath(m_goalNode);
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
