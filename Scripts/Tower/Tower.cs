﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower
{
    public Vector3 position;
    public Node node;
    public string name;
    public int health = 100;
    public float attackRange = 10f;
    public int damage = 5;

    public Tower(Node node)
    {
        this.node = node;
        this.position = node.position;
    }
}
