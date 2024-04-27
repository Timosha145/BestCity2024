using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Road : MonoBehaviour
{
    const float STRAIGHT_ROAD_MIN_LENGTH = 1.5f;
    const float CHANCE_CROSSING_ROAD = 0.2f;

    [SerializeField, Range(0f, 5f)] private float _checkNodesRadius = 1.5f;
    [field: SerializeField] public List<RoadNode> nodes { get; private set; } = new List<RoadNode>();
    [field: SerializeField] public RoadType type { get; private set; } = RoadType.Other;
    [field: SerializeField] public Road crossingVersion { get; private set; }

    private Renderer _renderer;
    public List<Road> collidingRoads { get; private set; } = new List<Road>();

    public enum RoadType
    {
        Corner,
        Straight,
        Preview,
        Other
    }

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    public List<Road> GetCollidingRoads(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, _checkNodesRadius);
        List<Road> collidingRodes = new List<Road>();

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Road collidingRoad) && collidingRoad != this)
            {
                collidingRodes.Add(collidingRoad);
            }
        }

        return collidingRodes;
    }

    public Road GetSuitableRoad()
    {
        List<Road> collidingRodes = GetCollidingRoads(transform.position);

        foreach (Road road in PlacementSystem.Instance.roadVariants)
        {
            if (collidingRodes.Count == 2)
            {
                bool isStraight = Vector3.Distance(collidingRodes[0].transform.position, collidingRodes[1].transform.position) > STRAIGHT_ROAD_MIN_LENGTH;

                if ((isStraight && road.type == RoadType.Straight) || (!isStraight && road.type == RoadType.Corner))
                {
                    float chance = Random.Range(0, 1f);
                    return chance <= CHANCE_CROSSING_ROAD ? road.crossingVersion ?? road : road;
                }
            }
            else if (road.nodes.Count == collidingRodes.Count)
            {
                return Random.Range(0, 1f) <= CHANCE_CROSSING_ROAD ? road.crossingVersion ?? road : road;
            }
        }

        return this;
    }

    public bool AreNodesConnected()
    {
        return nodes.All(node => node.IsCollidingOtherRoad());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 0f, 1f, 0.3f);
        Gizmos.DrawSphere(transform.position, _checkNodesRadius);
    }

    public void ChangeSelectMaterial(Material material)
    {
        if (type == RoadType.Preview && _renderer && _renderer.material != material)
        {
            _renderer.material = material;
        }
    }
}