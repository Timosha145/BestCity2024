using UnityEngine;

[CreateAssetMenu(menuName = "Building System/Commercial")]
public class CommercialSO : BuildingSO
{
    [field: SerializeField] public int production { get; private set; }
    [field: SerializeField] public float incomeInterval { get; private set; }
    [field: SerializeField] public ProductionType type { get; private set; }
}

public enum ProductionType
{
    Food,
    Clothes,
    Furniture,
}