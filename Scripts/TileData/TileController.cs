using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public MapData mapData;
    public Graph graph;
    public Pathfinder pathfinder;

    public int startX = 0;
    public int startY = 0;
    public int goalX = 6;
    public int goalY = 5;
    Node m_startNode;
    Node m_goalNode;

    public Node StartNode { get => m_startNode; }
    public Node GoalNode { get => m_goalNode; }

    public void Init()
    {
        if (mapData != null && graph != null)
        {
            int[,] mapInstance = mapData.MakeMap();
            graph.Init(mapInstance);

            GraphView graphView = graph.gameObject.GetComponent<GraphView>();

            if (graphView != null)
            {
                graphView.Init(graph);
            }

            if (graph.IsWithinBounds(startX, startY) && graph.IsWithinBounds(goalX, goalY) && pathfinder != null)
            {
                Node startNode = graph.nodes[startX, startY];
                m_startNode = startNode;
                Node goalNode = graph.nodes[goalX, goalY];
                m_goalNode = goalNode;
                pathfinder.Init(graph, graphView, startNode, goalNode);
            }
        }
    }
}
