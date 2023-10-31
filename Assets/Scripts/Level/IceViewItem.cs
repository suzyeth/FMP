using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceViewItem : TileViewItem

{
    public bool iceIsCracked=false;
    public bool iceIsDestroyed = false;
    #region spriteVariables
    public Sprite iceUnCracked;
    public Sprite iceCracked;
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

        if (iceIsCracked)
        {
            spriteRenderer.sprite = iceCracked;
        }
        else 
        {
            spriteRenderer.sprite = iceUnCracked;
        }

    }
}
