using UnityEngine;

public class Commercial : Building
{
    [field: Header("Commercial Settings")]
    [field: SerializeField] public float salary { get; private set; }
    [field: SerializeField] public int workerNeededAmount { get; private set; }
    [field: Range(0, 1)]
    [field: SerializeField] public float workerAmountThreshold { get; private set; }
    [field: SerializeField] public int materialsNeededAmount { get; private set; }
    [field: Space]
    [field: SerializeField] public float productionRate { get; private set; }
    [field: SerializeField] public int productionAmount { get; private set; }
    private float _timer = 0;
    private int _workerCount = 0;

    protected override void Start()
    {
        base.Start();

        GameManager.Instance.onDayPassed += GameManagerOnDayPassed;
    }

    private void GameManagerOnDayPassed(object sender, System.EventArgs e)
    {
        if (isBuilt)
        {
            GameManager.Instance.PayTaxes(salary * _workerCount);
        }
    }

    protected override void Update()
    {
        if (!isBuilt)
        {
            return;
        }

        base.Update();

        if (!IsEnoughWorkers(true))
        {
            int employAmount = GetMaximumJobCount() - _workerCount;
            _workerCount += GameManager.Instance.Employ(employAmount);
        }

        _timer += Time.deltaTime;
        timerUntilDestruction += shouldBeDestroyed ? Time.deltaTime : 0;

        if (_timer > productionRate)
        {   
            _timer = 0;

            if (GameManager.Instance.materials >= materialsNeededAmount && IsEnoughWorkers())
            {
                HideUI();
                shouldBeDestroyed = false;
                timerUntilDestruction = 0;
                GameManager.Instance.products += productionAmount;
                GameManager.Instance.materials -= materialsNeededAmount;
            }
            else
            {
                ShowUI();
                shouldBeDestroyed = true;
            }
        }
    }

    private void OnDestroy()
    {
        if (isBuilt)
        {
            GameManager.Instance.jobCount -= GetMaximumJobCount();
            GameManager.Instance.neededMaterialsPerDay -= GetNeededMaterialsPerDay();
            GameManager.Instance.producingProductsPerDay -= GetProducingProductsPerDay();
            GameManager.Instance.Unemploy(_workerCount);
        }
    }

    private bool IsEnoughWorkers(bool maxWorkers = false)
    {
        int neededWorkerCount = maxWorkers
            ? (int)Mathf.Ceil(workerNeededAmount + workerNeededAmount * workerAmountThreshold)
            : (int)Mathf.Ceil(workerNeededAmount - workerNeededAmount * workerAmountThreshold);

        return _workerCount >= neededWorkerCount;
    }

    private int GetMaximumJobCount()
    {
        return (int)Mathf.Ceil(workerNeededAmount + workerNeededAmount * workerAmountThreshold);
    }

    protected override void OnBuild()
    {
        GameManager.Instance.jobCount += GetMaximumJobCount();
        GameManager.Instance.neededMaterialsPerDay += GetNeededMaterialsPerDay();
        GameManager.Instance.producingProductsPerDay += GetProducingProductsPerDay();
    }

    private float GetNeededMaterialsPerDay()
    {
        return GameManager.Instance.dayDuration / productionRate * materialsNeededAmount;
    }

    private float GetProducingProductsPerDay()
    {
        return GameManager.Instance.dayDuration / productionRate * productionAmount;
    }
}