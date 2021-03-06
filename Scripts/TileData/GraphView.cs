﻿using UnityEngine;

namespace LTD.Map
{
    public class GraphView : MonoBehaviour
    {
        public GameObject nodeViewPrefab;
        public NodeView[,] nodeViews;
        public Color baseColor = Color.white;
        public Color wallColor = Color.black;

        public void Init(Graph graph)
        {
            if (graph == null)
            {
                Debug.LogWarning("Need graph!");
            }
            nodeViews = new NodeView[graph.Width, graph.Height];

            foreach (Node n in graph.nodes)
            {
                GameObject instance = Instantiate(nodeViewPrefab, Vector3.zero, Quaternion.identity, graph.transform);
                NodeView nodeView = instance.GetComponent<NodeView>();

                if (nodeView != null)
                {
                    nodeView.Init(n);
                    nodeViews[n.xIndex, n.yIndex] = nodeView;

                    if (n.nodeType == NodeType.Blocked)
                    {
                        nodeView.ColorNode(wallColor);
                    }
                    else
                    {
                        nodeView.ColorNode(baseColor);
                    }
                }
            }
        }
    }

}
