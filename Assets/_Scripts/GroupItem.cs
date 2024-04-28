using UnityEngine;
using UnityEngine.UI;

public class GroupItem : MonoBehaviour
{
    [SerializeField] public Button button;

    private GameObject _buildingPrefab;

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            PlacementSystem.Instance.ChangePrefab(_buildingPrefab);
        });
    }

    public void Init(Sprite sprite, GameObject buildingPrefab)
    {
        button.image.sprite = sprite;
        _buildingPrefab = buildingPrefab;
    }
}
