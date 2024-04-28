using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LabelManager : MonoBehaviour
{
    [Header("Global Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float _lerp = 0.1f;

    [SerializeField] private TextMeshProUGUI _labelMoney;
    [SerializeField] private TextMeshProUGUI _labelPopulation;
    [SerializeField] private TextMeshProUGUI _materialsLabel;
    [SerializeField] private TextMeshProUGUI _productsLabel;

    [SerializeField] private Button _industrialBtn;
    [SerializeField] private Button _commercialBtn;
    [SerializeField] private Button _residentBtn;
    [SerializeField] private Button _roadBtn;

    [SerializeField] private GameObject _industrial;
    [SerializeField] private GameObject _commercial;
    [SerializeField] private GameObject _residence;
    [SerializeField] private GameObject _road;

    [Header("Employed Pie Chart")]
    [SerializeField] private Image _pieChartEmployed;

    [Header("Needs Charts")]
    [SerializeField] private Image _residenceNeed;
    [SerializeField] private Image _commercialNeed;
    [SerializeField] private Image _industrialNeed;
    [SerializeField] private Image _jobNeed;

    [Header("Toolbar")]
    [SerializeField] private float _groupItemWidth;
    [SerializeField] private GroupItem _groupItem;
    [SerializeField] private RectTransform _residenceRect;
    [SerializeField] private RectTransform _commercialRect;
    [SerializeField] private RectTransform _industryRect;
    [SerializeField] private RectTransform _otherRect;

    private HorizontalLayoutGroup _residenceLayoutGroup;
    private HorizontalLayoutGroup _commercialLayoutGroup;
    private HorizontalLayoutGroup _industryLayoutGroup;
    private HorizontalLayoutGroup _otherLayoutGroup;

    private void Awake()
    {
        _residenceLayoutGroup = _residenceRect.GetComponent<HorizontalLayoutGroup>();
        //_commercialLayoutGroup = _commercialRect.GetComponent<HorizontalLayoutGroup>();
        //_industryLayoutGroup = _industryRect.GetComponent<HorizontalLayoutGroup>();
        //_otherLayoutGroup = _otherRect.GetComponent<HorizontalLayoutGroup>();
    }

    private void Start()
    {
        _industrialBtn.onClick.AddListener(ChangeIndustrial);
        _commercialBtn.onClick.AddListener(ChangeCommercial);
        _residentBtn.onClick.AddListener(ChangeResidence);
        _roadBtn.onClick.AddListener(ChangeRoad);

        InitLayoutGroups();
    }

    private void InitLayoutGroups()
    {
        foreach (Residence residence in GameManager.Instance.residencePrefabs)
        {
            GroupItem groupItem = Instantiate(_groupItem, _residenceLayoutGroup.transform);
            groupItem.Init(residence.sprite, residence.gameObject);
            _residenceRect.sizeDelta = new Vector2(_residenceRect.sizeDelta.x + _groupItemWidth, _residenceRect.sizeDelta.y);
        }
    }

    private void ChangeRoad()
    {
        PlacementSystem.Instance.ChangePrefab(_road);
    }

    private void ChangeIndustrial()
    {
        PlacementSystem.Instance.ChangePrefab(_industrial);
    }

    private void ChangeCommercial()
    {
        PlacementSystem.Instance.ChangePrefab(_commercial);
    }

    private void ChangeResidence()
    {
        PlacementSystem.Instance.ChangePrefab(_residence);
    }

    private void Update()
    {
        _labelMoney.text = GameManager.Instance.money.ToString("N0");
        _labelPopulation.text = GameManager.Instance.population.ToString();
        _materialsLabel.text = GameManager.Instance.materials.ToString();
        _productsLabel.text = GameManager.Instance.products.ToString();
        UpdateNeedCharts();
        UpdateEmployedPieChart();
    }

    private void UpdateNeedCharts()
    {
        if (GameManager.Instance.population > 0)
        {
            float jobs = GameManager.Instance.jobCount > 0
                ? 1f - ((float)GameManager.Instance.jobCount / (float)GameManager.Instance.population)
                : 1f;

            float fill = jobs > 1f ? 0f : jobs;

            _jobNeed.fillAmount = Mathf.MoveTowards(_jobNeed.fillAmount, fill, _lerp * Time.deltaTime);
        }

        if (GameManager.Instance.jobCount > 0)
        {
            float workers = GameManager.Instance.population > 0
                ? 1f - ((float)GameManager.Instance.population / (float)GameManager.Instance.jobCount)
                : 1f;

            float fill = workers > 1f ? 0f : workers;

            _residenceNeed.fillAmount = Mathf.MoveTowards(_residenceNeed.fillAmount, fill, _lerp * Time.deltaTime);
        }
    }

    private void UpdateEmployedPieChart()
    {
        if (GameManager.Instance.population > 0)
        {
            float employed = (float)GameManager.Instance.employed / (float)GameManager.Instance.population;
            float fill = GameManager.Instance.employed > 0
                ? Mathf.Clamp(employed, 0f, 1f)
                : 0;

            _pieChartEmployed.fillAmount = Mathf.MoveTowards(_pieChartEmployed.fillAmount, fill, _lerp * Time.deltaTime);
        } 
    }
}
