using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [field: Header("Prefabs")]
    [field: SerializeField] public List<Residence> residencePrefabs { get; private set; }
    [field: SerializeField] public List<Industrial> industryPrefabs { get; private set; }
    [field: SerializeField] public List<Commercial> commercialPrefabs { get; private set; }
    [field: SerializeField] public List<Building> otherPrefabs { get; private set; }
    [field: SerializeField] public List<Road> roadPrefabs { get; private set; }
    [field: SerializeField] public Road roadPreview { get; private set; }
    [field: Header("Game Settings")]
    [field: Range(0, 1)]
    [field: SerializeField] public float productPerCitizen { get; private set; }
    [field: SerializeField] public float tax { get; private set; }
    [field: SerializeField] public float dayDuration { get; private set; }
    [field: SerializeField] public int population { get; set; }
    [field: SerializeField] public int employed { get; private set; }
    [field: SerializeField] public float jobCount { get; set; }
    [field: SerializeField] public float money { get; set; }
    [field: SerializeField] public float materials { get; set; }
    [field: SerializeField] public float products { get; set; }
    [field: SerializeField] public float producingMaterialsPerDay { get; set; } = 0f;
    [field: SerializeField] public float neededMaterialsPerDay { get; set; } = 0f;
    [field: SerializeField] public float producingProductsPerDay { get; set; } = 0f;
    [field: SerializeField] public float neededProductsPerDay { get; set; } = 0f;

    public Mode currentMode { get; private set; } = Mode.idle;
    public event EventHandler<ModeEventArgs> onChangeMode;
    public event EventHandler onDayPassed;

    private float _dayTimer;

    public class ModeEventArgs : EventArgs
    {
        public readonly Mode newMode;

        public ModeEventArgs(Mode mode)
        {
            newMode = mode;
        }
    }

    public enum Mode
    {
        building,
        destroying,
        idle
    }

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

    private void Update()
    {
        _dayTimer += Time.deltaTime;

        if (_dayTimer > dayDuration)
        {
            _dayTimer = 0;
            onDayPassed?.Invoke(null, EventArgs.Empty);
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

    public void ChangeMode(Mode mode)
    {
        currentMode = mode;
        onChangeMode?.Invoke(null, new ModeEventArgs(mode));
    }

    public void PayTaxes(float salary)
    {
        money += salary * tax;
    }

    public float GetMaxMaterials()
    {
        return neededMaterialsPerDay * 5;
    }

    public bool IsMaterialOverstock()
    {
        return materials > GetMaxMaterials();
    }
}
