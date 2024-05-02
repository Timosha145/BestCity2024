using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    [field: Header("Main Settings")]
    [field: SerializeField] public int cost { get; private set; }
    [field: SerializeField] public float waitUntilDestroying { get; private set; } = 60f;
    [field: SerializeField] public int widthX;
    [field: SerializeField] public int lengthZ;
    [field: Header("Prefab Settings")]
    [field: SerializeField] public Sprite sprite { get; private set; }
    [SerializeField] private GameObject _selectVisual;
    [SerializeField] private GameObject _UIVisual;
    [SerializeField] private TextMeshProUGUI _timerLbl;
    [SerializeField] private AudioClip _onBuildAudio;
    [SerializeField] private AudioClip _onDestroyAudio;

    private AudioSource _audioSource;
    protected float timerUntilDestruction = 0;
    protected bool shouldBeDestroyed = false;
    private Renderer _renderer;

    public bool isBuilt = false;
    protected bool toDestroy = false;

    protected virtual void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _renderer = _selectVisual?.GetComponent<Renderer>();
    }

    protected virtual async Task Update()
    {
        if (toDestroy)
        {
            return;
        }

        _timerLbl.text = $"{Mathf.Ceil(waitUntilDestroying - timerUntilDestruction)}s";

        if (timerUntilDestruction >= waitUntilDestroying && shouldBeDestroyed)
        {
            toDestroy = true;
            PlayDestroySound();
            GetComponent<MeshRenderer>().enabled = false;
            _UIVisual.SetActive(false);
            await Task.Delay(TimeSpan.FromSeconds(_onDestroyAudio.length));
            Destroy(gameObject);
        }
    }

    public bool CanBuild()
    {
        return cost <= GameManager.Instance.money;
    }

    public void Pay()
    {
        GameManager.Instance.money -= cost;
        _audioSource.clip = _onBuildAudio;
        _audioSource.volume = GameManager.Instance.sfxVolume;
        _audioSource.Play();
        OnBuild();
    }

    public void Select()
    {
        _selectVisual.SetActive(true);
    }

    public void Unselect()
    {
        _selectVisual.SetActive(false);
    }

    public void ShowUI()
    {
        _UIVisual.SetActive(true);
    }

    public void HideUI()
    {
        _UIVisual.SetActive(false);
    }

    protected void PlayDestroySound()
    {
        _audioSource.clip = _onDestroyAudio;
        _audioSource.volume = GameManager.Instance.sfxVolume;
        _audioSource.Play();
    }

    public void ChangeSelectMaterial(Material material)
    {
        if (_renderer && _renderer.material != material)
        {
            _renderer.material = material;
        }
    }

    protected virtual void OnBuild() { }
}
