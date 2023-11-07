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



    #region spriteVariables
    public Sprite ButtonUnPressed;
    public Sprite ButtonPressed;
    SpriteRenderer spriteRenderer;
    #endregion

    void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {

        if (ButtonIsPressed)
        {
            //spriteRenderer.sprite = iceCracked;
            ChangeGraphic(1);
            spriteRenderer.sprite = ButtonPressed;
            spriteRenderer.color = Color.red;
        }
        else
        {
            //spriteRenderer.sprite = iceUnCracked;
            ChangeGraphic(0);
            spriteRenderer.sprite = ButtonUnPressed;
            spriteRenderer.color = new Color(1, 1, 1, 1);
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