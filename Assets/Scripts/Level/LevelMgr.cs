using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMgr : MonoBehaviour
{
    [Header("Map")]
    public Transform tfMap;

    [Header("Character")]
    public Transform tfCharacter;
    public GameObject pfCharacter;

    private CharacterViewItem curCharacterView;
    private MapMgr curMap;

    public void Init()
    {
        InitCharacter();
        GenerateMap(2);
    }

    public void InitCharacter()
    {
        PublicTool.ClearChildItem(tfCharacter);
        GameObject objCharacter = GameObject.Instantiate(pfCharacter, tfCharacter);
        curCharacterView = objCharacter.GetComponent<CharacterViewItem>();
        curCharacterView.Init();
    }

    public void GenerateMap(int id)
    {
        PublicTool.ClearChildItem(tfMap);

        Object objMap = Resources.Load("Map/Map"+ id);
        GameObject gobjMap = Instantiate(objMap, tfMap) as GameObject;
        Debug.Log("Map/Map01");
    }
}
