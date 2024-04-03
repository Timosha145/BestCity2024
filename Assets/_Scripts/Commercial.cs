using UnityEngine;

public abstract class Commercial : Building
{
    public CommercialSO commercialSO;

    // Количество продовольствия, производимое одним работником в день
    private const int FoodProductionPerWorker = 10;

    // Минимальное количество материалов, необходимых для производства продовольствия
    private const int MinimumMaterialsRequired = 20;

    public override void PerformAction()
    {
        // Рассчитываем производство продовольствия
        int foodProduction = CalculateFoodProduction();

        // Увеличиваем количество продовольствия в игре
        GameManager.Instance.IncreaseFood(foodProduction);

        // Рассчитываем количество работников для найма
        int hireEmployees = CalculateEmployeesToHire();

        // Увеличиваем население в игре
        GameManager.Instance.IncreasePopulation(hireEmployees);

        // Рассчитываем необходимые материалы для производства продовольствия
        int materialsNeeded = CalculateMaterialsNeeded();

        // Уменьшаем количество материалов в игре
        GameManager.Instance.DecreaseMaterials(materialsNeeded);
    }

    // Метод для расчета производства продовольствия
    private int CalculateFoodProduction()
    {
        // Рассчитываем общее количество работников в здании
        int totalWorkers = CalculateEmployees();
        // Рассчитываем общее производство продовольствия на основе количества работников
        int totalProduction = totalWorkers * FoodProductionPerWorker;
        return totalProduction;
    }

    protected abstract int CalculateEmployees();

    // Метод для расчета количества работников для найма
    private int CalculateEmployeesToHire()
    {
        // Примерная логика: количество работников зависит от уровня развития здания
        int buildingLevel = buildingSO.Level;
        int hireEmployees = buildingLevel * 5; // Например, каждый уровень дает возможность нанять 5 работников
        return hireEmployees;
    }

    // Метод для расчета необходимых материалов для производства продовольствия
    private int CalculateMaterialsNeeded()
    {
        // Примерная логика: количество материалов зависит от уровня развития здания
        int buildingLevel = buildingSO.Level;
        int materialsNeeded = buildingLevel * 10; // Например, каждый уровень требует 10 единиц материалов
        // Гарантируем, что необходимое количество материалов не опускается ниже минимального
        materialsNeeded = Mathf.Max(materialsNeeded, MinimumMaterialsRequired);
        return materialsNeeded;
    }
}
