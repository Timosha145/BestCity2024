using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject cellIndicator; // в c# private поля лучше пиши начиная с нижнего подчеркивания _foo
    [SerializeField] private GameObject buildingPrefab; // вместо GameObject лучше transform, так как нас интересует именно свойства позиции, не более (gameobject всегда можно получить из transform)
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;

    private bool isBuilding = false;
    private GameObject currentBuilding;

    void Update()
    {
        // Старайся делать Update как можно абстракнее (вынеси в отдельный метод код снизу)
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