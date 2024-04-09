using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject cellIndicator; // в c# private поля лучше пиши начиная с нижнего подчеркивания _foo
    [SerializeField] private GameObject buildingPrefab; // вместо GameObject лучше transform, так как нас интересует именно свойства позиции, не более (gameobject всегда можно получить из transform)
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private Vector3 _offset;
    [field: SerializeField] public List<Road> roadVariants { get; private set; }

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
            Build();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(currentBuilding);
            isBuilding = false;
            currentBuilding = null;
        }

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    RotateBuilding(currentBuilding);
        //}

        if (isBuilding && currentBuilding != null)
        {
            currentBuilding.transform.position = _selectedGridPosition;
        }
    }

    private void Build()
    {
        isBuilding = !isBuilding;

        if (isBuilding)
        {
            currentBuilding = Instantiate(buildingPrefab, _selectedGridPosition, Quaternion.identity);
            currentBuilding?.TryGetComponent(out _road);
        }
        else
        {
            List<Road> roadsToRotate = new List<Road> { _road };
            roadsToRotate.AddRange(_road.GetCollidingRoads(_road.transform.position));

            foreach (Road road in roadsToRotate)
            {
                Road suitableRoad = road.GetSuitableRoad();
                GameObject newBuilding = ChangeBuilding(road.gameObject, suitableRoad.gameObject);
                Road newRoad = newBuilding.GetComponent<Road>();

                for (int i = 0; i < 10; i++)
                {
                    RotateBuilding(newBuilding);

                    if (newRoad.AreNodesConnected())
                    {
                        break;
                    }
                }
            }
        }
    }

    private void RotateBuilding(GameObject building)
    {
        building.transform.Rotate(0f, 90f, 0f);
    }

    private GameObject ChangeBuilding(GameObject oldBuilding, GameObject newBuilding)
    {
        Destroy(oldBuilding);
        return Instantiate(newBuilding, oldBuilding.transform.position, Quaternion.identity);
    }
}