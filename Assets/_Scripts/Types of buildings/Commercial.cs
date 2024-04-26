using UnityEngine;

public class Commercial : Building
{
    [field: SerializeField] public CommercialSO commercialSO { get; private set; }
    private float _timer = 0;
    private int _workerCount = 0;

    private void Awake()
    {
        initBuildingSO(commercialSO);
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
            int employAmount = getMaximumJobCount() - _workerCount;
            _workerCount += GameManager.Instance.Employ(employAmount);
        }

        _timer += Time.deltaTime;
        timerUntilDestruction += shouldBeDestroyed ? Time.deltaTime : 0;


        if (_timer > commercialSO.productionRate)
        {   
            _timer = 0;

            if (GameManager.Instance.materials >= commercialSO.materialsNeededAmount && IsEnoughWorkers())
            {
                HideUI();
                shouldBeDestroyed = false;
                timerUntilDestruction = 0;
                GameManager.Instance.products += commercialSO.productionAmount;
                GameManager.Instance.materials -= commercialSO.materialsNeededAmount;
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
        GameManager.Instance.jobCount -= getMaximumJobCount();
        GameManager.Instance.Unemploy(_workerCount);
    }

    private bool IsEnoughWorkers(bool maxWorkers = false)
    {
        int neededWorkerCount = maxWorkers
            ? (int)Mathf.Ceil(commercialSO.workerNeededAmount + commercialSO.workerNeededAmount * commercialSO.workerAmountThreshold)
            : (int)Mathf.Ceil(commercialSO.workerNeededAmount - commercialSO.workerNeededAmount * commercialSO.workerAmountThreshold);

        return _workerCount >= neededWorkerCount;
    }

    private int getMaximumJobCount()
    {
        return (int)Mathf.Ceil(commercialSO.workerNeededAmount + commercialSO.workerNeededAmount * commercialSO.workerAmountThreshold);
    }

    protected override void OnBuild()
    {
        GameManager.Instance.jobCount += getMaximumJobCount();
    }
}