using UnityEngine;

[CreateAssetMenu(menuName = "Building System/Industrial")]
public class IndustrialSO : BuildingSO
{
    [field: SerializeField] public int workerMaxEmount { get; private set; }
    [field: SerializeField] public float productionRate { get; private set; }
    [field: SerializeField] public int productionEmount { get; private set; }
    [field: SerializeField] public RawMaterialType type { get; private set; }
}

public enum RawMaterialType
{
    Wheat,
    Wood,
    Textile
}
