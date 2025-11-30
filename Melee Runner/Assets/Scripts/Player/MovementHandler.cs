using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class MovementHandler : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 8;
    public float sprintSpeed = 12;
    public float acceleration = 24;
    public float weight = 3;

    [Header("Components")]
    private CharacterController characterController;

    [Header("Info")]
    public Vector3 velocity;
    public float verticalVelocity;
    public float currentSpeed;

    public bool isGrounded = true;
    public float maxSpeed = 8;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void VelocityUpdate(Vector2 moveInput)
    {
        Vector3 inputDir = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized;
        Vector3 horizontalVelocity = new Vector3(velocity.x, 0f, velocity.z);

        isGrounded = characterController.isGrounded;

        // Gain Velocity, or Slow to stop
        if (inputDir.sqrMagnitude >= 0.01f)
        {
            horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, inputDir * maxSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, Vector3.zero, acceleration * Time.deltaTime);
        }

        // assign horizontal back to velocity
        velocity.x = horizontalVelocity.x;
        velocity.z = horizontalVelocity.z;

        
        // Gravity
        if (characterController.isGrounded && velocity.y <= 0.01f)
        {
            velocity.y = -3f; // small downward push
        }
        else
        {
            velocity.y = Mathf.MoveTowards(velocity.y, -51f, math.abs(Physics.gravity.y) * weight * Time.deltaTime);
        }

        characterController.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector3(velocity.x, 0f, velocity.z).magnitude;
    }

    public void ApplyForce(Vector3 force)
    {
        velocity += force;
    }
}
