using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScenceViewItem : TileViewItem
{


    public void ChangeScence()
    {

        GameMgr.Instance.levelMgr.ChangeMap();
      
    }
}
