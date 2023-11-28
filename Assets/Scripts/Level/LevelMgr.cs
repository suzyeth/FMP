using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMgr : MonoBehaviour
{
    [Header("Map")]
    public Transform tfMap;
    public GameObject tfmap;

    private MapMgr curMap;
    public  int CurMapID=0 ;
    private int id = 0;
    

    #region Init
    public void Init()
    {
        StartLevel(CurMapID);
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

    public void ChangLevel(int id)
    {
        GenerateMap(id);
        EventCenter.Instance.EventTrigger("ChangeLevelText", 1);
    }

    

}
