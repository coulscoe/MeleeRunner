using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MovementHandler))]
public class JumpBehavior : MonoBehaviour
{
    [Header("Components")]
    private MovementHandler movementHandler;

    [Header("Jump Settings")]
    public float jumpPower = 8;
    public float doubleJumps = 1;
    public float cayoteTime = .2f;
    public float bufferTime = .15f;

    [Header("Events")]
    public UnityEvent onJump, onDoubleJump;

    [Header("Info")]
    public bool hasJumped = false;
    public float remainingDoubleJumps;
    public float cayoteTimer;
    public float bufferTimer;

    public void Awake()
    {
        movementHandler = GetComponent<MovementHandler>();
    }

    public void Tick()
    {
        cayoteTimer -= Time.deltaTime;

        if (movementHandler.isGrounded)
        {
            hasJumped = false;
            cayoteTimer = cayoteTime;
            remainingDoubleJumps = doubleJumps;
        }

        if (bufferTimer > 0)
        {
            bufferTimer -= Time.deltaTime;

            if (cayoteTimer > 0 && !hasJumped)
            {
                Jump();
            }
            else if (remainingDoubleJumps > 0)
            {
                DoubleJump();
            }
        }
    }

    public void QueueJump()
    {
        bufferTimer = bufferTime;
    }

    public void Jump()
    {
        onJump.Invoke();

        hasJumped = true;
        bufferTimer = 0;

        Vector3 jumpForce = new Vector3(0f, Mathf.Sqrt(jumpPower * -2f * Physics.gravity.y), 0f);
        movementHandler.ApplyForce(jumpForce);
    }

    public void DoubleJump()
    {
        Debug.Log("Double Jump");
        onDoubleJump.Invoke();

        remainingDoubleJumps -= 1;
        bufferTimer = 0;

        Vector3 jumpForce = new Vector3(0f, Mathf.Sqrt(jumpPower * -2f * Physics.gravity.y), 0f);
        movementHandler.ApplyForce(jumpForce);
    }
}
