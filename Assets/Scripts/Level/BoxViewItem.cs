using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoxViewItem : UnitViewItem
{
    public override void Init()
    {
        base.Init();
    }

    public void Move(Vector2Int moveDir)
    {
        posID = posID + moveDir;

        this.transform.DOMove(PublicTool.ConvertPosFromID(posID), 0.2f);
    }
}
