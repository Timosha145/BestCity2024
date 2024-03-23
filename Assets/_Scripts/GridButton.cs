using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridButton : MonoBehaviour
{
    public GameObject Plane;
    public GameObject CoursorIndicator; // Добавляем переменную для CoursorIndicator
    private bool isPlaneVisible  = true;
    private bool isIndicatorVisible = true; // Добавляем переменную для отслеживания видимости CoursorIndicator

    void Start()
    {
        if (Plane == null)
        {
            Debug.LogError("Plane не установлен для кнопки GridButton!");
        }
        if (CoursorIndicator == null) // Проверяем, установлен ли CoursorIndicator
        {
            Debug.LogError("CoursorIndicator не установлен для кнопки GridButton!");
        }
    }

    public void TogglePlaneVisibility()
    {
        isPlaneVisible = !isPlaneVisible;
        Plane.SetActive(isPlaneVisible);

        // Инвертируем видимость для CoursorIndicator и устанавливаем его активность
        isIndicatorVisible = !isIndicatorVisible;
        CoursorIndicator.SetActive(isIndicatorVisible);
    }
}
