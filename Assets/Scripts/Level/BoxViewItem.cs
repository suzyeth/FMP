using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxViewItem : UnitViewItem
{
    public override void Init()
    {
        base.Init();
    }

    public void Move(Vector2Int moveDir)
    {
        posID = posID + moveDir;
        RefreshPos();
    }
}
