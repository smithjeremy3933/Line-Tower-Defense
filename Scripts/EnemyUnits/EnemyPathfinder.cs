using LTD.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace LTD.EnemyUnits
{
    public class EnemyPathfinder : MonoBehaviour
    {
        EnemyController enemyController;
        EnemyMovement enemyMovement;
        Graph m_graph;

        PriorityQueue<Node> m_frontierNodes;
        List<Node> m_exploredNodes;
        List<Node> m_pathNodes;
        readonly bool exitOnGoal = true;
        bool isComplete = false;
        int m_iterations = 0;

        private void Start()
        {
            enemyController = GetComponent<EnemyController>();
            enemyMovement = GetComponent<EnemyMovement>();
            m_graph = FindObjectOfType<Graph>();
        }

        public void RecalcPath(Node goalNode)
        {
            InitForUnitPathfinding();
            enemyController.needsPathRecalc = false;
            SearchRoutine(goalNode);
        }

        public void InitForUnitPathfinding()
        {
            if (enemyController.currentNode == null || enemyController.m_goalNode == null || m_graph == null)
            {
                return;
            }

            if (enemyController.currentNode.nodeType == NodeType.Blocked || enemyController.m_goalNode.nodeType == NodeType.Blocked)
            {
                return;
            }

            m_frontierNodes = new PriorityQueue<Node>();
            m_frontierNodes.Enqueue(enemyController.currentNode);
            m_exploredNodes = new List<Node>();
            m_pathNodes = new List<Node>();

            for (int x = 0; x < m_graph.Width; x++)
            {
                for (int y = 0; y < m_graph.Height; y++)
                {
                    m_graph.nodes[x, y].Reset();
                }
            }
            isComplete = false;
            enemyController.currentNode.distanceTravled = 0;
            m_iterations = 0;
        }

        public void SearchRoutine(Node goalNode)
        {
            while (!isComplete)
            {
                if (m_frontierNodes.Count > 0)
                {
                    Node currentNode = m_frontierNodes.Dequeue();
                    m_iterations++;
                    if (!m_exploredNodes.Contains(currentNode))
                    {
                        m_exploredNodes.Add(currentNode);
                    }

                    ExpandFrontierAStar(currentNode, goalNode);

                    if (m_frontierNodes.Contains(goalNode))
                    {
                        m_pathNodes = GetPathNodes(goalNode);

                        StartCoroutine(enemyMovement.FollowPath(m_pathNodes));
                        if (exitOnGoal)
                        {
                            isComplete = true;
                        }
                    }
                }
                else
                {
                    isComplete = true;
                    enemyController.isPathToGoal = false;
                    enemyController.needsPathRecalc = true;
                }
            }
        }

        private void ExpandFrontierAStar(Node node, Node goalNode)
        {
            if (node != null && node.nodeType == NodeType.Open)
            {
                for (int i = 0; i < node.neighbors.Count; i++)
                {
                    if (!m_exploredNodes.Contains(node.neighbors[i]))
                    {
                        float distanceToNeighbor = m_graph.GetNodeDistance(node, node.neighbors[i]);
                        float newDistanceTraveled = distanceToNeighbor + node.distanceTravled + (int)node.nodeType;

                        if (float.IsPositiveInfinity(node.neighbors[i].distanceTravled) || newDistanceTraveled < node.neighbors[i].distanceTravled)
                        {
                            node.neighbors[i].previous = node;
                            node.neighbors[i].distanceTravled = newDistanceTraveled;
                        }

                        if (!m_frontierNodes.Contains(node.neighbors[i]) && m_graph != null)
                        {
                            float distanceToGoal = m_graph.GetNodeDistance(node.neighbors[i], goalNode);
                            node.neighbors[i].priority = node.neighbors[i].distanceTravled + distanceToGoal;
                            m_frontierNodes.Enqueue(node.neighbors[i]);
                        }
                    }
                }
            }
        }

        List<Node> GetPathNodes(Node endNode)
        {
            List<Node> path = new List<Node>();
            if (endNode == null)
            {
                return path;
            }

            path.Add(endNode);

            Node currentNode = endNode.previous;

            while (currentNode != null)
            {
                path.Insert(0, currentNode);
                currentNode.isPlaceable = false;
                currentNode = currentNode.previous;
            }
            return path;
        }
    }

}
