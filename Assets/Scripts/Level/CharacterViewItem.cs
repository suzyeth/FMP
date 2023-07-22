using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CharacterViewItem : UnitViewItem
{
    public override void Init()
    {
        base.Init();
    }


    #region Move

    public void Move(Vector2Int moveDir)
    {
        posID = posID + moveDir;
        this.transform.DOMove(PublicTool.ConvertPosFromID(posID), 0.2f);
    }
    #endregion
}
