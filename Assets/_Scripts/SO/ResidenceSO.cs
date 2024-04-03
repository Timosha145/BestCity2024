using UnityEngine;

[CreateAssetMenu(menuName = "Building System/Residence")]
public class ResidenceSO : ScriptableObject
{
    public int population { get; private set; }
    public ResidenceType type { get; private set; }

    public enum ResidenceType
    {
        Private, // Частный дом
        Apartment, // Квартирный дом
        Dormitory, // Общежитие
        Duplex, // Дуплекс
        Townhouse, // Таунхаус
        Mansion  // Особняк
    }

}
