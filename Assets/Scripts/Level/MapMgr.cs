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

    #region Event

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener("CharacterMove", CharacterMoveEvent);

    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener("CharacterMove", CharacterMoveEvent);

    }

    private void CharacterMoveEvent(object arg0)
    {
        Vector2Int dir = (Vector2Int)arg0;
        if (curCharacter != null)
        {
            curCharacter.Move(dir);
        }
    }

    #endregion

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
