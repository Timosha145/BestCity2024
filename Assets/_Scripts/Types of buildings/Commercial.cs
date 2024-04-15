using UnityEngine;

public class Commercial : Building
{
    [field: SerializeField] public CommercialSO commercialSO { get; private set; }
    public int currentProduction { get; private set; }
    private float _timer = 0;
    private int _maxIncomePerInterval;


    private void Awake()
    {
        initBuildingSO(commercialSO);
        AssignRandomInComeMoveInCount();
    }

    private void AddMoney(float amount)
    {
        GameManager.Instance.material += amount;
    }

    private void Update()
    {
        if (!isBuilt)
        {
            return;
        }

        _timer += Time.deltaTime;

        if (_timer > commercialSO.incomeInterval && currentProduction < commercialSO.production)
        {   
            _timer = 0;
            AssignRandomInComeMoveInCount();
            currentProduction = Mathf.Clamp(currentProduction + _maxIncomePerInterval, 0, commercialSO.production);
            GameManager.Instance.material += _maxIncomePerInterval;
        }
    }

    private void AssignRandomInComeMoveInCount()
    {
        _maxIncomePerInterval = Random.Range(0, commercialSO.production + 1 - currentProduction);
    }
}