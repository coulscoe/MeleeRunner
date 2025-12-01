using UnityEngine;

public class RoomBehavior : MonoBehaviour
{
    [Header("Data")]
    public bool hasEntered = false;
    public int id;

    [Header("Components")]
    public RoomInfoSO roomInfo;
    public RoomBoundaryBehavior roomBoundary;
    public DoorBehavior enteranceDoor;
    public VoidPointBehavior voidPoint; // Determines void placement

    [Header("Managers")]
    public RoomManager roomManager;

    #region Unity Functions

    public void OnValidate()
    {
        // Auto Populate Room Boundary
        if (roomBoundary == null)
        {
            roomBoundary = transform.GetComponentInChildren<RoomBoundaryBehavior>();
        }
        if (enteranceDoor == null)
        {
            enteranceDoor = transform.GetComponentInChildren<DoorBehavior>();
        }
        if (voidPoint == null)
        {
            voidPoint = transform.GetComponentInChildren<VoidPointBehavior>();
        }
    }

    public void Awake()
    {
        if (enteranceDoor != null)
        {
            Debug.Log("Door found");
            enteranceDoor.onFastOpen.AddListener(OnPlayerEnter);
            enteranceDoor.onOpen.AddListener(OnPlayerEnter);
        }
        
    }

    #endregion

    #region Actions

    public void OnPlayerEnter()
    {
        if (!hasEntered)
        {
            hasEntered = true;
            roomManager.OnRoomEnter(id);
        }
    }

    #endregion

}
