using System.Collections;
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
            //spriteRenderer.sprite = iceCracked;
            ChangeGraphic(1);
        }
        else
        {
            //spriteRenderer.sprite = iceUnCracked;
            ChangeGraphic(0);
        }

    }

    public void ChangeGraphic(int index)
    {
        //index = index += direction;
        index = Math.Clamp(index, 0, Corridors.Count - 1);


        for (int i = 0; i < Corridors.Count; i++)
        {
            Corridors[i].SetActive(i == index);
        }
    }
   
    public void OnPress()
    {
        Debug.Log("Pressed button " + keyID);
        ButtonIsPressed = true;
        onPress.Invoke();
    }

    public void OnRelease()
    {
        Debug.Log("Released button " + keyID);
        ButtonIsPressed = false;
        onRelease.Invoke();
    }
}
