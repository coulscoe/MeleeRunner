using System;
using UnityEngine;

// Game-specific Code

[RequireComponent(typeof(Animator))]
public class PlayerAnimationHandler : MonoBehaviour
{

    [Header("Components")]
    public Animator playerAnimator;
    public MovementHandler movementHandler;
    public SwordBehavior swordBehavior;

    //TODO: Move Speed to a speed handler script to handle FOV, Anim Speed, Etc? (Or for in camera,)
    [Header("Settings")]
    public float speedDivider = 10;
    public float speedMax = 3;
    public float speedMin = .2f;
    public float speed => Math.Clamp(movementHandler.currentSpeed / speedDivider, speedMin, speedMax);

    public void UpdateAnimations(bool isGrounded)
    {
        playerAnimator.SetFloat("Speed", speed);
        playerAnimator.SetBool("Grounded", isGrounded);
    }
        
}
