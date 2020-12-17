using LTD.Map;
using UnityEngine;

public class Unit
{
    public Vector3 position;
    public Node node;
    public int health = 100;
    public string name = "Orc";

    public Unit(Node node)
    {
        this.node = node;
        this.position = node.position;
    }
}
