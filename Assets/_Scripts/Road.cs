using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Road : MonoBehaviour
{
    [SerializeField, Range(0f, 5f)] private float _checkNodesRadius = 1.5f;
    [field: SerializeField] public List<RoadNode> nodes { get; private set; } = new List<RoadNode>();
    [field: SerializeField] public string roadName { get; private set; }

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

    public Road GetSuitableRoad(Vector3 position)
    {
        List<Road> collidingRodes = GetCollidingRoads(position);

        foreach (Road road in PlacementSystem.Instance.roadVariants)
        {
            if (road.nodes.Count == collidingRodes.Count)
            {
                return road;
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
}
