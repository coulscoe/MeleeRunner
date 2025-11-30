using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{

    [SerializeField] private DoorBehavior door;
    [SerializeField] private float fastOpenSpeed = 10f;
    [SerializeField] private List<string> tagWhitelist = new List<string>();

    private void OnTriggerEnter(Collider other)
    {
        bool isWhitelisted = false;

        foreach (string tag in tagWhitelist)
        {
            if (other.CompareTag(tag))
            {
                isWhitelisted = true;
            }
        }

        if (isWhitelisted == false) { return; }

        MovementHandler movement = other.GetComponent<MovementHandler>();

        if (movement == null || movement.currentSpeed <= fastOpenSpeed)
        {
            door.Open();
        } else
        {
            door.FastOpen();
        }
    }

}
