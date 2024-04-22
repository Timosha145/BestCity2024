using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LabelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _labelMoney;
    [SerializeField] private TextMeshProUGUI _labelPopulation;
    [SerializeField] private TextMeshProUGUI _materialsLabel;
    [SerializeField] private TextMeshProUGUI _productsLabel;
    [SerializeField] private TextMeshProUGUI _employedLabel;

    [SerializeField] private Button _industrialBtn;
    [SerializeField] private Button _commercialBtn;
    [SerializeField] private Button _residentBtn;

    [SerializeField] private GameObject _industrial;
    [SerializeField] private GameObject _commercial;
    [SerializeField] private GameObject _residence;

    private void Start()
    {
        _industrialBtn.onClick.AddListener(ChangeIndustrial);
        _commercialBtn.onClick.AddListener(ChangeCommercial);
        _residentBtn.onClick.AddListener(ChangeResidence);
    }

    private void ChangeIndustrial()
    {
        PlacementSystem.Instance.ChangeBuilding(_industrial);
    }

    private void ChangeCommercial()
    {
        PlacementSystem.Instance.ChangeBuilding(_commercial);
    }

    private void ChangeResidence()
    {
        PlacementSystem.Instance.ChangeBuilding(_residence);
    }

    private void Update()
    {
        _labelMoney.text = GameManager.Instance.money.ToString() + "$";
        _labelPopulation.text = GameManager.Instance.population.ToString();
        _materialsLabel.text = GameManager.Instance.materials.ToString();
        _productsLabel.text = GameManager.Instance.products.ToString();
        _employedLabel.text = GameManager.Instance.employed.ToString();
    }
}
