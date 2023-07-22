using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterViewItem : MonoBehaviour
{
    public Vector2Int initPosID;
    public Vector2Int posID;


    #region Init
    public void Init()
    {
        InitPosID();
        RefreshPos();
    }

    /// <summary>
    /// Initial Position ID
    /// </summary>
    public void InitPosID()
    {
        posID = PublicTool.TranslatePosToPosID(this.transform.position);
        initPosID = posID;
    }

    #endregion




    #region Position

    /// <summary>
    /// Refresh Real Position According to Position ID
    /// </summary>
    public void RefreshPos()
    {
        this.transform.position = PublicTool.ConvertPosFromID(posID);
    }

    /// <summary>
    /// Track Back to Initial Position
    /// </summary>
    public void ResetPos()
    {
        this.posID = initPosID;
        RefreshPos();
    }
    #endregion

    #region Move

    public void Move(Vector2Int moveDir)
    {
        posID = posID + moveDir;
        RefreshPos();
    }
    #endregion
}
