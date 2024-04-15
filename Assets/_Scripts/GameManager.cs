using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [field: SerializeField] public int population { get; set; }
    [field: SerializeField] public float money { get; set; }
    [field: SerializeField] public float material { get; set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void SubtractMoney(float amount)
    {
        money -= amount;
    }

    public void AddMaterials(float amount)
    {
        material += amount;
    }
}
