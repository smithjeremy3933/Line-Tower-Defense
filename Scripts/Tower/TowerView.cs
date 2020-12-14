using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerView : MonoBehaviour
{
    public GameObject tower;
    Tower m_tower;

    public void Init(Tower tower)
    {
        if (tower != null)
        {
            gameObject.name = "Tower (" + tower.node.xIndex + "," + tower.node.yIndex + ")";
            gameObject.transform.position = tower.position;
            m_tower = tower;
        }
    }
}
