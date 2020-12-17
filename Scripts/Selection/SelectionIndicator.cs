using UnityEngine;

namespace LTD.Controller
{
    public class SelectionIndicator : MonoBehaviour
    {
        [SerializeField] GameObject selectionIndicator;
        SelectionController selectionController;

        private void Start()
        {
            selectionController = FindObjectOfType<SelectionController>();
        }

        private void Update()
        {
            if (selectionController.HoveredGO != null)
            {
                transform.position = selectionController.HoveredGO.transform.position;
            }
        }

        public void HideSelectionIndicator()
        {
            selectionIndicator.SetActive(false);
        }

        public void ShowSelectionIndicator()
        {
            selectionIndicator.SetActive(true);
        }
    }
}

