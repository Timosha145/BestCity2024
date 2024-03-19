using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSO : ScriptableObject
{
    public int Width { get; private set; } // Ширина в тайлах
    public int Length { get; private set; } // Длина в тайлах
    public float BuildSpeed { get; private set; } // Скорость постройки в секундах
    public int WaterRequirement { get; private set; } // Потребность в воде (цифровое значение)
    public int ElectricityRequirement { get; private set; } // Потребность в электричестве (киловаты)
}
