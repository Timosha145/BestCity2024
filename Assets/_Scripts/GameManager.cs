using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [field: SerializeField] public int population { get; set; }
    [field: SerializeField] public int employed { get; private set; }
    [field: SerializeField] public float money { get; set; }
    [field: SerializeField] public float materials { get; set; }
    [field: SerializeField] public float products { get; set; }

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

    public int Employ(int people)
    {
        int employedSuccess = Mathf.Clamp(people, 0, population - employed);
        employed += employedSuccess;

        return employedSuccess;
    }

    public void Unemploy(int people)
    {
        employed -= people;
    }
}
