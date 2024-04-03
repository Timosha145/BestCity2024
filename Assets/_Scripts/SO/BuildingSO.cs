using UnityEngine;

[CreateAssetMenu(menuName = "Building System/Standard")] // Добавляет возможность создать SO в панеле быстро доступа при ПКМ по иерархии
public class BuildingSO : ScriptableObject
{
    // Свойства с маленькой буквы + если корректное название, то комментарий не нужно ставить
    public int width { get; private set; }
    public int length { get; private set; }
    public float buildSpeed { get; private set; }
    public int waterRequirement { get; private set; }
    public int electricityRequirement { get; private set; }
    public int Level { get; set; }

    // Поля, так как работаем напрямую с ними
    private int Foo1;
    public string Foo2;

    // Свойства, так как работаем с копией значения
    public string Foo3 { get; protected set; }
    public float Foo4 { get; private set; }
}
