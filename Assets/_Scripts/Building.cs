using System.Collections.Generic;
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

    protected float timerUntilDestruction = 0;
    protected bool shouldBeDestroyed = false;
    private Renderer _renderer;

    public bool isBuilt = false;

    protected virtual void Start()
    {
        _renderer = _selectVisual?.GetComponent<Renderer>();
    }

    protected virtual void Update()
    {
        _timerLbl.text = $"{Mathf.Ceil(waitUntilDestroying - timerUntilDestruction)}s";

        if (timerUntilDestruction >= waitUntilDestroying && shouldBeDestroyed)
        {
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

    public void ChangeSelectMaterial(Material material)
    {
        if (_renderer && _renderer.material != material)
        {
            _renderer.material = material;
        }
    }

    protected virtual void OnBuild() { }
}
