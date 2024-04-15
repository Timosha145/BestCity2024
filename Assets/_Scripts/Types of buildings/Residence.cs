using UnityEngine;

public class Residence : Building
{
    [field: SerializeField] public ResidenceSO residenceSO { get; private set; }

    public int currentPopulation { get; private set; }

    private float _timer = 0;
    private int _peopleMoveInCount;

    private void Awake()
    {
        initBuildingSO(residenceSO);
        AssignRandomPeopleMoveInCount();
    }

    private void Update()
    {
        if (!isBuilt)
        {
            return;
        }

        _timer += Time.deltaTime;

        if (_timer > residenceSO.timeToMoveIn && currentPopulation < residenceSO.population)
        {   
            _timer = 0;
            AssignRandomPeopleMoveInCount();
            currentPopulation = Mathf.Clamp(currentPopulation + _peopleMoveInCount, 0, residenceSO.population);
            GameManager.Instance.population += _peopleMoveInCount;
        }
    }

    private void AssignRandomPeopleMoveInCount()
    {
        _peopleMoveInCount = Random.Range(0, residenceSO.population + 1 - currentPopulation);
    }
}
