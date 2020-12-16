using LTD.Database;
using LTD.Towers;
using UnityEngine;

namespace LTD.Controller
{
    public class SelectionController : MonoBehaviour
    {
        GameObject hoveredObject;
        Selection selection;
        TowerFactory towerFactory;
        Graph graph;
        UnitDatabase unitDatabase;

        readonly float maxRayDist = 1000f;

        private void Start()
        {
            selection = gameObject.GetComponent<Selection>();
            towerFactory = FindObjectOfType<TowerFactory>();
            graph = FindObjectOfType<Graph>();
            unitDatabase = FindObjectOfType<UnitDatabase>();
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            bool hasHit = Physics.Raycast(ray, out hitInfo, maxRayDist);

            if (hasHit)
            {
                GameObject go = hitInfo.collider.gameObject;
                if (Input.GetMouseButtonDown(0))
                {
                    selection.SetSelectedObject(go);

                    if (graph.IsWithinBounds(Mathf.RoundToInt(go.transform.position.x), Mathf.RoundToInt(go.transform.position.z)))
                    {
                        Node node = graph.GetNodeAt(Mathf.RoundToInt(go.transform.position.x), Mathf.RoundToInt(go.transform.position.z));
                        //Debug.Log(node.xIndex + "," + node.yIndex);
                        if (!unitDatabase.IsEnemyInArea(node))
                        {
                            towerFactory.SpawnTower(node);
                        }
                    }
                }
                HoveredObject(go);
            }
            else
            {
                ClearSelection();
            }
        }

        void HoveredObject(GameObject obj)
        {
            if (hoveredObject != null)
            {
                if (obj == hoveredObject)
                    return;

                ClearSelection();
            }

            hoveredObject = obj;
            //Debug.Log(hoveredObject.name + " is hovered.");
        }

        void ClearSelection()
        {
            if (hoveredObject == null)
                return;

            hoveredObject = null;
        }
    }

}
