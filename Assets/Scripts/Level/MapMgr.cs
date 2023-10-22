using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
/*
public class TileData
{

}
*/


public class MapMgr : MonoBehaviour
{
    private GameData gameData;

    public Transform tfCharacter;
    private CharacterViewItem curCharacter;


    public Transform tfTile;
    /// <summary>
    /// List of Tile View£¨Including Box Wall)
    /// </summary>
    private List<TileViewItem> listTile = new List<TileViewItem>();
    
    /// <summary>
    /// Dictionary of Box Position. Key is Pos ID such as (0,1)
    /// </summary>
    //Store the relationship between PosID --- Different Types of tile
    private Dictionary<Vector2Int, UnitViewItem> dicBox = new Dictionary<Vector2Int, UnitViewItem>();
    private Dictionary<Vector2Int, UnitViewItem> dicButton = new Dictionary<Vector2Int, UnitViewItem>();
    private Dictionary<Vector2Int, UnitViewItem> dicWall = new Dictionary<Vector2Int, UnitViewItem>();
    //energy
    private Dictionary<Vector2Int, UnitViewItem> dicCrystal = new Dictionary<Vector2Int, UnitViewItem>();
    private Dictionary<Vector2Int, UnitViewItem> dicIce = new Dictionary<Vector2Int, UnitViewItem>();
    private Dictionary<Vector2Int, UnitViewItem> dicRedDoor = new Dictionary<Vector2Int, UnitViewItem>();
    private Dictionary<Vector2Int, UnitViewItem> dicTraps = new Dictionary<Vector2Int, UnitViewItem>();
    //Store the relationship between keyID ----- Particular tile
    private Dictionary<int, TileViewItem> dicAllTile = new Dictionary<int, TileViewItem>();

    public int keyID = -1;

    public void Init()
    {
        gameData = PublicTool.GetGameData();

        CheckAllCharacter();
        CheckAllTilePos();
        ScanAllPos();
    }

