using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IceViewItem : TileViewItem

{
    public bool iceIsCracked = false;

    public List<GameObject> Corridors = new List<GameObject>();

    private bool playSound = false;



    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        if (iceIsCracked)
        {

            ChangeGraphic(1);
            if (!playSound)
            {
                AudioManager.Instance.IceCrack();
                playSound = true;
            }

        }
        else
        {

            ChangeGraphic(0);
            playSound = false;
        }

    }



    public void ChangeGraphic(int index)
{

    
    

    for (int i = 0; i < Corridors.Count; i++)
    {
        Corridors[i].SetActive(i == index);
    }
}



}