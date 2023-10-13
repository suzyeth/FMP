using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameData
{
    public int energy = 0;
    public Dictionary<int, Crystal> dicCrystal = new Dictionary<int, Crystal>();

    //Init game data
    public GameData()
    {
        energy = 0;
        dicCrystal.Clear();
    }

    public void AddCrystal(int id)
    {
        if (!dicCrystal.ContainsKey(id))
        {
            Crystal crystal = new Crystal(id);
            dicCrystal.Add(id, crystal);
        }
    }

    public Crystal GetCrystal(int id)
    {
        if (dicCrystal.ContainsKey(id))
        {
            return dicCrystal[id]; 
        }
        else
        {
            return null;
        }
    }

    //Get the number of active crystal
    public int GetNumActiveCrystal()
    {
        //return dicCrystal.Count;
        int count = 0;
        foreach(var item in dicCrystal)
        {
            if (!item.Value.isSubmitted)
            {
                count++;
            }
        }
        return count;
    }
}

public class Crystal
{
    public int keyID = 0;
    public int energy = 0;
    public bool isSubmitted = false;

    public Crystal(int keyID)
    {
        this.keyID = keyID;
        energy = 0;
        isSubmitted = false;
    }
}