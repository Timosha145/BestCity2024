using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject buildingPrefab;
    [SerializeField] private InputManager inputManager;
    [Header("Offset")]
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _extraOffset;
    [SerializeField] private Vector3 _cursorDefaultOffset;
    [Header("Materials")]
    [SerializeField] private Material _selectMaterial;
    [SerializeField] private Material _selectDangerMaterial;
    [Header("Layers")]
    [SerializeField] private LayerMask _buildingLayer;
    [Header("Grid System")]
    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private GameObject gridPlane;
    [SerializeField] private Grid grid;

    private Vector3 _cursorDefaultSize = Vector3.zero;
    private Vector3 _selectedGridPosition = Vector3.zero;
    private Quaternion _rotation = Quaternion.identity;
    private GameObject currentObject;
    private Road _road;
    private Building _currentBuilding;
    private bool _objectPreview = false;
    private bool canBuild = true;
    private bool _active = false;

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

    private void Start()
    {
        ToggleGrid(false);
        GameManager.Instance.onChangeMode += GameManagerOnChangeMode;
    }

    private void OnDestroy()
    {
        GameManager.Instance.onChangeMode -= GameManagerOnChangeMode;
    }

    private void GameManagerOnChangeMode(object sender, GameManager.ModeEventArgs e)
    {
        CancelBuilding();
        _active = e.newMode == GameManager.Mode.building;
        ToggleGrid(_active);
    }

    private void ToggleGrid(bool toggle)
    {
        gridPlane.SetActive(toggle);
        cellIndicator.SetActive(toggle);
    }

    private async void Update()
    {
        if (!_active)
        {
            return;
        }

        Vector3 mousePosition = inputManager.GetSelectedMapPosition(out bool hits);

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
            CancelBuilding();
        }

        if (_objectPreview && currentObject != null)
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
                UpdateCursorIndicator(_currentBuilding);
            }
        }
    }

    private void CancelBuilding()
    {
        Destroy(currentObject);
        ResetCursorIndicator();
        _objectPreview = false;
        canBuild = true;
        currentObject = null;
    }

    private void initObject()
    {
        currentObject = Instantiate(buildingPrefab, _selectedGridPosition, _rotation);
        currentObject?.TryGetComponent(out _road);

        if (currentObject.TryGetComponent(out Building building))
        {
            _currentBuilding = building;
            building.Select();
            UpdateCursorIndicator(building);
        }
    }

    private async Task Build()
    {
        if (!canBuild)
        {
            return;
        }

        _objectPreview = !_objectPreview;

        if (_objectPreview)
        {
            initObject();
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

                foreach (Road roadToRotate in roadsToRotate)
                {
                    Road suitableRoad = roadToRotate.GetSuitableRoad();
                    GameObject newBuilding = ChangeBuilding(roadToRotate.gameObject, suitableRoad.gameObject);
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
            }

            _currentBuilding = null;
            currentObject = null;
            _objectPreview = true;
            initObject();
            ResetCursorIndicator();
        }
    }

    private bool isObjectSquare()
    {
        Debug.Log("SQUARE: " + (_currentBuilding && _currentBuilding.widthX == _currentBuilding.lengthZ || _road));
        return _currentBuilding && _currentBuilding.widthX == _currentBuilding.lengthZ || _road;
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
        _rotation = building.transform.rotation;
    }

    private bool IsCursorDefaultSize()
    {
        return cellIndicator.transform.localScale == _cursorDefaultSize;
    }

    public void UpdateCursorIndicator(Building building)
    {
        Vector3 cursorSize = new Vector3(building.widthX, 1f, building.lengthZ);
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

