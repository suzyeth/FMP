using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class BaseRecordData
{
    public int keyID;
}

public class ActionRecordData : BaseRecordData
{
    //Define that keyID=-2 mean character
    public Vector2Int startPos;
    public Vector2Int endPos;
    

    public ActionRecordData(int keyID, Vector2Int startPos, Vector2Int endPos)
    {

        this.keyID = keyID;
        this.startPos = startPos;
        this.endPos = endPos;
        
    }


}

public class DestoryStateRecordData : BaseRecordData
{
    //Define that keyID=-2 mean character
    public Vector2Int PosID;
    public TileType tileType;


    public DestoryStateRecordData(int keyID, Vector2Int PosID, TileType tileType)
    {

        this.keyID = keyID;
        this.PosID = PosID;
        this.tileType=tileType;
        

    }
}

public class GameData
{
    //Init game data
    public GameData()
    {
        energy = 0;
        dicCrystal.Clear();
    }

    #region data destroyedTiles
    public Dictionary<int, bool> destroyedTiles = new Dictionary<int, bool>();

    public void RecordDestroyedTile(int keyID)
    {
        destroyedTiles[keyID] = true;
    }

    public bool IsTileDestroyed(int keyID)
    {
        return destroyedTiles.ContainsKey(keyID) && destroyedTiles[keyID];
    }

    #endregion


    #region Crystal
    public int energy = 0;
    public Dictionary<int, Crystal> dicCrystal = new Dictionary<int, Crystal>();
    public void AddCrystal(int id)
    {
        if (!dicCrystal.ContainsKey(id))
        {
            Crystal crystal = new Crystal(id);
            dicCrystal.Add(id, crystal);

        }
    }

    public Crystal GetCrystal(int id)
    {
        if (dicCrystal.ContainsKey(id))
        {
            return dicCrystal[id]; 
        }
        else
        {
            return null;
        }
    }

    //Get the number of active crystal
    public int GetNumActiveCrystal()
    {
        //return dicCrystal.Count;
        int count = 0;
        foreach(var item in dicCrystal)
        {
            if (!item.Value.isSubmitted)
            {
                count++;
            }
        }
        return count;
        
        
    }
    #endregion

    #region Undo

    private Stack<List<BaseRecordData>> stackActionRecord = new Stack<List<BaseRecordData>>();

    public void AddRecordAction(List<BaseRecordData> listAction)
    {
        stackActionRecord.Push(listAction);
    }

    public void UndoAction()
    {
        if (stackActionRecord.Count > 0)
        {
            List<BaseRecordData> listUndoAction = stackActionRecord.Pop();


            if (listUndoAction != null)
            {
                foreach (var action in listUndoAction)
                {
                    if (action.GetType() == typeof(DestoryStateRecordData))
                    {
                        EventCenter.Instance.EventTrigger("UndoDestroy", action);
                        
                    }
                    else if(action.GetType() == typeof(ActionRecordData))
                    {
                        EventCenter.Instance.EventTrigger("Undo", action);
                    }
                    

                }
            }
        }
    }

    #endregion

}

public class Crystal
{
    public int keyID = 0;
    public int energy = 0;
    public bool isSubmitted = false;

    public Crystal(int keyID)
    {
        this.keyID = keyID;
        energy = 0;
        isSubmitted = false;
    }
}