using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDoorViewItem : TileViewItem
{
    public override void Init()
    {
        base.Init();
        DoorIsOpened = false;
    }

}
