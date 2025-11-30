using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [Header("Inputs")]
    public Vector2 moveInput;
    public Vector2 lookInput;
    public bool sprintInput;

    // Events
    public event Action onJump = delegate {};
    public event Action onToss = delegate {};
    public event Action onClick = delegate {};

    #region Functions

    //TODO: Keep here?
    public void LockMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnlockMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    #endregion

    #region Unity Methods
    
    private void OnMouseOne(InputValue input)
    {
        onClick.Invoke();
    }

    private void OnMove(InputValue input)
    {
        moveInput = input.Get<Vector2>();
    }

    private void OnLook(InputValue input)
    {
        lookInput = input.Get<Vector2>();
    }

    private void OnSprint(InputValue input)
    {
        sprintInput = input.isPressed;
    }

    private void OnJump(InputValue input)
    {
        if (input.isPressed)
        {
            onJump.Invoke();
        }
    }

    private void OnToss(InputValue input)
    {
        if (input.isPressed)
        {
            onToss.Invoke();
        }
    }

    #endregion
}
