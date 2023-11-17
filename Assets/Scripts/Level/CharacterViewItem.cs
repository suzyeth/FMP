using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CharacterViewItem : UnitViewItem
{
    public Animator animator;
   

    public override void Init()
    {
        base.Init();
        animator = GetComponent<Animator>();
    }


    #region Move

    private Vector2Int RightMove = new Vector2Int(1, 0);
    private Vector2Int LeftMove = new Vector2Int(-1, 0);
    private Vector2Int UpMove = new Vector2Int(0, 1);
    

    public void Move(Vector2Int moveDir)
    {
        posID = posID + moveDir;
        this.transform.DOMove(PublicTool.ConvertPosFromID(posID), 0.2f);


        if (moveDir == RightMove)
        {
            animator.SetBool("RightMove", true);
            animator.SetBool("LeftMove", false);
            animator.SetBool("UpMove", false);
            animator.SetBool("DownMove", false);
            
        }
        else if (moveDir == LeftMove)
        {
            animator.SetBool("LeftMove", true);

            animator.SetBool("RightMove", false);
            animator.SetBool("UpMove", false);
            animator.SetBool("DownMove", false);
            
        }
        else if (moveDir == UpMove)
        {
            animator.SetBool("UpMove", true);


            animator.SetBool("RightMove", false);
            animator.SetBool("LeftMove", false);
            animator.SetBool("DownMove", false);
        }
        else
        {
            animator.SetBool("DownMove", true);

            animator.SetBool("RightMove", false);
            animator.SetBool("LeftMove", false);
            animator.SetBool("UpMove", false);
        }

    }

    


    #endregion
}
