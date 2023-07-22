using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxViewItem : MonoBehaviour
{
    public Vector2Int posID;


    public void Init()
    {
        InitPosID();
        ResetPos();
    }

    public void InitPosID()
    {
        posID = PublicTool.TranslatePosToPosID(this.transform.position);
    }

    public void ResetPos()
    {
        this.transform.position = PublicTool.ConvertPosFromID(posID);
    }
}
