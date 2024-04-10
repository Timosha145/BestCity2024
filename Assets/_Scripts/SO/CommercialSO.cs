using UnityEngine;

[CreateAssetMenu(menuName = "Building System/Commercial")]
public class CommercialSO : BuildingSO
{
    [field: SerializeField] public int workerMaxEmount { get; private set; }
    [field: SerializeField] public float productionRate { get; private set; }
    [field: SerializeField] public int productionEmount { get; private set; }
    [field: SerializeField] public RawMaterialType neededMaterial { get; private set; }
    [field: SerializeField] public ProductionType type { get; private set; }
}

public enum ProductionType
{
    Food,
    Clothes,
    Furniture,
}