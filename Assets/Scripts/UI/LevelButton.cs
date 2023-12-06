using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public int id;

    public void changeLevel()
    {
        
        GameMgr.Instance.levelMgr.ChangLevel(id);
       
    }
}
