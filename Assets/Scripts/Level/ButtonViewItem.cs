using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonViewItem : TileViewItem
{
    public UnityEvent onPress;
    public UnityEvent onRelease;
    public bool ButtonIsPressed = false;

    public void OnPress()
    {
        Debug.Log("Pressed button " + keyID);
        onPress.Invoke();
    }

    public void OnRelease()
    {
        Debug.Log("Released button " + keyID);
        onRelease.Invoke();
    }
}
