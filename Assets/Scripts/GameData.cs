using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


public class ActionRecordData
{
    //Define that keyID=-2 mean character
    public int keyID;
    public Vector2Int startPos;
    public Vector2Int endPos;

    public ActionRecordData(int keyID, Vector2Int startPos, Vector2Int endPos)
    {
        this.keyID = keyID;
        this.startPos = startPos;
        this.endPos = endPos;
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
        Debug.Log(count);
        
    }
    #endregion

    #region Undo

    private Stack<List<ActionRecordData>> stackActionRecord = new Stack<List<ActionRecordData>>();

    public void AddRecordAction(List<ActionRecordData> listAction)
    {
        stackActionRecord.Push(listAction);
    }

    public void UndoAction()
    {
        if (stackActionRecord.Count > 0)
        {
            List<ActionRecordData> listUndoAction = stackActionRecord.Pop();


            if (listUndoAction != null)
            {
                foreach (var action in listUndoAction)
                {
                    EventCenter.Instance.EventTrigger("Undo", action);
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