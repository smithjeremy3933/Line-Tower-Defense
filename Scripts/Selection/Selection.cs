using LTD.Database;
using LTD.Map;
using LTD.Towers;
using UnityEngine;

namespace LTD.Controller
{
    public class Selection : MonoBehaviour
    {
        GameObject selectedObject;
        Graph graph;
        UnitDatabase unitDatabase;
        TowerDatabase towerDatabase;

        private void Start()
        {
            graph = FindObjectOfType<Graph>();
            unitDatabase = FindObjectOfType<UnitDatabase>();
            towerDatabase = FindObjectOfType<TowerDatabase>();
        }

        public void SetSelectedObject(GameObject go)
        {
            if (IsSelected(go) || go == null) return;
            if (selectedObject != go)
            {
                Node node = graph.GetNodeAt(Mathf.RoundToInt(go.transform.position.x), Mathf.RoundToInt(go.transform.position.z));
                if (unitDatabase.NodeContainsEnemies(node))
                {
                    Unit unit = unitDatabase.GetUnitFromNode(node);
                    Debug.Log(unit.health);
                    selectedObject = go;
                }
                else if (towerDatabase.ContainsTowers(node))
                {
                    Tower tower = towerDatabase.GetTowerFromNode(node);
                    TowerHealth towerHealth = towerDatabase.GetTowerHealth(tower);
                    Debug.Log(tower.Health);
                    selectedObject = towerHealth.gameObject;
                    Debug.Log("Selected GameObject " + selectedObject.name);
                }
                else
                {
                    Debug.Log("No tower or units on node");
                    DeselectObject();
                }         
            }
        }

        public void DeselectObject()
        {
            selectedObject = null;
        }

        public bool IsSelected(GameObject go)
        {
            return selectedObject == go;
        }
    }
}

