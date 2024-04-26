using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private GameObject _selectVisual;
    [SerializeField] private GameObject _UIVisual;
    [SerializeField] private TextMeshProUGUI _timerLbl;

    public BuildingSO buildingSO { get; private set; }
    protected float timerUntilDestruction = 0;
    protected bool shouldBeDestroyed = false;
    private Renderer _renderer;

    public bool isBuilt = false;

    private void Start()
    {
        _renderer = _selectVisual?.GetComponent<Renderer>();
    }

    protected virtual void Update()
    {
        _timerLbl.text = $"{Mathf.Ceil(buildingSO.waitUntilDestroying - timerUntilDestruction)}s";

        if (timerUntilDestruction >= buildingSO.waitUntilDestroying && shouldBeDestroyed)
        {
            Destroy(gameObject);
        }
    }

    protected void initBuildingSO(BuildingSO buildingSO)
    {
        this.buildingSO = buildingSO;
    }

    public bool CanBuild()
    {
        return buildingSO.cost <= GameManager.Instance.money;
    }

    public void Pay()
    {
        GameManager.Instance.money -= buildingSO.cost;
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
