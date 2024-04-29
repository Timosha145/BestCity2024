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
    [SerializeField] private GameObject _residenceScrollView;
    [SerializeField] private GameObject _commercialScrollView;
    [SerializeField] private GameObject _industryScrollView;
    [SerializeField] private GameObject _otherScrollView;
    [SerializeField] private RectTransform _residenceRect;
    [SerializeField] private RectTransform _commercialRect;
    [SerializeField] private RectTransform _industryRect;
    [SerializeField] private RectTransform _otherRect;
    [Space]
    [SerializeField] private Button _residenceSelectBtn;
    [SerializeField] private Button _commercialSelectBtn;
    [SerializeField] private Button _industrySelectBtn;
    [SerializeField] private Button _otherSelectBtn;
    [SerializeField] private Button _roadSelectBtn;

    private HorizontalLayoutGroup _residenceLayoutGroup;
    private HorizontalLayoutGroup _commercialLayoutGroup;
    private HorizontalLayoutGroup _industryLayoutGroup;
    private HorizontalLayoutGroup _otherLayoutGroup;

    private void Awake()
    {
        _residenceLayoutGroup = _residenceRect.GetComponent<HorizontalLayoutGroup>();
        _commercialLayoutGroup = _commercialRect.GetComponent<HorizontalLayoutGroup>();
        _industryLayoutGroup = _industryRect.GetComponent<HorizontalLayoutGroup>();
        //_otherLayoutGroup = _otherRect.GetComponent<HorizontalLayoutGroup>();
    }

    private void Start()
    {
        InitLayoutGroups();
        DisableAllScrollViews();

        _residenceNeed.fillAmount = 0f;
        _commercialNeed.fillAmount = 0f;
        _industrialNeed.fillAmount = 0f;
        _jobNeed.fillAmount = 0f;
        _pieChartEmployed.fillAmount = 1f;

        _residenceSelectBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.ChangeMode(GameManager.Mode.building);
            DisableAllScrollViews();

            _residenceScrollView.SetActive(true);
            PlacementSystem.Instance.CancelBuilding();
        });

        _commercialSelectBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.ChangeMode(GameManager.Mode.building);
            DisableAllScrollViews();

            _commercialScrollView.SetActive(true);
            PlacementSystem.Instance.CancelBuilding();
        });

        _industrySelectBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.ChangeMode(GameManager.Mode.building);
            DisableAllScrollViews();

            _industryScrollView.SetActive(true);
            PlacementSystem.Instance.CancelBuilding();
        });

        _roadSelectBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.ChangeMode(GameManager.Mode.building);
            DisableAllScrollViews();

            PlacementSystem.Instance.CancelBuilding();
            PlacementSystem.Instance.ChangePrefab(GameManager.Instance.roadPreview.gameObject);
            PlacementSystem.Instance.InitObject();
        });
    }

    private void Update()
    {
        _labelMoney.text = GameManager.Instance.money > 999999999f
            ? "999,999,999+"
            : GameManager.Instance.money.ToString("N0");

        _labelPopulation.text = GameManager.Instance.population.ToString();
        _materialsLabel.text = GameManager.Instance.neededProductsPerDay.ToString();
        _productsLabel.text = GameManager.Instance.products.ToString();
        UpdateNeedCharts();
        UpdateEmployedPieChart();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlacementSystem.Instance.CancelBuilding();
            DisableAllScrollViews();
            GameManager.Instance.ChangeMode(GameManager.Mode.idle);
        }
    }

    private void DisableAllScrollViews()
    {
        _residenceScrollView.SetActive(false);
        _commercialScrollView.SetActive(false);
        _industryScrollView.SetActive(false);
    }

    private void InitLayoutGroups()
    {
        foreach (Residence residence in GameManager.Instance.residencePrefabs)
        {
            GroupItem groupItem = Instantiate(_groupItem, _residenceLayoutGroup.transform);
            groupItem.Init(residence.sprite, residence.gameObject);
            _residenceRect.sizeDelta = new Vector2(_residenceRect.sizeDelta.x + _groupItemWidth, _residenceRect.sizeDelta.y);
        }

        foreach (Commercial commercial in GameManager.Instance.commercialPrefabs)
        {
            GroupItem groupItem = Instantiate(_groupItem, _commercialLayoutGroup.transform);
            groupItem.Init(commercial.sprite, commercial.gameObject);
            _commercialRect.sizeDelta = new Vector2(_commercialRect.sizeDelta.x + _groupItemWidth, _commercialRect.sizeDelta.y);
        }

        foreach (Industrial industrial in GameManager.Instance.industryPrefabs)
        {
            GroupItem groupItem = Instantiate(_groupItem, _industryLayoutGroup.transform);
            groupItem.Init(industrial.sprite, industrial.gameObject);
            _industryRect.sizeDelta = new Vector2(_industryRect.sizeDelta.x + _groupItemWidth, _industryRect.sizeDelta.y);
        }
    }

    private void UpdateNeedCharts()
    {
        // Jobs
        if (GameManager.Instance.population > 0)
        {
            float jobs = GameManager.Instance.jobCount > 0
                ? 1f - ((float)GameManager.Instance.jobCount / (float)GameManager.Instance.population)
                : 1f;

            float fill = jobs > 1f ? 0f : jobs;

            _jobNeed.fillAmount = Mathf.MoveTowards(_jobNeed.fillAmount, fill, _lerp * Time.deltaTime);
        }
        else
        {
            _jobNeed.fillAmount = Mathf.MoveTowards(_jobNeed.fillAmount, 0, _lerp * Time.deltaTime);
        }

        // Residence
        if (GameManager.Instance.jobCount > 0)
        {
            float workers = GameManager.Instance.population > 0
                ? 1f - ((float)GameManager.Instance.population / (float)GameManager.Instance.jobCount)
                : 1f;

            float fill = workers > 1f ? 0f : workers;

            _residenceNeed.fillAmount = Mathf.MoveTowards(_residenceNeed.fillAmount, fill, _lerp * Time.deltaTime);
        }
        else
        {
            _residenceNeed.fillAmount = Mathf.MoveTowards(_residenceNeed.fillAmount, 0, _lerp * Time.deltaTime);
        }

        // Industry
        if (GameManager.Instance.neededMaterialsPerDay > 0)
        {
            float industry = GameManager.Instance.producingMaterialsPerDay > 0
                ? 1f - ((float)GameManager.Instance.producingMaterialsPerDay / (float)GameManager.Instance.neededMaterialsPerDay)
                : 1f;

            float fill = industry > 1f ? 0f : industry;

            _industrialNeed.fillAmount = Mathf.MoveTowards(_industrialNeed.fillAmount, fill, _lerp * Time.deltaTime);
        }
        else
        {
            _industrialNeed.fillAmount = Mathf.MoveTowards(_industrialNeed.fillAmount, 0, _lerp * Time.deltaTime);
        }

        // Commercial
        if ((float)GameManager.Instance.materials > 0 || GameManager.Instance.producingMaterialsPerDay > 0 || GameManager.Instance.neededProductsPerDay > 0)
        {
            float commercialForIndustry = (float)GameManager.Instance.GetMaxMaterials() > 0
                ? 1f - ((float)GameManager.Instance.GetMaxMaterials() / (float)GameManager.Instance.materials)
                : 1f;

            float commercialForResidence = (float)GameManager.Instance.products > 0
                ? 1f - ((float)GameManager.Instance.products / (float)GameManager.Instance.neededProductsPerDay)
                : 1f;

            Debug.Log($"I: {commercialForIndustry} R: {commercialForResidence}");
            float commercial = (commercialForIndustry + commercialForResidence) / 2;

            float fill = commercial > 1f ? 0f : commercial;

            _commercialNeed.fillAmount = Mathf.MoveTowards(_commercialNeed.fillAmount, fill, _lerp * Time.deltaTime);
        }
        else
        {
            _commercialNeed.fillAmount = Mathf.MoveTowards(_commercialNeed.fillAmount, 0, _lerp * Time.deltaTime);
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
        else
        {
            _pieChartEmployed.fillAmount = Mathf.MoveTowards(_pieChartEmployed.fillAmount, 1, _lerp * Time.deltaTime);
        }
    }
}
