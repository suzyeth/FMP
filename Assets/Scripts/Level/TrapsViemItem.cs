using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrapsViemItem : TileViewItem

{
    public List<GameObject> Corridors = new List<GameObject>();
   
    
    
    
    public bool TrapsIsFilled = false;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        if (TrapsIsFilled)
        {
            ChangeGraphic(1);
        }
        else if (!TrapsIsFilled)
        {
            ChangeGraphic(0);
        }

    }

    public void ChangeGraphic(int index)
    {
        //index = index += direction;
        index = Math.Clamp(index, 0, Corridors.Count - 1);
        Debug.Log("index" + index);

        for (int i = 0; i < Corridors.Count; i++)
        {
            Corridors[i].SetActive(i == index);
        }
    }
}
