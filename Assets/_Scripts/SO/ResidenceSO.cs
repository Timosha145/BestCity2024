using UnityEngine;

[CreateAssetMenu(menuName = "Building System/Residence")]
public class ResidenceSO : BuildingSO
{
    [field: SerializeField] public int population { get; private set; }
    [field: SerializeField] public ProductionType type { get; private set; }
}
