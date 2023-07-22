using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class InputMgr : MonoSingleton<InputMgr>
{
    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction touchAction;
    private InputAction touchPositionAction;
    public Vector2 moveVector;


    private bool isInitInput = false;

    public IEnumerator IE_Init()
    {
        InitInput();
        yield break;
    }

    private void InitInput()
    {
        if (!isInitInput)
        {
            playerInput = new PlayerInput();
            moveAction = playerInput.Gameplay.Move;
            touchAction = playerInput.Gameplay.Touch;
            touchPositionAction = playerInput.Gameplay.TouchPosition;
            isInitInput = true;
        }
    }

    private void OnEnable()
    {
        EnableInput();
    }

    private void OnDisable()
    {
        DisableInput();
    }

    private void EnableInput()
    {
        if (playerInput == null)
        {
            InitInput();
        }

        playerInput.Enable();
        moveAction.performed += Move_performed;
        touchAction.performed += Touch_performed;
    }



    private void DisableInput()
    {
        moveAction.performed -= Move_performed;
        touchAction.performed -= Touch_performed;
        playerInput.Disable();
    }


    private void Move_performed(InputAction.CallbackContext obj)
    {
        Vector2 valueMove = obj.ReadValue<Vector2>();
        moveVector = valueMove;
    }

    private void Touch_performed(InputAction.CallbackContext obj)
    {
        Vector2 screenPosition = touchPositionAction.ReadValue<Vector2>();

        Debug.Log("Click "+ screenPosition);
    }
}
