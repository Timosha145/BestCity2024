using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject cellIndicator; // в c# private поля лучше пиши начиная с нижнего подчеркивания _foo
    [SerializeField] private GameObject buildingPrefab; // вместо GameObject лучше transform, так как нас интересует именно свойства позиции, не более (gameobject всегда можно получить из transform)
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private Vector3 _offset;
    [Space]
    [SerializeField] private List<Road> _roads;

    private bool isBuilding = false;
    private GameObject currentBuilding;
    private int _currentRoadSelection = 0;

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _currentRoadSelection = (_currentRoadSelection + 1) % (_roads.Count + 1);
            buildingPrefab = _roads[_currentRoadSelection].gameObject;

            Destroy(currentBuilding);
            StartBuilding();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            currentBuilding.transform.Rotate(0f, 90f, 0f);
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
        currentBuilding = Instantiate(buildingPrefab, grid.CellToWorld(gridPosition) + _offset, Quaternion.identity);
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
        currentBuilding.transform.position = grid.CellToWorld(gridPosition) + _offset;
    }
}