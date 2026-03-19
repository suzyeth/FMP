using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LevelMgr : MonoBehaviour
{
    [Header("Map")]
    public Transform tfMap;
   

    private MapMgr curMap;
    public  int CurMapID=0 ;
    private int id = 0;
    public GameData gameData;

    #region Init
    public void Init()
    {
        StartLevel(CurMapID);
        gameData = new GameData();
    }

/*    public void InitCharacter()
    {
        PublicTool.ClearChildItem(tfCharacter);
        GameObject objCharacter = GameObject.Instantiate(pfCharacter, tfCharacter);
        curCharacterView = objCharacter.GetComponent<CharacterViewItem>();
        curCharacterView.Init();
    }*/
    #endregion

    #region StartLevel

    public void StartLevel(int id)
    {
        GenerateMap(id);
    }


    public void GenerateMap(int id)
    {
        PublicTool.ClearChildItem(tfMap);
       

        Object objMap = Resources.Load("Map/Map"+ id);
        GameObject gobjMap = Instantiate(objMap, tfMap) as GameObject;
        Debug.Log("Map/Map" + id);
        curMap = gobjMap.GetComponent<MapMgr>();
        curMap.Init(id);
    }

   



    #endregion

    #region ChangeMap
    public void ChangeMap()
    {
        curMap.ClearDataChangScence();
        gameData.ClearUndoStack();
         id++;
        Debug.Log("id" + id);
        GenerateMap(id);

       EventCenter.Instance.EventTrigger("ChangeLevelText", 1);
    }

    

    #endregion

    #region RestartThisMap
    public void RestartThisMap()
    {
        curMap.ClearDataChangScence();
        
        Debug.Log("id" + id);
        GenerateMap(id);


    }


    #endregion

    public int CurrentMapID()
    {
        return id;
    }

    public void ChangLevel(int Lecvelid)
    {
        
        curMap.ClearDataChangScence();
        gameData.ClearUndoStack();

        id = Lecvelid;
        Debug.Log("id" + id);
        GenerateMap(Lecvelid);
       EventCenter.Instance.EventTrigger("ChangeLevelText", 1);
    }

    

}
