using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidenceSO : ScriptableObject
{
    public int Population { get; private set; } // Количество жителей
    public ResidenceType Type { get; private set; } // Тип дома
    public enum ResidenceType
    {
        Private,
        Apartment,
        Dormitory,
        Duplex,
        Townhouse,
        Mansion,
    }
}
