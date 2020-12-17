using LTD.Database;
using LTD.Map;
using LTD.Towers;
using UnityEngine;
using UnityEngine.InputSystem;

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

        public GameObject HoveredGO { get => hoveredObject; }

        private void Start()
        {
            selection = gameObject.GetComponent<Selection>();
            towerFactory = FindObjectOfType<TowerFactory>();
            graph = FindObjectOfType<Graph>();
            unitDatabase = FindObjectOfType<UnitDatabase>();
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hitInfo;
            bool hasHit = Physics.Raycast(ray, out hitInfo, maxRayDist);

            if (hasHit)
            {
                GameObject go = hitInfo.collider.gameObject;
                if (graph.IsWithinBounds(Mathf.RoundToInt(go.transform.position.x), Mathf.RoundToInt(go.transform.position.z)))
                {
                    if (Mouse.current.leftButton.isPressed)
                    {

                        Node node = graph.GetNodeAt(Mathf.RoundToInt(go.transform.position.x), Mathf.RoundToInt(go.transform.position.z));
                        if (!unitDatabase.IsEnemyInArea(node))
                        {
                            towerFactory.SpawnTower(node);
                        }

                    }
                    else if (Mouse.current.rightButton.isPressed)
                    {
                        selection.SetSelectedObject(go);
                    }
                    HoveredObject(go);
                }   
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
