using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class UnitViewItem : MonoBehaviour
{
    public Vector2Int initPosID;
    public Vector2Int posID;
    public bool IsDestroyed = false;
    public bool DoorIsOpened = false;

  





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

    

    /*public void inactivation()
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
        
        

    }*/

    


    public void OpenDoor()
    {
        DoorIsOpened = true;
        //UnityEngine.Debug.Log("OpenDoor");

    }

    public void CloseDoor()
    {
        DoorIsOpened = false;
        //UnityEngine.Debug.Log("CloseDoor");


    }

    
}
