using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    GameObject selectedObject;

    public void SetSelectedObject(GameObject go)
    {
        if (selectedObject != go)
        {
            selectedObject = go;
            //Debug.Log("Selected GameObject " + selectedObject);
        }
    }

    public void DeselectObject()
    {
        selectedObject = null;
    }

    public bool IsSelected(GameObject go)
    {
        return selectedObject = go;
    }
}
