using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RoomPlacerBehavior : MonoBehaviour
{
    [Header("Data")]
    public RoomSetSO roomSet;

    [Header("Settings")]
    public int attemptsToSpawn = 10;

    [Header("Exit Door Cells")]
    public int doorSpaceDepth = 2;
    public int doorSpaceWidth = 1;

    private List<Vector2Int> lastRoomCells = new List<Vector2Int>();

    [Header("Components")]
    public RoomManager roomManager;
    public GridOccupancyBehavior gridOccupancy;
    public Transform roomParent;

    [Header("Info")]
    public float roomBalance = 0;
    public float maxBalance = 1;

    #region Actions

    public RoomBehavior CreateRoom()
    {
        // Pick Room
        RoomInfoSO roomInfo = PickRoom();

        // Return if no room was selected
        if (roomInfo == null)
        {
            Debug.LogWarning("Could Not Spawn Room");
            return null;
        }

        // Place Room
        Pose spawnLocation = GetSpawnPose();
        GameObject roomObj = Instantiate(roomInfo.roomPrefab, spawnLocation.position, spawnLocation.rotation, roomParent);
        RoomBehavior newRoom = roomObj.GetComponent<RoomBehavior>();

        // Update Room Balance
        UpdateRoomBalance(newRoom);

        // Give Placed Room Its Own Information
        newRoom.roomInfo = roomInfo;

        // Delay occupy cells for next time this is ran
        BoundaryBoxBehavior newRoomBounds = newRoom.roomBoundary.boundaryBox;
        DelayOccupyCells(newRoomBounds);

        return newRoom;
    }

    public RoomInfoSO PickRoom()
    {
        for (int attempt = 0; attempt < attemptsToSpawn; attempt++)
        {
            RoomInfoSO roomInfo = GetRandomRoom();
            if (roomInfo != null && CanPlaceRoom(roomInfo))
            {
                return roomInfo;
            }
        }

        return null;
    }
    
    public void DelayOccupyCells(BoundaryBoxBehavior newRoomBounds)
    {
        if (lastRoomCells.Count > 0)
        {
            gridOccupancy.SetOccupancy(lastRoomCells, OccupancyState.Occupied);
        }

        lastRoomCells = new List<Vector2Int>();

        foreach (GameObject boundary in newRoomBounds.boundaryObjects)
        {
            Bounds bound = boundary.GetComponent<Renderer>().bounds;
            lastRoomCells.AddRange(gridOccupancy.GetOccupiedCells(bound));
        }
    }

    #endregion

    #region Validation

    private bool CanPlaceRoom(RoomInfoSO roomInfo)
    {
        GameObject boundaryPrefab = roomInfo.boundaryPrefab;
        GameObject roomPrefab = roomInfo.roomPrefab;
        Debug.Log("RoomPrefab: " + roomPrefab.name);

        if (roomPrefab == null)
        {
            Debug.LogWarning($"RoomInfo: {roomInfo.name} is missing a room prefab!");
            return false;
        }
        if (boundaryPrefab == null)
        {
            Debug.LogWarning($"RoomInfo: {roomInfo.name} is missing a boundary component!");
            return false;
        }
        if (roomPrefab.GetComponent<RoomBehavior>() == null)
        {
            Debug.LogWarning($"{roomPrefab.name} is missing Room Behavior Component");
        }

        bool canPlace = true;

        // Spawn Test Boundary
        Pose spawnLocation = GetSpawnPose();
        GameObject boundary = Instantiate(boundaryPrefab, spawnLocation.position, spawnLocation.rotation);
        RoomBoundaryBehavior boundaryRoom = boundary.GetComponent<RoomBoundaryBehavior>();
        BoundaryBoxBehavior boundaryBox = boundaryRoom.boundaryBox;

        // Check Test Boundary
        if (!DoBoundariesOverlap(boundaryBox)) { canPlace = false; }
        if (!CheckOccupancy(boundaryBox)) { canPlace = false; }
        if (!CheckDoorSpace(roomInfo, boundaryRoom)) { canPlace = false; }

        boundaryBox.DisableBoundaries();
        Destroy(boundary);

        return canPlace;
    }

    private bool DoBoundariesOverlap(BoundaryBoxBehavior boundaryBox)
    {
        if (boundaryBox == null) { return false; }

        LayerMask layerMask = LayerMask.GetMask("Boundaries");
        List<GameObject> hits = boundaryBox.getOverlappingObjects(layerMask, "Boundaries", 0.01f);

        if (hits.Count > 0) { Debug.LogWarning("Hit Boundary"); return false; }

        return true;
    }

    private bool CheckOccupancy(BoundaryBoxBehavior boundaryBox)
    {
        foreach (GameObject boundary in boundaryBox.boundaryObjects)
        {
            if (gridOccupancy.IsOccupied(boundary)) { Debug.Log("Occupied"); return false; }
        }
        return true;
    }

    private bool CheckDoorSpace(RoomInfoSO roomInfo, RoomBoundaryBehavior boundaryRoom)
    {
        GameObject exit = boundaryRoom.exit;
        Bounds exitCollider = exit.GetComponent<Renderer>().bounds;

        List<Vector2Int> doorCellSpaces = new List<Vector2Int>();
        doorCellSpaces = gridOccupancy.GetOccupiedCells(exitCollider);

        List<Vector2Int> areaCellSpaces = new List<Vector2Int>(doorCellSpaces);

        // Get Direction
        Vector2Int direction = GetDirectionValue(exit);
        Vector2Int right = new Vector2Int(-direction.y, direction.x);

        // Get Cells
        foreach (Vector2Int doorCell in doorCellSpaces)
        {

            for (int depth = 0; depth <= doorSpaceDepth; depth++) // 0, 1
            {
                for (int width = -doorSpaceWidth; width <= doorSpaceWidth; width++) // -1, 0, 1
                {

                    Vector2Int offset = direction * depth + right * width;

                    areaCellSpaces.Add(doorCell + offset);
                }
            }
        }


        // gridOccupancy.visualizeCells(areaCellSpaces); // See Door Spaces

        if (gridOccupancy.doCellsOverlap(lastRoomCells, areaCellSpaces)) { Debug.LogWarning("Door Ovelap With Previous Room"); return false; }

        foreach (Vector2Int cell in areaCellSpaces)
        {
            if (gridOccupancy.IsCellOccupied(cell))
            {
                Debug.Log("Exit Space Cell Taken");
                return false;
            }
        }

        return true;
    }

    private void UpdateRoomBalance(RoomBehavior newRoom)
    {
        Debug.Log("Updating Room Balance");
        RoomInfoSO roomInfo = newRoom.roomInfo;
        RoomDirection roomDirection = roomInfo.direction;
        Debug.Log(roomDirection);
        switch (roomDirection)
        {
            case RoomDirection.Left:
                roomBalance--;
                break;
            case RoomDirection.Right:
                roomBalance++;
                break;
            default:
                break;
        };
        Debug.Log(roomBalance);
    }

    #endregion

    #region Helper Functions

    public Pose GetSpawnPose()
    {
        Pose pose = new Pose(Vector3.zero, Quaternion.identity);
        RoomBehavior lastRoom = roomManager.lastRoom;
        if (lastRoom == null) { return pose; }

        RoomBoundaryBehavior boundaryRoom = lastRoom.roomBoundary;
        GameObject lastExit = boundaryRoom.exit;

        if (lastExit != null)
        {
            pose.position = lastExit.transform.position;
            pose.rotation = lastExit.transform.rotation;
        }

        return pose;
    }

    public Vector2Int GetDirectionValue(GameObject gameObject)
    {
        Vector3 forward = gameObject.transform.forward;
        Vector2 forward2d = new Vector2(forward.x, forward.z).normalized;

        if (math.abs(forward2d.x) > math.abs(forward2d.y))
        {
            forward2d = new Vector2(math.sign(forward2d.x), 0);
        }
        else
        {
            forward2d = new Vector2(0, math.sign(forward2d.y));
        }

        Vector2Int forward2dInt = Vector2Int.RoundToInt(forward2d);

        return forward2dInt;

    }

    public RoomInfoSO GetRandomRoom()
    {
        List<RoomInfoSO> possibleRooms = PossibleRooms(roomSet.rooms);
        float totalWeight = 0f;

        // Calculate Total Weight
        foreach (RoomInfoSO room in possibleRooms)
        {
            totalWeight += room.spawnWeight;
        }

        // Get Random Value
        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        // Select Room Based on Weight
        foreach (RoomInfoSO room in possibleRooms)
        {
            cumulativeWeight += room.spawnWeight;
            if (randomValue <= cumulativeWeight)
            {
                return room;
            }
        }

        Debug.LogError("GetRandomRoom: No room selected, check spawn weights calculations.");
        return null; // Fallback (shouldn't reach here if weights are set correctly)

    } 

    public List<RoomInfoSO> PossibleRooms(List<RoomInfoSO> list) // Gots to be changed
    {
        List<RoomInfoSO> possibleRooms = list;

        if (roomBalance <= -maxBalance)
        {
            possibleRooms = RemoveDirectionFromList(possibleRooms, RoomDirection.Left);
        }
        else if (roomBalance >= maxBalance)
        {
            possibleRooms = RemoveDirectionFromList(possibleRooms, RoomDirection.Right);
        }

        return possibleRooms;
    }

    private List<RoomInfoSO> RemoveDirectionFromList(List<RoomInfoSO> list, RoomDirection dir)
    {
        List<RoomInfoSO> newList = new List<RoomInfoSO>(list);
        foreach (RoomInfoSO room in list)
        {
            if (room.direction == dir)
            {
                newList.Remove(room);
            }
        }
        return newList;
    }

    #endregion
}

