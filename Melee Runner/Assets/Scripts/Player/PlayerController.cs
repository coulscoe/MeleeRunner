using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private SwordBehavior sword;
    [SerializeField] private PlayerAnimationHandler playerAnimation;
    [SerializeField] private PlayerInput input;
    [SerializeField] private MovementHandler movement;
    [SerializeField] private JumpBehavior jump;
    [SerializeField] private SprintBehavior sprint;
    [SerializeField] private FPCameraHandler cameraController;

    void OnEnable()
    {
        input.onJump += HandleJumpPressed;
        input.onClick += HandleSlash;
    }

    void OnDisable()
    {
        input.onJump -= HandleJumpPressed;
        input.onClick -= HandleSlash;
    }

    void Update()
    {
        cameraController.LookUpdate(input.lookInput);
        sprint.handleSprint(input.sprintInput);
        movement.VelocityUpdate(input.moveInput);
        jump.Tick();
        playerAnimation.UpdateAnimations(movement.isGrounded);
    }

    void HandleJumpPressed()
    {
        jump.QueueJump();
    }
    
    void HandleSlash()
    {
        sword.QueueSwing();
    }
}
