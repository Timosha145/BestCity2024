using UnityEngine;

[CreateAssetMenu(menuName = "Building System/Industrial")]
public class IndustrialSO : ScriptableObject
{
    public int productionRate { get; private set; } // Скорость производства
    public IndustrialType type { get; private set; } // Тип промышленного здания

    public enum IndustrialType
    {
        Factory, // Фабрика
        Warehouse, // Склад
        Refinery, // Нефтеперерабатывающий завод
        Mill, // Мельница
        Foundry // Литейный завод
    }
}
