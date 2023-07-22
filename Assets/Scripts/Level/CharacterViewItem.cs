using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterViewItem : MonoBehaviour
{
    public Vector2Int characterPosID;

    public void Init()
    {
        RefreshPos();
    }

    public void RefreshPos()
    {
        this.transform.position = PublicTool.ConvertPosFromID(characterPosID);
    }
}
