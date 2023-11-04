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

    private InputAction SkillAction1;
    private InputAction SkillAction12;
    private InputAction SkillAction13;
    private InputAction SkillAction14;

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
            SkillAction1= playerInput.Gameplay.IceBreakingSkill1;
            SkillAction12 = playerInput.Gameplay.ThroughSpikesSkill2;
            SkillAction13 = playerInput.Gameplay.PullBoxSkill3;
            SkillAction14 = playerInput.Gameplay.TeleportationSkill4;


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
        SkillAction1.performed += IceBreakingSkill1_performed;
        SkillAction12.performed += ThroughSpikesSkill2_performed;
        SkillAction13.performed += PullBoxSkill3_performed;
        SkillAction14.performed += TeleportationSkill4_performed;
     

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
        else
        {
            Debug.LogWarning("GameData is null");
        }

    }

   #region skills
    private void IceBreakingSkill1_performed(InputAction.CallbackContext obj)
    {
        

        EventCenter.Instance.EventTrigger("IceBreakingSkill1", 1);


    }

    private void ThroughSpikesSkill2_performed(InputAction.CallbackContext obj)
    {
        EventCenter.Instance.EventTrigger("ThroughSpikesSkill2", 1);

    }

    
    private void PullBoxSkill3_performed(InputAction.CallbackContext obj)
    {
        EventCenter.Instance.EventTrigger("PullBoxSkill3", 1);

    }

    private void TeleportationSkill4_performed(InputAction.CallbackContext obj)
    {
        EventCenter.Instance.EventTrigger("TeleportationSkill4",1);

    }


    #endregion



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
