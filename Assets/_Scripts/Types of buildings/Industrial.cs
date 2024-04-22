using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Industrial : Building
{
    [field: SerializeField] public IndustrialSO industrialSO { get; private set; }
    private float _timer = 0;
    private float _workerCount = 0;

    private void Awake()
    {
        initBuildingSO(industrialSO);
    }

    protected override void Update()
    {
        if (!isBuilt)
        {
            return;
        }

        base.Update();

        _timer += Time.deltaTime;

        if (_timer > industrialSO.productionRate)
        {
            _timer = 0;
            GameManager.Instance.materials += industrialSO.productionAmount;
        }
    }

}
