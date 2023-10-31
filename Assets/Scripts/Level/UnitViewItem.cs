using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class UnitViewItem : MonoBehaviour
{
    public Vector2Int initPosID;
    public Vector2Int posID;
    public bool IsDestroyed = false;

    //public GameObject father;
    // public GameObject TilePrefab;
    public GameObject thisItem;



    public virtual void Init()
    {
        InitPosID();
        RefreshPosInstant();
    }

    /// <summary>
    /// Initial Position ID
    /// </summary>
    public void InitPosID()
    {
        posID = PublicTool.TranslatePosToPosID(this.transform.position);
        initPosID = posID;
    }

    /// <summary>
    /// Refresh Real Position According to Position ID Instantly
    /// </summary>
    public void RefreshPosInstant()
    {
        this.transform.position = PublicTool.ConvertPosFromID(posID);
    }

    /// <summary>
    /// Track Back to Initial Position
    /// </summary>
    public void ResetPos()
    {
        this.posID = initPosID;
        RefreshPosInstant();
    }

    public void MoveToTarPos(Vector2Int tarPos)
    {
        posID = tarPos;
        RefreshPosInstant();
    }

    public void RegenerateTile(Vector2Int posID)
    {
       
        //GameObject NewTile = Instantiate(TilePrefab, PublicTool.ConvertPosFromID(posID), quaternion.identity);
        //NewTile.transform.parent = father.transform;

    }

    public void inactivation()
    {
        if (thisItem.activeSelf == false)
        {
            thisItem.SetActive(true);
        }
        else
        {
            thisItem.SetActive(false);
            UnityEngine.Debug.Log("SetActiveFalse");
        }
        
        

    }
}
