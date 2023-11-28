using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RedDoorViewItem : TileViewItem
{
    public List<GameObject> Corridors = new List<GameObject>();
    private bool playSound=false;
    public override void Init()
    {
        base.Init();
        DoorIsOpened = false;
    }

    void Update()
    {

        if (DoorIsOpened)
        {
            //spriteRenderer.sprite = iceCracked;
            ChangeGraphic(1);
            if (!playSound)
            { 
                AudioManager.Instance.OpenDoor();
                playSound = true;
            }
        }
        else
        {
            //spriteRenderer.sprite = iceUnCracked;
            ChangeGraphic(0);
            playSound = false;
        }

    }

    public void ChangeGraphic(int index)
    {
        //index = index += direction;
        //index = Math.Clamp(index, 0, Corridors.Count - 1);
        //Debug.Log("index" + index);

        for (int i = 0; i < Corridors.Count; i++)
        {
            Corridors[i].SetActive(i == index);
        }
    }

}
