using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VoidBehavior : MonoBehaviour
{
    
    [SerializeField] private RoomManager roomManager;

    void Awake()
    {
        roomManager.onAddRoom.AddListener(UpdateVoid);
    }

    public void UpdateVoid()
    {
        List<RoomBehavior> activeRooms = roomManager.activeRooms.Values.ToList();
        Vector3 lowestPoint = new Vector3 (0, float.PositiveInfinity, 0);

        foreach(RoomBehavior room in activeRooms)
        {
            Vector3 voidPos = room.voidPoint.transform.position;
            if (voidPos.y < lowestPoint.y)
            {
                lowestPoint = voidPos;
            }
        }

        transform.position = lowestPoint;
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerBehavior player))
        {
            player.TakeDamage(2142069);
        }
    }
}
