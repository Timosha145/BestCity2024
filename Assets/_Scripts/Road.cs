using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Road : MonoBehaviour
{
    [field: SerializeField] public List<RoadNode> nodes { get; private set; }

    private List<RoadNode> _collidingNodes = new List<RoadNode>();

    public void Build(Vector3 position)
    {
        foreach (RoadNode node in nodes)
        {
            if (node.TryGetCollidingNode(out RoadNode collidingRoad))
            {
                _collidingNodes.Add(collidingRoad);
            }
        }
    }

    public Road GetSuitableRoad(List<Road> roads)
    {
        List<RoadNode> collidingNodes = new List<RoadNode>();

        foreach (RoadNode node in nodes)
        {
            if (node.TryGetCollidingNode(out RoadNode collidingRoad))
            {
                collidingNodes.Add(collidingRoad);
            }
        }

        foreach (Road road in roads)
        {
            if (road.nodes.Count == nodes.Count)
            {
                return road;
            }
        }

        return this;
    }
}
