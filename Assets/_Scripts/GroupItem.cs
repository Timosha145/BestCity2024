using UnityEngine;
using UnityEngine.UI;

public class GroupItem : MonoBehaviour
{
    [SerializeField] public Button button;

    [SerializeField] private AudioClip _sound;
    private GameObject _buildingPrefab;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            _audioSource.volume = GameManager.Instance.sfxVolume;
            _audioSource.clip = _sound;
            _audioSource.Play();

            PlacementSystem.Instance.CancelBuilding();
            PlacementSystem.Instance.ChangePrefab(_buildingPrefab);
            PlacementSystem.Instance.InitObject();
        });
    }

    public void Init(Sprite sprite, GameObject buildingPrefab)
    {
        button.image.sprite = sprite ?? button.image.sprite;
        _buildingPrefab = buildingPrefab;
    }
}
