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
    [field: SerializeField] public List<Road> roadVariants { get; private set; }
    [SerializeField] private Transform _defaultRoad;

    private bool isBuilding = false;
    private GameObject currentBuilding;
    private Road _road;
    private Vector3 _selectedGridPosition = Vector3.zero;

    public static PlacementSystem Instance { get; private set; }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Update()
    {
        // Старайся делать Update как можно абстракнее (вынеси в отдельный метод код снизу)
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        _selectedGridPosition = grid.CellToWorld(grid.WorldToCell(mousePosition)) + _offset;
        cellIndicator.transform.position = _selectedGridPosition;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            isBuilding = !isBuilding;
            currentBuilding = isBuilding
                ? Instantiate(buildingPrefab, _selectedGridPosition, Quaternion.identity)
                : null;

            currentBuilding?.TryGetComponent(out _road);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(currentBuilding);
            isBuilding = false;
            currentBuilding = null;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateBuilding();
        }

        if (isBuilding && currentBuilding != null)
        {
            currentBuilding.transform.position = _selectedGridPosition;

            if (_road)
            {
                Road suitableRoad = _road.GetSuitableRoad(_selectedGridPosition);

                if (_road.roadName == suitableRoad.roadName)
                {
                    return;
                }

                List<Road> roadsToRotate = _road.GetCollidingRoads(_road.transform.position);
                roadsToRotate.Add(_road);

                ChangeBuilding(suitableRoad.gameObject);
                
                foreach (Road road in roadsToRotate)
                {
                    while (!road.AreNodesConnected())
                    {
                        road.transform.Rotate(0f, 90f, 0f);
                    }
                }
            }
        }
    }

    private void RotateBuilding()
    {
        currentBuilding.transform.Rotate(0f, 90f, 0f);
    }

    private void ChangeBuilding(GameObject newBuilding)
    {
        Destroy(currentBuilding);
        currentBuilding = Instantiate(newBuilding, _selectedGridPosition, Quaternion.identity);
        currentBuilding?.TryGetComponent(out _road);
    }
}