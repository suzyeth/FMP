using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMgr : MonoBehaviour
{
    [Header("Map")]
    public Transform tfMap;

    private MapMgr curMap;

    #region Init
    public void Init()
    {
        StartLevel(3);
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
        Debug.Log("Map/Map01");
        curMap = gobjMap.GetComponent<MapMgr>();
        curMap.Init();
    }



    #endregion
}
