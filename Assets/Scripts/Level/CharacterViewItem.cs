using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        RefreshPos();
    }
    #endregion
}
