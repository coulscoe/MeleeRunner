using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RoomManager : MonoBehaviour
{
    public RoomPlacerBehavior roomPlacer;

    [Header("Settings")]
    [SerializeField] private int roomsAhead;
    [SerializeField] private int roomsBehind;

    [Header("Events")]
    public UnityEvent onRemoveRoom, onAddRoom;

    [Header("Rooms")]
    public Dictionary<int, RoomBehavior> activeRooms = new Dictionary<int, RoomBehavior>();

    [Header("Info")]
    public RoomBehavior lastRoom = null;
    public int playerCurrentRoom = 0;

    public void addRooms(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            addRoom();
        }
    }

    public void addRoom()
    {
        int id = lastRoom ? lastRoom.id + 1 : 1;
        Debug.Log("Trying to Place Room: " + id);
        RoomBehavior newRoom = roomPlacer.CreateRoom();

        if (newRoom == null)
        {
            return;
        }

        registerRoom(newRoom);
        onAddRoom.Invoke();
    }

    public void removeRoom(int roomId)
    {
        if (activeRooms.ContainsKey(roomId))
        {
            RoomBehavior room = activeRooms[roomId];
            if (room != null)
            {
                Destroy(room.gameObject);
                activeRooms.Remove(roomId);
                RoomBehavior currentFirstRoom = GetFirstRoom();
                currentFirstRoom.enteranceDoor.Lock();
                onRemoveRoom.Invoke();
                Debug.Log("Removed Room: " + roomId);
            }
        }
    }

    public void removeRooms(int min, int max)
    {
        for (int roomId = min; roomId <= max; roomId++)
        {
            removeRoom(roomId);
        }
    }

    public void registerRoom(RoomBehavior newRoom)
    {
        Debug.Log("Registering Room");
        int id = lastRoom ? lastRoom.id + 1 : 1;

        // Room Register
        newRoom.id = id;
        newRoom.roomManager = this;

        activeRooms.Add(newRoom.id, newRoom);
        lastRoom = newRoom;
    }

    public void OnRoomEnter(int roomId)
    {
        Debug.Log("Player entered room with ID: " + roomId);
        if (playerCurrentRoom >= roomId) { return; }
        int roomFrom = playerCurrentRoom;
        int roomTo = roomId;

        playerCurrentRoom = roomTo;

        int roomsToGenerate = roomTo + roomsAhead - lastRoom.id;
        int roomsToDeleteMin = roomFrom - roomsBehind;
        int roomsToDeleteMax = roomTo - roomsBehind - 1;

        addRooms(roomsToGenerate);
        removeRooms(roomsToDeleteMin, roomsToDeleteMax);
        
    }

    #region Helper Functions

    public RoomBehavior GetFirstRoom()
    {
        return activeRooms[activeRooms.Keys.Min()];
    }

    #endregion


}
