using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScenceViewItem : TileViewItem
{
    public LevelMgr levelMgr;
    // Start is called before the first frame update
    

    public void ChangeScence()
    {
       
            levelMgr.ChangeMap();
        
        

        

    }
}
