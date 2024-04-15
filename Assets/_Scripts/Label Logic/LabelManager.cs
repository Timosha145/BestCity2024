using TMPro;
using UnityEngine;

public class LabelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _labelMoney;
    [SerializeField] private TextMeshProUGUI _labelPopulation;
    [SerializeField] private TextMeshProUGUI _materialLabel;

    private void Update()
    {
        _labelMoney.text = GameManager.Instance.money.ToString() + "$";
        _labelPopulation.text = GameManager.Instance.population.ToString();
        _materialLabel.text = GameManager.Instance.material.ToString();
    }
}
