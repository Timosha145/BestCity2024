using UnityEngine; // Всегда уберай не нужные юзинги

[CreateAssetMenu(menuName = "Building System/Residence")]
public class ResidenceSO : ScriptableObject
{
    public int population { get; private set; }
    public ResidenceType type { get; private set; }

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
