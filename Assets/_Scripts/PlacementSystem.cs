using System.Collections;
using System.Collections.Generic;
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
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        _selectedGridPosition = grid.CellToWorld(grid.WorldToCell(mousePosition)) + _offset;
        cellIndicator.transform.position = _selectedGridPosition;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            isBuilding = !isBuilding;

            if (isBuilding)
            {
                currentBuilding = Instantiate(buildingPrefab, _selectedGridPosition, Quaternion.identity);

                if (currentBuilding.TryGetComponent(out Building building))
                {
                    building.Select();
                }
            }
            else
            {
                if (currentBuilding.TryGetComponent(out Building building) && building.CanBuild())
                {
                    building.isBuilt = true;
                    building.Pay();
                    building.Unselect();
                }
                else
                {
                    Destroy(currentBuilding);
                }

                currentBuilding = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(currentBuilding);
            isBuilding = false;
            currentBuilding = null;
        }

        if (Input.GetKeyDown(KeyCode.R) && currentBuilding)
        {
            RotateBuilding(currentBuilding);
        }

        if (isBuilding && currentBuilding != null)
        {
            currentBuilding.transform.position = _selectedGridPosition;
        }
    }

    private void RotateBuilding(GameObject building)
    {
        building.transform.Rotate(0f, 90f, 0f);
    }
}