using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RoadNode : MonoBehaviour
{
    [field: SerializeField] public Road road { get; private set; }
    [SerializeField] public float _checkRadius = 0.2f;

    public RoadNode collidingNode { get; private set; }

    public bool TryGetCollidingNode(out RoadNode collidingNode)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _checkRadius);
        collidingNode = null;

        foreach (Collider collider in colliders)
        {
            if (collider.transform != transform && collider.TryGetComponent(out collidingNode))
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}
