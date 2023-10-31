using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class InputMgr : MonoSingleton<InputMgr>
{
    private PlayerInput playerInput;

    private InputAction UpAction;
    private InputAction DownAction;
    private InputAction LeftAction;
    private InputAction RightAction;

    private InputAction touchAction;
    private InputAction touchPositionAction;

    private InputAction undoAction;

    public Vector2 moveVector;


    private bool isInitInput = false;
    private bool isPressWASD = false;

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
            UpAction = playerInput.Gameplay.Up;
            DownAction = playerInput.Gameplay.Down;
            LeftAction = playerInput.Gameplay.Left;
            RightAction = playerInput.Gameplay.Right;

            touchAction = playerInput.Gameplay.Touch;
            touchPositionAction = playerInput.Gameplay.TouchPosition;

            undoAction = playerInput.Gameplay.Undo;

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
        UpAction.started += Up_started;
        DownAction.started += Down_started;
        LeftAction.started += Left_started;
        RightAction.started += Right_started;
        touchAction.performed += Touch_performed;
        undoAction.performed += Undo_performed;
    }

    private void DisableInput()
    {
        UpAction.started -= Up_started;
        DownAction.started -= Down_started;
        LeftAction.started -= Left_started;
        RightAction.started -= Right_started;
        touchAction.performed -= Touch_performed;
        undoAction.performed -= Undo_performed;

        playerInput.Disable();
    }


/*    private void Move_performed(InputAction.CallbackContext obj)
    {
        Vector2 valueMove = obj.ReadValue<Vector2>();
        moveVector = valueMove;
    }
*/
    private void Touch_performed(InputAction.CallbackContext obj)
    {
        Vector2 screenPosition = touchPositionAction.ReadValue<Vector2>();

        Debug.Log("Click "+ screenPosition);
    }

    private void Undo_performed(InputAction.CallbackContext obj)
    {
        if (PublicTool.GetGameData() != null)
        {
            PublicTool.GetGameData().UndoAction();



            Debug.Log("Invoke Undo");
        }

    }

    #region WASD

    private void Up_started(InputAction.CallbackContext obj)
    {
        if (!isPressWASD)
        {
            EventCenter.Instance.EventTrigger("CharacterMove", new Vector2Int(0, 1));
        }
    }

    private void Down_started(InputAction.CallbackContext obj)
    {
        if (!isPressWASD)
        {
            EventCenter.Instance.EventTrigger("CharacterMove", new Vector2Int(0, -1));
        }
    }

    private void Left_started(InputAction.CallbackContext obj)
    {
        if (!isPressWASD)
        {
            EventCenter.Instance.EventTrigger("CharacterMove", new Vector2Int(-1, 0));
        }
    }

    private void Right_started(InputAction.CallbackContext obj)
    {
        if (!isPressWASD)
        {
            EventCenter.Instance.EventTrigger("CharacterMove", new Vector2Int(1, 0));
        }
    }

    #endregion


    private void Update()
    {
        if (isInitInput)
        {
            if (UpAction.IsPressed() || DownAction.IsPressed() || LeftAction.IsPressed() || RightAction.IsPressed())
            {
                isPressWASD = true;
            }
            else
            {
                isPressWASD = false;
            }
        }
    }
}
