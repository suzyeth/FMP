using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapsViemItem : TileViewItem

{
    #region spriteVariables
    public Sprite TrapsUnfilled;
    public Sprite TrapsFilled;
    
    
    SpriteRenderer spriteRenderer;
    #endregion
    public bool TrapsIsFilled = false;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (TrapsIsFilled)
        {
            spriteRenderer.sprite = TrapsFilled;
        }
        else if (!TrapsIsFilled)
        {
            spriteRenderer.sprite = TrapsUnfilled;
        }

    }
}
