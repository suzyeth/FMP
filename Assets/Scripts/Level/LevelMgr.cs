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

    public LevelData levelData;

    public void Init()
    {
        levelData = new LevelData();
        InitCharacter();
    }

    public void InitCharacter()
    {
        PublicTool.ClearChildItem(tfCharacter);
        GameObject objCharacter = GameObject.Instantiate(pfCharacter, tfCharacter);
        curCharacterView = objCharacter.GetComponent<CharacterViewItem>();
        curCharacterView.Init();
    }

    public void GenerateBox()
    {

    }
}
