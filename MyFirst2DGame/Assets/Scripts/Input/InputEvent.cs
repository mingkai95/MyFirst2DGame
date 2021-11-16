using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputEvent", menuName = "Game/Input Event")]
public class InputEvent : ScriptableObject, PlayerInput.IGamePlayActions
{
    public event UnityAction<Vector2> moveEvent;
    public event UnityAction jumpEvent;
    public event UnityAction attackEvent;

    PlayerInput playerInput;

    void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = new PlayerInput();
            playerInput.GamePlay.SetCallbacks(this);

        }
        EnableGamePlayInput();
    }

    void OnDisable()
    {
        DisableAllInput();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            jumpEvent?.Invoke();
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
           attackEvent?.Invoke();
        }
    }

    public void EnableGamePlayInput() => playerInput.GamePlay.Enable();

    public void DisableGamePlayInput() => playerInput.GamePlay.Disable();

    public void EnableUIInput() => playerInput.UI.Enable();

    public void DisableUIInput() => playerInput.UI.Disable();

    public void DisableAllInput()
    {
        playerInput.GamePlay.Disable();
        playerInput.UI.Disable();
    }

}