    #region Event

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener("CharacterMove", CharacterMoveEvent);
        EventCenter.Instance.AddEventListener("Undo", UndoEvent);
        


    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener("CharacterMove", CharacterMoveEvent);
        EventCenter.Instance.RemoveEventListener("Undo", UndoEvent);
        


    }
    #endregion

    #region CharacterMove

    //CharacterMove
    private void CharacterMoveEvent(object arg0)
    {
        Vector2Int dir = (Vector2Int)arg0;
        if (curCharacter != null)
        {

            List<ActionRecordData> listAllAction = new List<ActionRecordData>();

            Vector2Int startCharacterPosID = curCharacter.posID;
            Vector2Int targetPosCharacter = curCharacter.posID + dir;

            //Add Character Action

            if (dicWall.ContainsKey(targetPosCharacter)|| dicRedDoor.ContainsKey(targetPosCharacter))
            {
                if (dicWall.ContainsKey(targetPosCharacter)) 
                {
                    UnityEngine.Debug.Log("Wall");
                    //No effect
                }
                else if(dicRedDoor.ContainsKey(targetPosCharacter))
                {
                    UnityEngine.Debug.Log("RedDoor");
                    //No effect
                }
            }
            else if (dicCrystal.ContainsKey(targetPosCharacter))
            {
                UnityEngine.Debug.Log("Crystal");
                gameData.AddCrystal(1);
                gameData.GetNumActiveCrystal();
            

                // Move
                curCharacter.Move(dir);
                //Record Move
                ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                listAllAction.Add(characterAction);

            }
            else if (dicButton.ContainsKey(targetPosCharacter))
                {
                    UnityEngine.Debug.Log("Button");
                //Move
                curCharacter.Move(dir);
                //Record Move
                ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                listAllAction.Add(characterAction);

            }
            else if (dicTraps.ContainsKey(targetPosCharacter))
            {
                UnityEngine.Debug.Log("Traps");
                // Move
                curCharacter.Move(dir);
                //Record Move
                ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                listAllAction.Add(characterAction);
            }

            else if (dicBox.ContainsKey(targetPosCharacter))
            {
                UnityEngine.Debug.Log("Box");

                //Check whether this block is box
                TileViewItem box = (TileViewItem)dicBox[targetPosCharacter];
                Vector2Int targetPosBox = box.posID + dir;

                if (!dicBox.ContainsKey(targetPosBox) && !dicWall.ContainsKey(targetPosBox) && !dicRedDoor.ContainsKey(targetPosBox))
                {
                   
                        ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                        listAllAction.Add(characterAction);

                        ActionRecordData boxAction = new(box.keyID, box.posID, box.posID + dir);
                        listAllAction.Add(boxAction);

                        curCharacter.Move(dir);
                        box.Move(dir);
                    
                }
            }
            
            //Empty
            else
            {
                //Move
                curCharacter.Move(dir);
                //Record Move
                ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                listAllAction.Add(characterAction);
            }


            if (listAllAction != null && listAllAction.Count > 0)
            {
                gameData.AddRecordAction(listAllAction);
            }

            //Compare the pos of tiles and characters in relation to each other
            


            ScanAllPos();
        }
    }

    public void DestoryTile(int ketID)
    {
        if (dicAllTile.ContainsKey(keyID))
        { 
        TileViewItem tileViewItem = dicAllTile[keyID];
            listTile.Remove(tileViewItem);
            dicAllTile.Remove(keyID);   
            Destroy(tileViewItem.gameObject);
            ScanAllPos();
        }
    
    }
    private void UndoEvent(object info)
    {
        ActionRecordData recordData = (ActionRecordData)info;
        if (recordData.keyID == -2)
        {
            curCharacter.MoveToTarPos(recordData.startPos);
        }
        else if(recordData.keyID>=0)
        {
            if (dicAllTile.ContainsKey(recordData.keyID))
            {
                TileViewItem tile = dicAllTile[recordData.keyID];
                tile.MoveToTarPos(recordData.startPos);
            }
        }
        ScanAllPos();
    }

    
   

    #endregion

    #region InitCheck

    public void CheckAllCharacter()
    {
        foreach (Transform child in tfCharacter)
        {
            curCharacter = child.GetComponent<CharacterViewItem>();
            curCharacter.Init();
        }
    }

    /// <summary>
    /// Check All the Position of tile (dont care the type)
    /// </summary>
    public void CheckAllTilePos()
    {
        listTile.Clear();
        foreach (Transform child in tfTile)
        {
            TileViewItem itemTile = child.GetComponent<TileViewItem>();
            itemTile.Init();
            listTile.Add(itemTile);
        }
    }
   

    #endregion

    #region Scan

    public void ScanAllPos()
    {
        dicAllTile.Clear();
        dicBox.Clear();
        dicButton.Clear();
        dicWall.Clear();
        dicCrystal.Clear();
        dicIce.Clear();
        dicRedDoor.Clear();
        dicTraps.Clear();


        keyID = -1;
        foreach(var tile in listTile)
        {
            //Only record when map start
            keyID++;
            tile.keyID = keyID;

            //Pos Refresh Every time
            dicAllTile.Add(keyID, tile);
            switch (tile.tileType)
            {
                case TileType.Box:
                    dicBox.Add(tile.posID, tile);
                    break;
                case TileType.Button:
                    dicButton.Add(tile.posID, tile);
                    break;
                case TileType.RedDoor:
                    dicRedDoor.Add(tile.posID, tile);
                    break;
                case TileType.Wall:
                    dicWall.Add(tile.posID, tile);
                    break;
                case TileType.Crystal:
                    dicCrystal.Add(tile.posID, tile);
                    break;
                case TileType.Ice:
                    dicIce.Add(tile.posID, tile);
                    break;
                case TileType.Traps:
                    dicTraps.Add(tile.posID, tile);
                    break;
                case TileType.SceneChange:
                    //dicSceneChange.Add(tile.posID, tile);
                    break;
            }
        }
    }

    #endregion
}
