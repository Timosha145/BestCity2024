using UnityEngine;

[CreateAssetMenu(menuName = "Building System/Commercial")]
public class CommercialSO : ScriptableObject
{
    public string Name;
    public int Cost;
    public float TimeConstruction;
    public GameObject PrefabBuilding;

    public int revenue { get; private set; } // Доход от здания
    public CommercialType type { get; private set; } // Тип коммерческого здания

    public enum CommercialType
    {
        RetailStore, // Розничный магазин
        Restaurant, // Ресторан
        OfficeBuilding, // Офисное здание
        GasStation, // Заправка
        Mall  // Торговый центр
    }
}