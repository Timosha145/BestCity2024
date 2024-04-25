using UnityEngine;

[CreateAssetMenu(menuName = "Building System/Standard")]
public class BuildingSO : ScriptableObject
{
    [field: Header("Main Settings")]
    [field: SerializeField] public int cost { get; private set; }
    [field: SerializeField] public float waitUntilDestroying { get; private set; }
    [field: SerializeField] public int widthX;
    [field: SerializeField] public int lengthZ;
}
