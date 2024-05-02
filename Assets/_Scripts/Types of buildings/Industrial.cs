using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Industrial : Building
{
    [field: Header("Industrial Settings")]
    [field: SerializeField] public float salary { get; private set; }
    [field: SerializeField] public int workerNeededAmount { get; private set; }
    [field: Range(0, 1)]
    [field: SerializeField] public float workerAmountThreshold { get; private set; }
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

    protected async override Task Update()
    {
        if (!isBuilt || toDestroy)
        {
            return;
        }

        await base.Update();

        if (!IsEnoughWorkers(true))
        {
            int employAmount = getMaximumJobCount() - _workerCount;
            _workerCount += GameManager.Instance.Employ(employAmount);
        }

        _timer += Time.deltaTime;
        timerUntilDestruction += shouldBeDestroyed ? Time.deltaTime : 0;

        if (_timer > productionRate)
        {
            _timer = 0;

            if (IsEnoughWorkers() && !GameManager.Instance.IsMaterialOverstock())
            {
                HideUI();
                shouldBeDestroyed = false;
                timerUntilDestruction = 0;
                GameManager.Instance.materials += productionAmount;
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
            GameManager.Instance.jobCount -= getMaximumJobCount();
            GameManager.Instance.producingMaterialsPerDay -= GetProducingMaterialsPerDay();
            GameManager.Instance.Unemploy(_workerCount);
            GameManager.Instance.money -= cost * 2;
        }
    }

    private bool IsEnoughWorkers(bool maxWorkers = false)
    {
        int neededWorkerCount = maxWorkers
            ? (int)Mathf.Ceil(workerNeededAmount + workerNeededAmount * workerAmountThreshold)
            : (int)Mathf.Ceil(workerNeededAmount - workerNeededAmount * workerAmountThreshold);

        return _workerCount >= neededWorkerCount;
    }

    private int getMaximumJobCount()
    {
        return (int)Mathf.Ceil(workerNeededAmount + workerNeededAmount * workerAmountThreshold);
    }

    protected override void OnBuild()
    {
        GameManager.Instance.jobCount += getMaximumJobCount();
        GameManager.Instance.producingMaterialsPerDay += GetProducingMaterialsPerDay();
    }

    private float GetProducingMaterialsPerDay()
    {
        return GameManager.Instance.dayDuration / productionRate * productionAmount;
    }
}
