using LTD.Map;
using LTD.Towers;
using System.Collections.Generic;
using UnityEngine;

namespace LTD.Database
{
    public class TowerDatabase : MonoBehaviour
    {
        Dictionary<Node, Tower> nodeTowerMap = new Dictionary<Node, Tower>();
        List<TowerHealth> towerHealthList = new List<TowerHealth>();
        Graph graph;

        private void Start()
        {
            graph = FindObjectOfType<Graph>();
            TowerHealth.OnTowerDeath += TowerHealth_OnTowerDeath;
        }

        private void TowerHealth_OnTowerDeath(object sender, TowerHealth.OnTowerDeathEventArgs e)
        {
            Debug.Log("Tower Died");
            RemoveTowerHealth(e.tower);
            RemoveTower(e.tower);

        }

        public TowerHealth GetTowerHealth(Tower tower)
        {
            if (tower != null)
            {
                foreach (TowerHealth towerHealth in towerHealthList)
                {
                    if (towerHealth.Tower.node == tower.node)
                    {
                        return towerHealth;
                    }
                }
            }
            return null;
        }

        public void AddTowerHealth(TowerHealth towerHealth)
        {
            if (towerHealth != null)
            {
                towerHealthList.Add(towerHealth);
            }
        }

        void RemoveTowerHealth(Tower tower)
        {
            TowerHealth towerHealth = GetTowerHealth(tower);
            towerHealthList.Remove(towerHealth);
        }

        public void AddTower(Node node, Tower tower)
        {
            if (node != null || tower != null)
            {
                if (nodeTowerMap.ContainsKey(node))
                {
                    Debug.Log("There is already a tower here.");
                }
                else
                {
                    nodeTowerMap.Add(node, tower);
                }
            }
        }

        void RemoveTower(Tower tower)
        {
            if (tower != null)
            {
                nodeTowerMap.Remove(tower.node);
                tower.node.nodeType = NodeType.Open;
            }
        }

        public Tower FindClosestTower(Node enemyNode)
        {
            if (enemyNode != null)
            {
                Tower closestTower = null;
                float shortestTowerDist = Mathf.Infinity;

                if (nodeTowerMap.Count < 1)
                {
                    Debug.Log("There are no towers.");
                    return null;
                }

                foreach (Node node in nodeTowerMap.Keys)
                {
                    float nodeDist = graph.GetNodeDistance(enemyNode, node);
                    if (nodeDist < shortestTowerDist)
                    {
                        shortestTowerDist = nodeDist;
                        closestTower = nodeTowerMap[node];
                    }
                }

                //Debug.Log(closestNode.position);
                return closestTower;
            }
            return null;
        }

        public bool ContainsTowers(Node node)
        {
            if (node != null)
            {
                return nodeTowerMap.ContainsKey(node);
            }
            return false;
        }
    }

}
