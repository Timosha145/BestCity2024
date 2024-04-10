using UnityEngine;

public class Building : MonoBehaviour
{
    [field: SerializeField] public BuildingSO buildingSO;
    [SerializeField] private GameObject _selectVisual;

    public bool isBuilt = false;

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
    }

    public void Select()
    {
        _selectVisual.SetActive(true);
    }

    public void Unselect()
    {
        _selectVisual.SetActive(false);
    }
}
