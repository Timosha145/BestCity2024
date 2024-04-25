using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] public List<Road> roadVariants;
    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private GameObject buildingPrefab;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private Vector3 _offset;

    [SerializeField] private Vector3 residentialOffset;
    [SerializeField] private Vector3 commercialOffset;

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
        _selectedGridPosition = grid.CellToWorld(grid.WorldToCell(mousePosition));
        cellIndicator.transform.position = _selectedGridPosition + _offset;

        if (Input.GetKeyDown(KeyCode.B))
        {
            await Build();
        }

        if (Input.GetKeyDown(KeyCode.R) && currentBuilding)
        {
            RotateBuilding(currentBuilding);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(currentBuilding);
            isBuilding = false;
            currentBuilding = null;
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

            if (currentBuilding.TryGetComponent(out Building building))
            {
                UpdateCursorIndicator(building._buildingSO);
            }
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

            ResetCursorIndicator();

            currentBuilding = null;
        }
    }

    private void RotateBuilding(GameObject building)
    {
        building.transform.Rotate(0f, 90f, 0f);
    }

    public void UpdateCursorIndicator(BuildingSO buildingSO)
    {
        Vector3 cursorSize = new Vector3(buildingSO.widthX, 1f, buildingSO.lengthZ);
        cellIndicator.transform.localScale = cursorSize;

        cellIndicator.transform.position = _selectedGridPosition + _offset;
    }

    public void ResetCursorIndicator()
    {
        Vector3 cursorSize = new Vector3(1f, 1f, 1f);
        cellIndicator.transform.localScale = cursorSize;
    }

    public void ChangePrefab(GameObject building)
    {
        buildingPrefab = building;
    }

    public GameObject ChangeBuilding(GameObject oldBuilding, GameObject newBuilding)
    {
        Destroy(oldBuilding);
        return Instantiate(newBuilding, oldBuilding.transform.position, Quaternion.identity);
    }
}

