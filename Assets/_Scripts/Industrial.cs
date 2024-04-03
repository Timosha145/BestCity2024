using UnityEngine;

public abstract class Industrial : Building
{
    public IndustrialSO industrialSO;

    // Количество продовольствия, производимое одним работником в день
    private const int FoodProductionPerWorker = 5;

    // Минимальное количество работников, необходимых для производства продовольствия
    private const int MinimumWorkersRequired = 3;

    public override void PerformAction()
    {
        // Рассчитываем количество работников в здании
        int workers = CalculateWorkers();
        // Рассчитываем производство продовольствия
        int foodProduction = workers * FoodProductionPerWorker;
        // Увеличиваем количество продовольствия в игре
        GameManager.Instance.IncreaseFood(foodProduction);
    }

    // Метод для расчета количества работников в здании
    private int CalculateWorkers()
    {
        // Примерная логика: количество работников зависит от уровня развития здания
        int buildingLevel = buildingSO.Level;
        int workers = buildingLevel * 2; // Например, каждый уровень добавляет 2 работника
        // Гарантируем, что количество работников не опускается ниже минимального
        workers = Mathf.Max(workers, MinimumWorkersRequired);
        return workers;
    }
}
