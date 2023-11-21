using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class ButtonViewItem : TileViewItem
{
    public UnityEvent onPress;
    public UnityEvent onRelease;
    public bool ButtonIsPressed = false;

    public List<GameObject> Corridors = new List<GameObject>();




    void Start()
    {

       
    }
    void Update()
    {

        if (ButtonIsPressed)
        {
            
            ChangeGraphic(1);
            
        }
        else
        {
           
            ChangeGraphic(0);
            
        }

    }

    public void ChangeGraphic(int index)
    {
        //index = index += direction;
        //index = Math.Clamp(index, 0, Corridors.Count - 1);


        for (int i = 0; i < Corridors.Count; i++)
        {
            Corridors[i].SetActive(i == index);
        }
    }

    public void OnPress()
    {
        //Debug.Log("Pressed button " + keyID);
        ButtonIsPressed = true;
        onPress.Invoke();
    }
    public void OnRelease()
    {
        //Debug.Log("Released button " + keyID);
        ButtonIsPressed = false;
        onRelease.Invoke();
    }

    public void PlayerOnStartingLeftButton()
    {
        EventCenter.Instance.EventTrigger("PlayerOnButton", 1);
    }
    public void PlayerExitStartingLeftButton()
    {
        EventCenter.Instance.EventTrigger("PlayerOnButton", -1);
    }

    public void PlayerOnStartingRightButton()
    {
        EventCenter.Instance.EventTrigger("PlayerOnButton", 2);
    }
    public void PlayerExitStartingRightButton()
    {
        EventCenter.Instance.EventTrigger("PlayerOnButton", -2);
    }

    public void PlayerOnSettingLeftButton()
    {
        EventCenter.Instance.EventTrigger("PlayerOnButton", 3);
    }
    public void PlayerExitSettingLeftButton()
    {
        EventCenter.Instance.EventTrigger("PlayerOnButton", -3);
    }
    public void PlayerOnSettingRightButton()
    {
        EventCenter.Instance.EventTrigger("PlayerOnButton", 4);
    }
    public void PlayerExitSettingRightButton()
    {
        EventCenter.Instance.EventTrigger("PlayerOnButton", -4);
    }
}