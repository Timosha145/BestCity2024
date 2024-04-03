using UnityEngine;

public abstract class Residence : Building
{
    public ResidenceSO residenceSO;

    // Количество продовольствия, необходимое для одного жильца в день
    private const int FoodConsumptionPerPerson = 2;

    public override void PerformAction()
    {
        // Рассчитываем количество людей в здании
        int residents = CalculateResidents();
        // Увеличиваем население в игре
        GameManager.Instance.IncreasePopulation(residents);

        // Рассчитываем необходимое количество продовольствия
        int foodNeeded = residents * FoodConsumptionPerPerson;
        // Уменьшаем количество продовольствия в игре
        GameManager.Instance.DecreaseFood(foodNeeded);
    }

    // Метод для расчета количества жителей в здании
    private int CalculateResidents()
    {
        // Примерная логика: количество жителей зависит от уровня развития здания
        int buildingLevel = buildingSO.Level;
        int residents = buildingLevel * 10; // Например, каждый уровень дает возможность проживать 10 жителей
        return residents;
    }
}
