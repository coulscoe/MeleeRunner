using UnityEngine;

[RequireComponent(typeof(MovementHandler))]
public class SprintBehavior : MonoBehaviour
{

    MovementHandler movementHandler;

    public void Awake()
    {
        movementHandler = GetComponent<MovementHandler>();
    }

    public void handleSprint(bool isSprinting)
    {
        if (isSprinting)
        {
            movementHandler.maxSpeed = movementHandler.sprintSpeed;
        } else
        {
            movementHandler.maxSpeed = movementHandler.walkSpeed;
        }
    }

}
