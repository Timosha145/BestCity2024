using System.Threading.Tasks;
using UnityEngine;

public class Residence : Building
{
    [field: Header("Residence Settings")]
    [field: SerializeField] public int population { get; private set; }
    [field: SerializeField] public float residentsCountUpdateRate { get; private set; } = 15f;
    [field: SerializeField] public float residentsCheckNeedsRate { get; private set; } = 5f;

    public int currentPopulation { get; private set; }

    private float _timer = 0;
    private float _timerNeeds = 0;
    private int _peopleMoveInCount;

    private void Awake()
    {
        AssignRandomPeopleMoveInCount();
    }

    protected async override Task Update()
    {
        if (!isBuilt || toDestroy)
        {
            return;
        }

        await base.Update();

        _timer += Time.deltaTime;
        _timerNeeds += Time.deltaTime;
        timerUntilDestruction += shouldBeDestroyed ? Time.deltaTime : 0;

        if (_timer > residentsCountUpdateRate && currentPopulation < population)
        {   
            _timer = 0;

            AssignRandomPeopleMoveInCount();
            currentPopulation = Mathf.Clamp(currentPopulation + _peopleMoveInCount, 0, population);
            GameManager.Instance.population += _peopleMoveInCount;
        }

        if (_timerNeeds > residentsCountUpdateRate)
        {
            _timerNeeds = 0;

            if (IsEnoughWork() && IsEnoughCommercial())
            {
                HideUI();
                shouldBeDestroyed = false;
                timerUntilDestruction = 0;
                GameManager.Instance.products -= GetNeededProductCount();
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
            GameManager.Instance.population -= currentPopulation;
            GameManager.Instance.neededProductsPerDay -= GameManager.Instance.dayDuration
                / residentsCheckNeedsRate * GetMaxNeededProductCount();
            GameManager.Instance.money -= cost * 2;
        }
    }

    private void AssignRandomPeopleMoveInCount()
    {
        _peopleMoveInCount = Random.Range(0, population + 1 - currentPopulation);
    }

    private bool IsEnoughWork()
    {
        return true;
    }

    private bool IsEnoughCommercial()
    {
        return GameManager.Instance.products - GetNeededProductCount() > 0;
    }

    private float GetNeededProductCount()
    {
        return currentPopulation * GameManager.Instance.productPerCitizen;
    }

    private float GetMaxNeededProductCount()
    {
        return population * GameManager.Instance.productPerCitizen;
    }

    protected override void OnBuild()
    {
        GameManager.Instance.neededProductsPerDay += GameManager.Instance.dayDuration 
            / residentsCheckNeedsRate * GetMaxNeededProductCount();
    }
}
