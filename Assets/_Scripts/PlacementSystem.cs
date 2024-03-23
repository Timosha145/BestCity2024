using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private GameObject buildingPrefab; // префаб здания для строительства
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;

    private bool isBuilding = false; // флаг, указывающий, идет ли в данный момент строительство
    private GameObject currentBuilding; // ссылка на текущий экземпляр здания

    void Update()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (!isBuilding)
            {
                StartBuilding();
            }
            else
            {
                FinishBuilding();
            }
        }

        if (isBuilding && currentBuilding != null)
        {
            UpdateBuildingPosition();
        }
    }

    void StartBuilding()
    {
        isBuilding = true;
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        currentBuilding = Instantiate(buildingPrefab, grid.CellToWorld(gridPosition), Quaternion.identity);
    }

    void FinishBuilding()
    {
        isBuilding = false;
    }

    void UpdateBuildingPosition()
    {
        // Обновление позиции здания в соответствии с положением указателя мыши
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        currentBuilding.transform.position = grid.CellToWorld(gridPosition);
    }
}