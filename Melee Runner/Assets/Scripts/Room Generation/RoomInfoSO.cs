using UnityEngine;

[CreateAssetMenu(fileName = "RoomInfoSO", menuName = "Scriptable Objects/RoomStuff/RoomInfoSO")]
public class RoomInfoSO : ScriptableObject
{
    [Header("Components")]
    public GameObject roomPrefab;
    public GameObject boundaryPrefab;

    [Header("Settings")]
    public float spawnWeight = 1.0f;
    public float roomTimer = 5f;
    public RoomDirection direction = RoomDirection.Forward;
}

public enum RoomDirection
{
    Forward,
    Right,
    Left
}
