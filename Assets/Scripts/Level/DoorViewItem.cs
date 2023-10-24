using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorViewItem : TileViewItem
{
    
    #region spriteVariables
    public Sprite DoorOpen;
    public Sprite DoorClose;
    SpriteRenderer spriteRenderer;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoorIsOpen()
    {
        Debug.Log("DoorIsOpen" + keyID);
        spriteRenderer.sprite = DoorOpen;
    }

    public void DoorIsClose()
    {
        Debug.Log("DoorIsClose " + keyID);
        spriteRenderer.sprite = DoorClose;

    }


}
