using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonViewItem : TileViewItem
{
    public UnityEvent onPress;
    public UnityEvent onRelease;
    public bool ButtonIsPressed = false;

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
            spriteRenderer.sprite = ButtonPressed;
            spriteRenderer.color = Color.red;
        }
        else
        {
            spriteRenderer.sprite = ButtonUnPressed;
            spriteRenderer.color = new Color(1,1,1,1);
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
