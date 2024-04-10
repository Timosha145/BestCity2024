using TMPro;
using UnityEngine;

public class LabelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _labelMoney;
    [SerializeField] private TextMeshProUGUI _labelPopulation;

    private void Update()
    {
        _labelMoney.text = GameManager.Instance.money.ToString() + "$";
        _labelPopulation.text = GameManager.Instance.population.ToString();
    }
}
