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

    private InputAction EscAction;
    private InputAction ResetAction;
    private InputAction RestartGameAction;
    private InputAction SkipAnimationAction;



    public Vector2 moveVector;


    private bool isInitInput = false;
    private bool isPressWASD = false;

    private float currentCooldown=0.2f;
    private float moveCooldown=0.2f;

    private bool isHoldingS = false;
    private bool isHoldingW = false;
    private bool isHoldingA = false;
    private bool isHoldingD = false;
    

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
            EscAction = playerInput.Gameplay.Esc;
            ResetAction = playerInput.Gameplay.Reset;
            SkipAnimationAction = playerInput.Gameplay.SkipAnimation; 

            RestartGameAction = playerInput.Gameplay.RestartGame;
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
        UpAction.canceled += Up_canceled;
        DownAction.started += Down_started;
        DownAction.canceled += Down_canceled;
        LeftAction.started += Left_started;
        LeftAction.canceled += Left_canceled;
        RightAction.started += Right_started;
        RightAction.canceled += Right_canceled;
        touchAction.performed += Touch_performed;
        undoAction.performed += Undo_performed;
        //SkillAction1.performed += IceBreakingSkill1_performed;
        //SkillAction12.performed += ThroughSpikesSkill2_performed;
        SkillAction13.performed += PullBoxSkill3_performed;
        SkillAction14.performed += TeleportationSkill4_performed;
        EscAction.performed += Esc_performed;
        ResetAction.performed += Reset_performed;

        SkipAnimationAction.performed += SkipAnimation_performed;
        RestartGameAction.performed += RestartGame_performed;


    }

    private void DisableInput()
    {
        UpAction.started -= Up_started;
        DownAction.started -= Down_started;
        LeftAction.started -= Left_started;
        RightAction.started -= Right_started;
        touchAction.performed -= Touch_performed;
        undoAction.performed -= Undo_performed;

       // SkillAction1.performed -= IceBreakingSkill1_performed;
        //SkillAction12.performed -= ThroughSpikesSkill2_performed;
        SkillAction13.performed -= PullBoxSkill3_performed;
        SkillAction14.performed -= TeleportationSkill4_performed;
        EscAction.performed -= Esc_performed;
        ResetAction.performed -= Reset_performed;

        SkipAnimationAction.performed -= SkipAnimation_performed;
        RestartGameAction.performed -= RestartGame_performed;
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

    private void Esc_performed(InputAction.CallbackContext obj)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

    }

    private void RestartGame_performed(InputAction.CallbackContext obj)
    {
        
        //RestartGame
        GameMgr.Instance.levelMgr.ChangLevel(0);

    }
    private void Reset_performed(InputAction.CallbackContext obj)
    {
        PublicTool.GetGameData().LoadLevelData();
        GameMgr.Instance.levelMgr.RestartThisMap();

    }

    private void SkipAnimation_performed(InputAction.CallbackContext obj)
    {
        //int id = GameMgr.Instance.levelMgr.CurrentMapID();
        //if(id==0)
        //{
        //    VideoPlayerController.Instance.SkipStartVideo();
        //}
        //if (id == 24)
        //{
          //  VideoPlayerController.Instance.SkipEndVideo();

        //}

    }




    #region WASD

    private void Up_started(InputAction.CallbackContext obj)
    {
     isHoldingW = true;
        
    }

    private void Up_canceled(InputAction.CallbackContext obj)
    {
        isHoldingW = false;

        //EventCenter.Instance.EventTrigger("CheckWhetherStopPressing", new Vector2Int(0, 1));
    }

    
    private void Down_started(InputAction.CallbackContext obj)
    {
        isHoldingS = true;
        /*if (!isPressWASD)
        {
            EventCenter.Instance.EventTrigger("CharacterMove", new Vector2Int(0, -1));
        }*/
    }

    private void Down_canceled(InputAction.CallbackContext obj)
    {
        isHoldingS = false;
        //EventCenter.Instance.EventTrigger("CheckWhetherStopPressing", new Vector2Int(0, -1));

    }

    private void Left_started(InputAction.CallbackContext obj)
    {
       isHoldingA = true;
    /*if (!isPressWASD)
    {
        EventCenter.Instance.EventTrigger("CharacterMove", new Vector2Int(-1, 0));
    }*/
    }
    private void Left_canceled(InputAction.CallbackContext obj)
    {
        isHoldingA = false;
        //EventCenter.Instance.EventTrigger("CheckWhetherStopPressing", new Vector2Int(-1, 0));

    }

    private void Right_started(InputAction.CallbackContext obj)
    {
        isHoldingD = true;
        /*if (!isPressWASD)
        {
            EventCenter.Instance.EventTrigger("CharacterMove", new Vector2Int(1, 0));
        }*/
    }

    private void Right_canceled(InputAction.CallbackContext obj)
    {
        isHoldingD = false;
       // EventCenter.Instance.EventTrigger("CheckWhetherStopPressing", new Vector2Int(1, 0));

    }

    #endregion
    int OldmoveDir = -2;

    private void Update()
    {
        

       if (isInitInput)
        {
            int moveDir = -1;
            //W-1,A-2,S-3,D-4,Release All Button-0
            if (isHoldingW && currentCooldown <= 0f)
            {
                EventCenter.Instance.EventTrigger("CharacterMove", new Vector2Int(0, 1));
                currentCooldown = moveCooldown;
                moveDir = 1;
            }
            else if (isHoldingS && currentCooldown <= 0f)
            {
                EventCenter.Instance.EventTrigger("CharacterMove", new Vector2Int(0, -1));
                currentCooldown = moveCooldown;
                moveDir = 2;
            }
            else if (isHoldingA && currentCooldown <= 0f)
            {
                EventCenter.Instance.EventTrigger("CharacterMove", new Vector2Int(-1, 0));
                currentCooldown = moveCooldown;
                moveDir = 3;
            }
            else if (isHoldingD && currentCooldown <= 0f)
            {
                EventCenter.Instance.EventTrigger("CharacterMove", new Vector2Int(1, 0));
                currentCooldown = moveCooldown;
                moveDir = 4;
            }
            else if (!isHoldingA && !isHoldingW && !isHoldingS && !isHoldingD)
           
            {
                
               
                    EventCenter.Instance.EventTrigger("StopCharacterMove", new Vector2Int(1, 0));
                moveDir = 0;

            }
            //checkButtonState(moveDir);
                currentCooldown -= Time.deltaTime;
            
            
            
            
            /*if (UpAction.IsPressed() || DownAction.IsPressed() || LeftAction.IsPressed() || RightAction.IsPressed())
            {
                isPressWASD = true;
            }
            else
            {
                isPressWASD = false;
            }*/

        }
       
    }

    public void checkButtonState(int moveDir)
    {
        if (OldmoveDir != moveDir)
        {
            if (moveDir == 1)
            { 
            
            }
            OldmoveDir = moveDir;
        }
    }

}
