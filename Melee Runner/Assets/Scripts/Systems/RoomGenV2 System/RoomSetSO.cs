using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomSetSO", menuName = "Scriptable Objects/RoomStuff/RoomSetSO")]
public class RoomSetSO : ScriptableObject
{

    public List<RoomInfoSO> rooms = new List<RoomInfoSO>();
    
}
