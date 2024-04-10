using UnityEngine;

[CreateAssetMenu(menuName = "Building System/Standard")] // Добавляет возможность создать SO в панеле быстро доступа при ПКМ по иерархии
public class BuildingSO : ScriptableObject
{
    [field: SerializeField] public int cost { get; private set; }
    [field: SerializeField] public int waterRequirement { get; private set; }
    [field: SerializeField] public int electricityRequirement { get; private set; }
}
