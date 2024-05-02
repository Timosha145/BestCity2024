using UnityEngine;

public class RoadNode : MonoBehaviour
{
    [field: SerializeField] public Road road { get; private set; }
    [SerializeField] public float _checkRadius = 0.2f;

    public bool IsCollidingOtherRoad()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _checkRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Road road) && road != this.road)
            {
                return true;
            }
        }

        return false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, _checkRadius);
    }
}
