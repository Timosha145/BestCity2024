using UnityEngine;

[CreateAssetMenu(menuName = "Building System/Commercial")]
public class CommercialSO : BuildingSO
{
    [field: Header("Commercial Settings")]
    [field: SerializeField] public int workerNeededAmount { get; private set; }
    [field: Range(0, 1)]
    [field: SerializeField] public float workerAmountThreshold { get; private set; }
    [field: SerializeField] public int materialsNeededAmount { get; private set; }
    [field: Range(0, 1)]
    [field: SerializeField] public float materialsAmountThreshold { get; private set; }
    [field: Space]
    [field: SerializeField] public float productionRate { get; private set; }
    [field: SerializeField] public int productionAmount { get; private set; }
}