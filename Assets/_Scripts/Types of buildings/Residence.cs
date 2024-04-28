using UnityEngine;

public class Residence : Building
{
    [field: Header("Residence Settings")]
    [field: SerializeField] public int population { get; private set; }
    [field: SerializeField] public float residentsCountUpdateRate { get; private set; } = 15f;

    public int currentPopulation { get; private set; }

    private float _timer = 0;
    private int _peopleMoveInCount;

    private void Awake()
    {
        AssignRandomPeopleMoveInCount();
    }

    protected override void Update()
    {
        if (!isBuilt)
        {
            return;
        }

        base.Update();

        _timer += Time.deltaTime;

        if (_timer > residentsCountUpdateRate && currentPopulation < population)
        {   
            _timer = 0;
            AssignRandomPeopleMoveInCount();
            currentPopulation = Mathf.Clamp(currentPopulation + _peopleMoveInCount, 0, population);
            GameManager.Instance.population += _peopleMoveInCount;
        }
    }

    private void AssignRandomPeopleMoveInCount()
    {
        _peopleMoveInCount = Random.Range(0, population + 1 - currentPopulation);
    }
}
