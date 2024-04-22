using UnityEngine;

[CreateAssetMenu(menuName = "Building System/Residence")]
public class ResidenceSO : BuildingSO
{
    [field: Header("Residence Settings")]
    [field: SerializeField] public int population { get; private set; }
    [field: SerializeField] public float timeToMoveIn { get; private set; }
}
