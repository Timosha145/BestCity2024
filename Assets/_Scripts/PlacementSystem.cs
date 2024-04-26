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
    [Header("Offset")]
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _extraOffset;
    [SerializeField] private Vector3 _cursorDefaultOffset;
    [Header("Materials")]
    [SerializeField] private Material _selectMaterial;
    [SerializeField] private Material _selectDangerMaterial;
    [Header("Layers")]
    [SerializeField] private LayerMask _buildingLayer;

    private Vector3 _cursorDefaultSize = Vector3.zero;
    private bool isBuilding = false;
    private GameObject currentObject;
    private Road _road;
    private Vector3 _selectedGridPosition = Vector3.zero;
    private Building _currentBuilding;
    private bool canBuild = true;

    public static PlacementSystem Instance { get; private set; }

    private void Awake()
    {
        _cursorDefaultSize = cellIndicator.transform.localScale;

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

        _selectedGridPosition = currentObject && !isObjectSquare() && (currentObject.transform.eulerAngles.y == 90 || currentObject.transform.eulerAngles.y == 270)
            ? grid.CellToWorld(grid.WorldToCell(mousePosition)) + new Vector3(0.5f, 0, 0.5f)
            : grid.CellToWorld(grid.WorldToCell(mousePosition));

        cellIndicator.transform.position = IsCursorDefaultSize()
                ? _selectedGridPosition + _offset + _cursorDefaultOffset
                : _selectedGridPosition + _offset;

        if (Input.GetKeyDown(KeyCode.B))
        {
            await Build();
        }

        if (Input.GetKeyDown(KeyCode.R) && currentObject)
        {
            RotateBuilding(currentObject);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(currentObject);
            ResetCursorIndicator();
            isBuilding = false;
            currentObject = null;
        }

        if (isBuilding && currentObject != null)
        {
            cellIndicator.transform.rotation = currentObject.transform.rotation;
            currentObject.transform.position = IsCursorDefaultSize()
                ? _selectedGridPosition + _offset + _cursorDefaultOffset
                : _selectedGridPosition + _offset;

            canBuild = !isBuildingColliding() && (_road || isCollidingWithRoad());

            if (canBuild)
            {
                _currentBuilding?.ChangeSelectMaterial(_selectMaterial);
                _road?.ChangeSelectMaterial(_selectMaterial);
            }
            else
            {
                currentObject.transform.position = currentObject.transform.position + _extraOffset;
                _currentBuilding?.ChangeSelectMaterial(_selectDangerMaterial);
                _road?.ChangeSelectMaterial(_selectDangerMaterial);
            }

            if (_currentBuilding)
            {
                _currentBuilding.Select();
                UpdateCursorIndicator(_currentBuilding._buildingSO);
            }
        }
    }

    private async Task Build()
    {
        if (!canBuild)
        {
            return;
        }

        isBuilding = !isBuilding;

        if (isBuilding)
        {
            currentObject = Instantiate(buildingPrefab, _selectedGridPosition, Quaternion.identity);
            currentObject?.TryGetComponent(out _road);

            if (currentObject.TryGetComponent(out Building building))
            {
                _currentBuilding = building;
                building.Select();
                UpdateCursorIndicator(building?._buildingSO);
            }
        }
        else
        {
            if (_currentBuilding)
            {
                if (_currentBuilding.CanBuild())
                {
                    _currentBuilding.isBuilt = true;
                    _currentBuilding.Pay();
                    _currentBuilding.Unselect();
                }
                else
                {
                    Destroy(currentObject);
                }
            }
            else if (currentObject.TryGetComponent(out Road road))
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
                Destroy(currentObject);
            }

            ResetCursorIndicator();
            _currentBuilding = null;
            currentObject = null;
        }
    }

    private bool isObjectSquare()
    {
        return _currentBuilding && _currentBuilding._buildingSO.widthX == _currentBuilding._buildingSO.lengthZ || _road;
    }

    private bool isBuildingColliding()
    {
        if (!currentObject)
        {
            return false;
        }

        Collider[] colliders = Physics.OverlapBox(cellIndicator.transform.position, cellIndicator.transform.localScale * 0.4f,
            cellIndicator.transform.rotation, _buildingLayer);

        if (colliders.Length == 1 && colliders[0].transform == currentObject.transform)
        {
            return false;
        }
        else
        {
            return colliders.Length > 0;
        }
    }

    private bool isCollidingWithRoad()
    {
        if (!_currentBuilding)
        {
            return false;
        }

        Collider[] colliders = Physics.OverlapBox(cellIndicator.transform.position, cellIndicator.transform.localScale * 0.6f,
            cellIndicator.transform.rotation, _buildingLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] && colliders[i].TryGetComponent(out Road road))
            {
                return true;
            }
        }

        return false;
    }

    private void RotateBuilding(GameObject building)
    {
        building.transform.Rotate(0f, 90f, 0f);
    }

    private bool IsCursorDefaultSize()
    {
        return cellIndicator.transform.localScale == _cursorDefaultSize;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(cellIndicator.transform.position, cellIndicator.transform.localScale * 0.8f);
    }
}

