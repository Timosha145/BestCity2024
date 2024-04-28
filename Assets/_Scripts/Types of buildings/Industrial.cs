using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Industrial : Building
{
    [field: Header("Industrial Settings")]
    [field: SerializeField] public int workerNeededAmount { get; private set; }
    [field: Range(0, 1)]
    [field: SerializeField] public float workerAmountThreshold { get; private set; }
    [field: SerializeField] public float productionRate { get; private set; }
    [field: SerializeField] public int productionAmount { get; private set; }
    private float _timer = 0;
    private float _workerCount = 0;

    protected override void Update()
    {
        if (!isBuilt)
        {
            return;
        }

        base.Update();

        _timer += Time.deltaTime;

        if (_timer > productionRate)
        {
            _timer = 0;
            GameManager.Instance.materials += productionAmount;
        }
    }

}
