using System;
using UnityEngine;

public class TowerFactory : MonoBehaviour
{
    [SerializeField] GameObject fireTower;
    public static event EventHandler OnTowerSpawned;
    TowerDatabase towerDatabase;

    private void Start()
    {
        towerDatabase = FindObjectOfType<TowerDatabase>();
    }

    public void SpawnTower(Node node)
    {
        if (node.nodeType != NodeType.Blocked)
        {
            GameObject instance = Instantiate(fireTower, node.position, Quaternion.identity, this.transform);
            TowerView towerView = instance.GetComponent<TowerView>();
            TowerHealth towerHealth = instance.GetComponent<TowerHealth>();
            if (towerView != null)
            {
                Tower tower = new Tower(node);
                towerView.Init(tower);
                towerHealth.Init(tower);
                tower.name = towerView.name;
                towerDatabase.AddTowerHealth(towerHealth);
                towerDatabase.AddTower(node, tower);
            }
            node.nodeType = NodeType.Blocked;

            OnTowerSpawned?.Invoke(this, EventArgs.Empty);
        }
    }
}
