using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Playables;
using UnityEngine.Experimental.GlobalIllumination;


public class CharacterViewItem : UnitViewItem
{
    public Animator animator;
    public bool WhetherMove;
   

    public override void Init()
    {
        base.Init();
        animator = GetComponent<Animator>();
    }


    #region Move

    private Vector2Int RightMove = new Vector2Int(1, 0);
    private Vector2Int LeftMove = new Vector2Int(-1, 0);
    private Vector2Int UpMove = new Vector2Int(0, 1);
    private Vector2Int DownMove = new Vector2Int(0, -1);

    private Vector2Int currentMoveDir= new Vector2Int(0, 0);
    private Vector2Int oldMoveDir = new Vector2Int(0, 0);
    private Vector2Int stopMoveDir = new Vector2Int(0, 0);

    public void Move(Vector2Int moveDir)
    {
        posID = posID + moveDir;
        this.transform.DOMove(PublicTool.ConvertPosFromID(posID), 0.2f);
        if (oldMoveDir != moveDir)
        {
            currentMoveDir = moveDir;
            PlayMoveAnimation(moveDir);

        }
       oldMoveDir = moveDir;
        
 
    }




    #endregion

    #region Event

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener("StopCharacterMove", StopCharacterMoveEvent);
        
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener("StopCharacterMove", StopCharacterMoveEvent);
   
    }

    
    public void StopCharacterMoveEvent(object arg0)
    {
        if (stopMoveDir != currentMoveDir)
        {
            PlayIdleAnimation(currentMoveDir);
        }
        else
        { 
        
        }
        stopMoveDir=currentMoveDir;



    }
    #endregion



  
      

        private void PlayMoveAnimation(Vector2Int moveDir)
        {

        animator.ResetTrigger("D");
        animator.ResetTrigger("W");
        animator.ResetTrigger("A");
        animator.ResetTrigger("S");

        if (currentMoveDir == RightMove)
        {
            animator.SetTrigger("RightMove");
           animator.ResetTrigger("DownMove");
            animator.ResetTrigger("LeftMove");
            animator.ResetTrigger("UpMove");

            Debug.Log("RightMove");

        }
        else if (currentMoveDir == LeftMove)
        {
            animator.SetTrigger("LeftMove");  
            
            animator.ResetTrigger("DownMove");
            animator.ResetTrigger("RightMove");            
            animator.ResetTrigger("UpMove");

            Debug.Log("LeftMove");

        }
        else if (currentMoveDir == UpMove)
        {
            animator.SetTrigger("UpMove");

            animator.ResetTrigger("DownMove");
            animator.ResetTrigger("RightMove");
            animator.ResetTrigger("LeftMove");
  
           // Debug.Log("UpMove");
        }
        else
        {
            animator.SetTrigger("DownMove");

            animator.ResetTrigger("RightMove");
            animator.ResetTrigger("LeftMove");
            animator.ResetTrigger("UpMove");

           // Debug.Log("DownMove");

        }

    }

        private void PlayIdleAnimation(Vector2Int moveDir)
        {
        //Debug.Log(moveDir);
        animator.ResetTrigger("D");
        animator.ResetTrigger("W");
        animator.ResetTrigger("A");
        animator.ResetTrigger("S");

        animator.ResetTrigger("DownMove");
        animator.ResetTrigger("RightMove");
        animator.ResetTrigger("LeftMove");
        animator.ResetTrigger("UpMove");

        if (moveDir == RightMove)
        {
            animator.SetTrigger("D");
           // animator.ResetTrigger("RightMove", false);
           // Debug.Log("StopRight");
        }

        if (moveDir == UpMove)
        {
            animator.SetTrigger("W");
           // animator.ResetTrigger("UpMove", false);           
          //  Debug.Log("StopUp");

        }
        if (moveDir == LeftMove)
        {
            animator.SetTrigger("A");
           // animator.ResetTrigger("LeftMove");
        }
        if (moveDir == DownMove)
        {
            animator.SetTrigger("S");
           // animator.ResetTrigger("DownMove", false);            
            //Debug.Log("StopDown");

        }
    }

    private void PlayDianAnAnimation(Vector2Int moveDir)
    {
        //Debug.Log(moveDir);
        animator.ResetTrigger("D");
        animator.ResetTrigger("W");
        animator.ResetTrigger("A");
        animator.ResetTrigger("S");

        animator.ResetTrigger("DownMove");
        animator.ResetTrigger("RightMove");
        animator.ResetTrigger("LeftMove");
        animator.ResetTrigger("UpMove");

        if (moveDir == RightMove)
        {
            animator.SetTrigger("D");
            // animator.ResetTrigger("RightMove", false);
            // Debug.Log("StopRight");
        }

        if (moveDir == UpMove)
        {
            animator.SetTrigger("W");
            // animator.ResetTrigger("UpMove", false);           
            //  Debug.Log("StopUp");

        }
        if (moveDir == LeftMove)
        {
            animator.SetTrigger("diananA");
            // animator.ResetTrigger("LeftMove");
        }
        if (moveDir == DownMove)
        {
            animator.SetTrigger("S");
            // animator.ResetTrigger("DownMove", false);            
            //Debug.Log("StopDown");

        }
    }




}
