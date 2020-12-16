using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower
{
    public Vector3 position;
    public Node node;
    public string name;
    public int health = 100;
    public float attackRange = 3f;
    public int damage = 5;
    public int cost = 200;

    public Tower(Node node)
    {
        this.node = node;
        this.position = node.position;
    }
}
