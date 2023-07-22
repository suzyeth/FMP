using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMgr : MonoBehaviour
{
    public Transform tfBox;

    public void Init()
    {
        CheckAllBoxPos();
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
