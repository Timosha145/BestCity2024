using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridButton : MonoBehaviour
{
    public GameObject Plane;
    private bool isPlaneVisible  = true;
    void Start()
    {
        if (Plane == null)
        {
            Debug.LogError("Plane не установлен для кнопки GridButton!");
        }
    }

    public void TogglePlaneVisibility()
    {
        isPlaneVisible = !isPlaneVisible;
        Plane.SetActive(isPlaneVisible);
    }

}
