using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMgr : MonoBehaviour
{
    public Transform tfBox;
    public Transform tfCharacter;
    private CharacterViewItem curCharacter;

    public void Init()
    {
        CheckAllCharacter();
        CheckAllBoxPos();
    }

    public void CheckAllCharacter()
    {
        foreach (Transform child in tfCharacter)
        {
            curCharacter = child.GetComponent<CharacterViewItem>();
            curCharacter.Init();
        }
    }

    public void CheckAllBoxPos()
    {
        foreach (Transform child in tfBox)
        {
            BoxViewItem itemBox = child.GetComponent<BoxViewItem>();
            itemBox.Init();
        }
    }
}
