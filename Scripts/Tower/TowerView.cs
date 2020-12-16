using UnityEngine;

namespace LTD.Towers
{
    public class TowerView : MonoBehaviour
    {
        public GameObject tower;
        Tower m_tower;

        int cost;

        public int Cost { get => cost; }

        public void Init(Tower tower)
        {
            if (tower != null)
            {
                gameObject.name = "Tower (" + tower.node.xIndex + "," + tower.node.yIndex + ")";
                gameObject.transform.position = tower.position;
                m_tower = tower;
                cost = tower.cost;
            }
        }
    }
}

