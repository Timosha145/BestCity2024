using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class LabelManager : MonoBehaviour
{
    [Header("Global Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float _lerp = 0.1f;
    [SerializeField] private AudioClip _pressAudio;
    [SerializeField] private AudioClip _gameOverAudio;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private TextMeshProUGUI _labelGameTimer;
    [SerializeField] private TextMeshProUGUI _labelMoney;
    [SerializeField] private TextMeshProUGUI _labelPopulation;

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
    [SerializeField] private Button _settingsBtn;

    [Header("Toolbar")]
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private Button _resumeBtn;
    [SerializeField] private Button _optionsBtn;
    [SerializeField] private Button _exitBtn;
    [SerializeField] private Button _backFromSettingsBtn;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _musicSlider;

    [Header("Game Over")]
    [SerializeField] private GameObject _gameOverWindow;
    [SerializeField] private Button _backToMenuBtn;
    [SerializeField] private TextMeshProUGUI _gameOverLabel;
    [SerializeField] private GameObject[] _gameWindows;

    private HorizontalLayoutGroup _residenceLayoutGroup;
    private HorizontalLayoutGroup _commercialLayoutGroup;
    private HorizontalLayoutGroup _industryLayoutGroup;
    private HorizontalLayoutGroup _otherLayoutGroup;
    private AudioSource _audioSource;

    private Color _baseColor;
    private float _colorIncreaseRate = 0.05f;
    private float _currentRedFactor = 0f;

    private void Awake()
    {
        _residenceLayoutGroup = _residenceRect.GetComponent<HorizontalLayoutGroup>();
        _commercialLayoutGroup = _commercialRect.GetComponent<HorizontalLayoutGroup>();
        _industryLayoutGroup = _industryRect.GetComponent<HorizontalLayoutGroup>();
        //_otherLayoutGroup = _otherRect.GetComponent<HorizontalLayoutGroup>();
        _baseColor = _labelGameTimer.color;
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _pressAudio;

        InitLayoutGroups();
        DisableAllScrollViews();

        _pauseMenu.SetActive(false);
        _settingsMenu.SetActive(false);
        _gameOverWindow.SetActive(false);
        _residenceNeed.fillAmount = 0f;
        _commercialNeed.fillAmount = 0f;
        _industrialNeed.fillAmount = 0f;
        _jobNeed.fillAmount = 0f;
        _pieChartEmployed.fillAmount = 1f;

        GameManager.Instance.onGameOver += GameManagerOnGameOver;

        _sfxSlider.onValueChanged.AddListener((float value) =>
        {
            GameManager.Instance.sfxVolume = value;
        });

        _musicSlider.onValueChanged.AddListener((float value) =>
        {
            _musicSource.volume = value;
            GameManager.Instance.musicVolume = value;
        });

        _backFromSettingsBtn.onClick.AddListener(() =>
        {
            _audioSource.clip = _pressAudio;
            _audioSource.volume = GameManager.Instance.sfxVolume;
            _audioSource.Play();
            _settingsMenu.SetActive(false);
            _pauseMenu.SetActive(true);
        });

        _resumeBtn.onClick.AddListener(() =>
        {
            _audioSource.clip = _pressAudio;
            _audioSource.volume = GameManager.Instance.sfxVolume;
            _audioSource.Play();
            _pauseMenu.SetActive(false);
        });

        _optionsBtn.onClick.AddListener(() =>
        {
            _audioSource.clip = _pressAudio;
            _audioSource.volume = GameManager.Instance.sfxVolume;
            _audioSource.Play();
            _settingsMenu.SetActive(true);
            _pauseMenu.SetActive(false);
        });

        _exitBtn.onClick.AddListener(() =>
        {
            _audioSource.clip = _pressAudio;
            _audioSource.volume = GameManager.Instance.sfxVolume;
            _audioSource.Play();
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        _backToMenuBtn.onClick.AddListener(() =>
        {
            _audioSource.clip = _pressAudio;
            _audioSource.volume = GameManager.Instance.sfxVolume;
            _audioSource.Play();
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        _residenceSelectBtn.onClick.AddListener(() =>
        {
            _audioSource.clip = _pressAudio;
            _audioSource.volume = GameManager.Instance.sfxVolume;
            _audioSource.Play();
            GameManager.Instance.ChangeMode(GameManager.Mode.building);
            DisableAllScrollViews();

            _residenceScrollView.SetActive(true);
            PlacementSystem.Instance.CancelBuilding();
        });

        _commercialSelectBtn.onClick.AddListener(() =>
        {
            _audioSource.clip = _pressAudio;
            _audioSource.volume = GameManager.Instance.sfxVolume;
            _audioSource.Play();
            GameManager.Instance.ChangeMode(GameManager.Mode.building);
            DisableAllScrollViews();

            _commercialScrollView.SetActive(true);
            PlacementSystem.Instance.CancelBuilding();
        });

        _industrySelectBtn.onClick.AddListener(() =>
        {
            _audioSource.clip = _pressAudio;
            _audioSource.volume = GameManager.Instance.sfxVolume;
            _audioSource.Play();
            GameManager.Instance.ChangeMode(GameManager.Mode.building);
            DisableAllScrollViews();

            _industryScrollView.SetActive(true);
            PlacementSystem.Instance.CancelBuilding();
        });

        _roadSelectBtn.onClick.AddListener(() =>
        {
            _audioSource.clip = _pressAudio;
            _audioSource.volume = GameManager.Instance.sfxVolume;
            _audioSource.Play();
            GameManager.Instance.ChangeMode(GameManager.Mode.building);
            DisableAllScrollViews();

            PlacementSystem.Instance.CancelBuilding();
            PlacementSystem.Instance.ChangePrefab(GameManager.Instance.roadPreview.gameObject);
            PlacementSystem.Instance.InitObject();
        });

        _settingsBtn.onClick.AddListener(() =>
        {
            _audioSource.clip = _pressAudio;
            _audioSource.volume = GameManager.Instance.sfxVolume;
            _audioSource.Play();
            _pauseMenu.SetActive(true);
        });
    }

    private void GameManagerOnGameOver(object sender, GameManager.GameOverEventArgs e)
    {
        Debug.Log("Call");

        _audioSource.clip = _gameOverAudio;
        _audioSource.volume = GameManager.Instance.sfxVolume;
        _audioSource.Play();

        foreach (GameObject gameObject in _gameWindows)
        {
            gameObject.SetActive(false);
            PlacementSystem.Instance.CancelBuilding();
            GameManager.Instance.ChangeMode(GameManager.Mode.idle);
        }

        _gameOverWindow.SetActive(true);
        _gameOverLabel.text = e.population.ToString();
    }

    private void Update()
    {
        _labelMoney.text = GameManager.Instance.money > 999999999f
            ? "999,999,999+"
            : GameManager.Instance.money.ToString("N0");

        _labelPopulation.text = GameManager.Instance.population.ToString();
        UpdateNeedCharts();
        UpdateEmployedPieChart();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlacementSystem.Instance.CancelBuilding();
            DisableAllScrollViews();
            GameManager.Instance.ChangeMode(GameManager.Mode.idle);
        }

        _currentRedFactor += _colorIncreaseRate * Time.deltaTime;
        _currentRedFactor = Mathf.Clamp01(_currentRedFactor);
        _labelGameTimer.color = new Color(_baseColor.r + _currentRedFactor, _baseColor.g, _baseColor.b, _baseColor.a);

        _labelGameTimer.text = FormatTime(GameManager.Instance.gameTimer);
    }

    private void OnDestroy()
    {
        _sfxSlider.onValueChanged.RemoveAllListeners();
        _musicSlider.onValueChanged.RemoveAllListeners();
        _backFromSettingsBtn.onClick.RemoveAllListeners();
        _resumeBtn.onClick.RemoveAllListeners();
        _optionsBtn.onClick.RemoveAllListeners();
        _exitBtn.onClick.RemoveAllListeners();
        _residenceSelectBtn.onClick.RemoveAllListeners();
        _commercialSelectBtn.onClick.RemoveAllListeners();
        _industrySelectBtn.onClick.RemoveAllListeners();
        _roadSelectBtn.onClick.RemoveAllListeners();
        _settingsBtn.onClick.RemoveAllListeners();

        GameManager.Instance.onGameOver -= GameManagerOnGameOver;
    }

    private string FormatTime(float timeInSeconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeInSeconds);
        string formattedTime = string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);

        return formattedTime;
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
            float commercialForIndustry = (float)GameManager.Instance.neededMaterialsPerDay > 0
                ? 1f - ((float)GameManager.Instance.neededMaterialsPerDay / (float)GameManager.Instance.materials)
                : 1f;

            float commercialForResidence = (float)GameManager.Instance.products > 0
                ? 1f - ((float)GameManager.Instance.products / (float)GameManager.Instance.neededProductsPerDay)
                : 1f;

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
