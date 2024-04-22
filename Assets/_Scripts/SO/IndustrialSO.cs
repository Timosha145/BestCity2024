using UnityEngine;

[CreateAssetMenu(menuName = "Building System/Industrial")]
public class IndustrialSO : BuildingSO
{
    [field: Header("Industrial Settings")]
    [field: SerializeField] public int workerNeededAmount { get; private set; }
    [field: Range(0, 1)]
    [field: SerializeField] public float workerAmountThreshold { get; private set; }
    [field: SerializeField] public float productionRate { get; private set; }
    [field: SerializeField] public int productionAmount { get; private set; }
}
