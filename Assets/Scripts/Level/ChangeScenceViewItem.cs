using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScenceViewItem : TileViewItem
{


    public void ChangeScence()
    {
        int id = GameMgr.Instance.levelMgr.CurrentMapID();
        if (id == 24)
        {
            VideoPlayerController.Instance.PlayendingVdeo();
        }
        else
        {
            GameMgr.Instance.levelMgr.ChangeMap();
        }
        
        

    }
}
