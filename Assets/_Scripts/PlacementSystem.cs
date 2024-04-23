using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private GameObject buildingPrefab;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private Vector3 _offset;

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

    private async void Update()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        _selectedGridPosition = grid.CellToWorld(grid.WorldToCell(mousePosition)) + _offset;
        cellIndicator.transform.position = _selectedGridPosition;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            await Build();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(currentBuilding);
            isBuilding = false;
            currentBuilding = null; // Assigning null after destroying the current building
        }

        if (isBuilding && currentBuilding != null)
        {
            currentBuilding.transform.position = _selectedGridPosition;
        }
    }

    private async Task Build()
    {
        isBuilding = !isBuilding;

        if (isBuilding)
        {
            currentBuilding = Instantiate(buildingPrefab, _selectedGridPosition, Quaternion.identity);
            currentBuilding?.TryGetComponent(out _road);
        }
        else
        {
            if (currentBuilding.TryGetComponent(out Building building))
            {
                if (building.CanBuild())
                {
                    building.isBuilt = true;
                    building.Pay();
                    building.Unselect();
                }
                else
                {
                    Destroy(currentBuilding);
                }
            }
            else if (currentBuilding.TryGetComponent(out Road road))
            {
                List<Road> roadsToRotate = new List<Road> { road };
                roadsToRotate.AddRange(road.GetCollidingRoads(road.transform.position));

                foreach (Road r in roadsToRotate)
                {
                    Road suitableRoad = r.GetSuitableRoad();
                    GameObject newBuilding = ChangeBuilding(r.gameObject, suitableRoad.gameObject);
                    Road newRoad = newBuilding.GetComponent<Road>();
                    await Task.Delay(TimeSpan.FromSeconds(0.025f));

                    for (int i = 0; i < 10; i++)
                    {
                        if (newRoad.AreNodesConnected())
                        {
                            break;
                        }

                        RotateBuilding(newBuilding);
                    }
                }
                Destroy(currentBuilding);
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