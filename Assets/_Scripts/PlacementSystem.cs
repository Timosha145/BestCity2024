using UnityEngine;
using UnityEngine.UI;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private GameObject buildingPrefab;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    public Button houseButton;

    private bool isBuilding = false;
    private GameObject currentBuilding;

    void Start()
    {
        houseButton.onClick.AddListener(StartHouseBuilding);
    }

    void Update()
    {   
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);

        if (isBuilding && currentBuilding != null)
        {
            UpdateBuildingPosition();

            if (Input.GetMouseButtonDown(0))
            {
                PlaceBuilding();
            }

            // Проверяем нажатие кнопки R для вращения здания
            if (Input.GetKeyDown(KeyCode.R))
            {
                RotateBuilding();
            }
        }
    }

    void StartHouseBuilding()
    {
        if (!isBuilding)
        {
            isBuilding = true;
            Vector3 mousePosition = inputManager.GetSelectedMapPosition();
            Vector3Int gridPosition = grid.WorldToCell(mousePosition);

            Vector3 worldPosition = grid.CellToWorld(gridPosition) + new Vector3(grid.cellSize.x / 2, grid.cellSize.y / 2, 0f);
            currentBuilding = Instantiate(buildingPrefab, worldPosition, Quaternion.identity);
        }
    }

    void FinishBuilding()
    {
        isBuilding = false;
    }

    void UpdateBuildingPosition()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        currentBuilding.transform.position = grid.CellToWorld(gridPosition);
    }

    void PlaceBuilding()
    {   
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        FinishBuilding();
    }

    // Метод для вращения здания
    void RotateBuilding()
    {
        currentBuilding.transform.Rotate(Vector3.up, 90f); // Поворачиваем на 90 градусов вокруг оси Y
    }
}
