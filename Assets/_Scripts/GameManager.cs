using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton-паттерн позволяет обеспечить единственный экземпляр GameManager в игре
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    Debug.LogError("GameManager instance not found in the scene.");
                }
            }
            return instance;
        }
    }

    // Общие ресурсы игры
    private int food;
    private int materials;
    private int population;

    // Методы для управления ресурсами
    public void IncreaseFood(int amount)
    {
        food += amount;
    }

    public void DecreaseFood(int amount)
    {
        food -= amount;
        // Проверка на отрицательное количество ресурса, если нужно
        if (food < 0)
        {
            food = 0;
        }
    }

    public void IncreaseMaterials(int amount)
    {
        materials += amount;
    }

    public void DecreaseMaterials(int amount)
    {
        materials -= amount;
        // Проверка на отрицательное количество ресурса, если нужно
        if (materials < 0)
        {
            materials = 0;
        }
    }

    // Методы для управления населением
    public void IncreasePopulation(int amount)
    {
        population += amount;
    }

    public void DecreasePopulation(int amount)
    {
        population -= amount;
        // Проверка на отрицательное количество населения, если нужно
        if (population < 0)
        {
            population = 0;
        }
    }
}
