using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [field: Header("Prefabs")]
    [field: SerializeField] public List<Residence> residencePrefabs { get; private set; }
    [field: SerializeField] public List<Road> roadPrefabs { get; private set; }
    [field: Header("Game Settings")]
    [field: SerializeField] public int population { get; set; }
    [field: SerializeField] public int employed { get; private set; }
    [field: SerializeField] public float jobCount { get; set; }
    [field: SerializeField] public float money { get; set; }
    [field: SerializeField] public float materials { get; set; }
    [field: SerializeField] public float products { get; set; }

    public Mode currentMode { get; private set; } = Mode.idle;
    public event EventHandler<ModeEventArgs> onChangeMode;

    public class ModeEventArgs : EventArgs
    {
        public readonly Mode newMode;

        public ModeEventArgs(Mode mode)
        {
            newMode = mode;
        }
    }

    public enum Mode
    {
        building,
        destroying,
        idle
    }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Mode newMode = currentMode == Mode.building ? Mode.idle : Mode.building;
            changeMode(newMode);
        }
    }

    public int Employ(int people)
    {
        int employedSuccess = Mathf.Clamp(people, 0, population - employed);
        employed += employedSuccess;

        return employedSuccess;
    }

    public void Unemploy(int people)
    {
        employed -= people;
    }

    public void changeMode(Mode mode)
    {
        currentMode = mode;
        onChangeMode?.Invoke(null, new ModeEventArgs(mode));
    }
}
