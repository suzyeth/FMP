using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
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
    /// List of Tile View（Including Box Wall)
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
    private Dictionary<Vector2Int, UnitViewItem> dicBlueDoor = new Dictionary<Vector2Int, UnitViewItem>();
    private Dictionary<Vector2Int, UnitViewItem> dicTraps = new Dictionary<Vector2Int, UnitViewItem>();
    private Dictionary<Vector2Int, UnitViewItem> dicSceneChange = new Dictionary<Vector2Int, UnitViewItem>();

    //Store the relationship between keyID ----- Particular tile
    private Dictionary<int, TileViewItem> dicAllTile = new Dictionary<int, TileViewItem>();

    public int keyID = -1;
    public int ItemKeyID = -1;

    
    
    private bool ButtonIsPressed = false;
    

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

            if (dicWall.ContainsKey(targetPosCharacter))
            {
                if (dicWall.ContainsKey(targetPosCharacter))
                {
                    UnityEngine.Debug.Log("Wall");
                    //No effect
                }

            }

            else if (dicCrystal.ContainsKey(targetPosCharacter))
            {
                UnityEngine.Debug.Log("Crystal");
                gameData.AddCrystal(1);
                gameData.GetNumActiveCrystal();

                //Get the keyID of crystal 
                ScanCrystalkeyID(targetPosCharacter);
                //?尚未实现输出能量数量

                DestoryTile(ItemKeyID);
                // Move
                curCharacter.Move(dir);
                //Record Move
                ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                listAllAction.Add(characterAction);

            }
          
            else if (dicIce.ContainsKey(targetPosCharacter))
            {
                UnityEngine.Debug.Log("Ice");
                IceViewItem Ice = (IceViewItem)dicIce[targetPosCharacter];
                //No effect
                if (dicIce.ContainsKey(targetPosCharacter) && Ice.iceIsCracked)
                {
                    //Attachment of properties to a single prefabricated body
                    UnityEngine.Debug.Log("Icebroken");
                    ScanIceKeyID(targetPosCharacter);
                    DestoryTile(ItemKeyID);

                    //Move
                    curCharacter.Move(dir);
                    //Record Move
                    ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                    listAllAction.Add(characterAction);


                    
                }
                else
                {
                    Ice.iceIsCracked = true;
                }





            }

            else if (dicBox.ContainsKey(targetPosCharacter))
            {
                UnityEngine.Debug.Log("Box");

                //Check whether this block is box
                TileViewItem box = (TileViewItem)dicBox[targetPosCharacter];
                Vector2Int targetPosBox = box.posID + dir;

                if (!dicBox.ContainsKey(targetPosBox) && !dicWall.ContainsKey(targetPosBox)  && !dicIce.ContainsKey(targetPosBox))
                {
                    
                    if (dicButton.ContainsKey(targetPosBox))
                    {
                        UnityEngine.Debug.Log("Box&Button");

                        ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                        listAllAction.Add(characterAction);

                        ActionRecordData boxAction = new(box.keyID, box.posID, box.posID + dir);
                        listAllAction.Add(boxAction);

                        curCharacter.Move(dir);
                        box.Move(dir);
                    }
                    else if (dicTraps.ContainsKey(targetPosBox))
                    {
                        TrapsViemItem Traps = (TrapsViemItem)dicTraps[targetPosBox];

                        if (!Traps.TrapsIsFilled)
                        {
                            UnityEngine.Debug.Log("BoxDrop");

                            ScanBoxkeyID(targetPosCharacter);
                            DestoryTile(ItemKeyID);

                            //Move
                            curCharacter.Move(dir);
                            //Record Move
                            ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                            listAllAction.Add(characterAction);
                            Traps.TrapsIsFilled = true;
                        }
                        else
                        {
                            ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                            listAllAction.Add(characterAction);

                            ActionRecordData boxAction = new(box.keyID, box.posID, box.posID + dir);
                            listAllAction.Add(boxAction);

                            curCharacter.Move(dir);
                            box.Move(dir);
                        }



                    }
                    else if (dicRedDoor.ContainsKey(targetPosBox) )
                    {
                        if (ButtonIsPressed)
                        {
                            UnityEngine.Debug.Log("Box&RedDoor");
                            ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                            listAllAction.Add(characterAction);

                            ActionRecordData boxAction = new(box.keyID, box.posID, box.posID + dir);
                            listAllAction.Add(boxAction);

                            curCharacter.Move(dir);
                            box.Move(dir);
                        }
                        else 
                        {
                            UnityEngine.Debug.Log("Can't go through reddoor");
                        }

                        
                    }
                    else if (dicBlueDoor.ContainsKey(targetPosBox) )
                    {
                        if (!ButtonIsPressed)
                        {
                            UnityEngine.Debug.Log("Box&BlueDoor");
                            ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                            listAllAction.Add(characterAction);

                            ActionRecordData boxAction = new(box.keyID, box.posID, box.posID + dir);
                            listAllAction.Add(boxAction);

                            curCharacter.Move(dir);
                            box.Move(dir);
                        }
                        else
                        {
                            UnityEngine.Debug.Log("Can't go through bluedoor");
                        }

                        
                    }
                    
                    else
                    {
                        ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                        listAllAction.Add(characterAction);

                        ActionRecordData boxAction = new(box.keyID, box.posID, box.posID + dir);
                        listAllAction.Add(boxAction);

                        curCharacter.Move(dir);
                        box.Move(dir);
                    }



                }
            }

            else if (dicTraps.ContainsKey(targetPosCharacter))
            {
                UnityEngine.Debug.Log("Traps");
                TrapsViemItem Traps = (TrapsViemItem)dicTraps[targetPosCharacter];

                if (Traps.TrapsIsFilled)
                {
                    UnityEngine.Debug.Log("TrapsIsFilled");
                    //Move
                    curCharacter.Move(dir);
                    //Record Move
                    ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                    listAllAction.Add(characterAction);
                }
                else
                {
                    UnityEngine.Debug.Log("TrapsIsImpassable");
                    //No effect
                    //调用UNDO

                }

            }

            else if (dicRedDoor.ContainsKey(targetPosCharacter))
            {
                // DoorViewItem redDoor = (DoorViewItem)dicRedDoor[targetPosCharacter];
                // if(redDoor.IsOpen())
                if (ButtonIsPressed)
                {
                    UnityEngine.Debug.Log("RedDoorisopen");
                    //Move
                    curCharacter.Move(dir);
                    //Record Move
                    ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                    listAllAction.Add(characterAction);
                }
                else
                {
                    UnityEngine.Debug.Log("RedDoorisclose");

                    //No effect
                }

            }

            else if (dicBlueDoor.ContainsKey(targetPosCharacter))
            {
                UnityEngine.Debug.Log("BlueDoor");
                if (!ButtonIsPressed)
                {
                    UnityEngine.Debug.Log("BlueDoorisopen");
                    //Move
                    curCharacter.Move(dir);
                    //Record Move
                    ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                    listAllAction.Add(characterAction);
                }
                else
                {
                    UnityEngine.Debug.Log("BlueDoorisclose");
                    //No effect
                }

            }

            else if (dicButton.ContainsKey(targetPosCharacter))
            {
                UnityEngine.Debug.Log("Button");
                //Move
                curCharacter.Move(dir);
                //Record Move
                ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                listAllAction.Add(characterAction);
                CheckButtonState();

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

            CheckButtonState();
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
        else if (recordData.keyID >= 0)
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

    #region DestoryTile
    public void DestoryTile(int keyID)
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
        dicBlueDoor.Clear();
        dicTraps.Clear();
        dicSceneChange.Clear();



        keyID = -1;
        foreach (var tile in listTile)
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
                case TileType.BlueDoor:
                    dicBlueDoor.Add(tile.posID, tile);
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
                    dicSceneChange.Add(tile.posID, tile);
                    break;
            }
        }
    }

    #endregion

    #region Get the CrystalkeyID 
    public void ScanCrystalkeyID(Vector2Int CrystalPosID)
    {
        keyID = -1;
        foreach (var tile in listTile)
        {
            keyID++;

            if (dicAllTile[keyID] == dicCrystal[CrystalPosID])
            {
                ItemKeyID = keyID;
            }
        }
    }

    #endregion

    #region Get the IceKeyID 
    public void ScanIceKeyID(Vector2Int IceKeyID)
    {
        keyID = -1;
        foreach (var tile in listTile)
        {
            keyID++;

            if (dicAllTile[keyID] == dicIce[IceKeyID])
            {
                ItemKeyID = keyID;
            }
        }
    }

    #endregion

    #region Get the ButtonkeyID 
    public void ScanButtonkeyID(Vector2Int ButtonPosID)
    {
        keyID = -1;
        foreach (var tile in listTile)
        {
            keyID++;

            if (dicAllTile[keyID] == dicButton[ButtonPosID])
            {
                ItemKeyID = keyID;

            }
        }
    }

    #endregion

    #region Get the BoxkeyID 
    public void ScanBoxkeyID(Vector2Int BoxkeyID)
    {
        keyID = -1;
        foreach (var tile in listTile)
        {
            keyID++;

            if (dicAllTile[keyID] == dicBox[BoxkeyID])
            {
                ItemKeyID = keyID;

            }
        }
    }

    #endregion



    #region CheckButtonState
    public void CheckButtonState()
    {
        //Button and box coincide. Red door open.

        //Character and box positions coincide, red door opens

        //Otherwise the red door closes
        Vector2Int CharacterPosID = curCharacter.posID;
        ScanAllPos();

       foreach (var kyevalue in dicButton)
        {
          Vector2Int ButtonKeyID = kyevalue.Key;

          if (dicBox.ContainsKey(ButtonKeyID))
          {
              ButtonIsPressed = true;
               // ButtonViewItem button = (ButtonViewItem)kyevalue.Value;
                //button.OnPress();
           }
           else if (ButtonKeyID == CharacterPosID)
           { 
                ButtonIsPressed = true;
                //ButtonViewItem button = (ButtonViewItem)kyevalue.Value;
               // button.OnPress();
            }
           else
            {
               ButtonIsPressed = false;
                //ButtonViewItem button = (ButtonViewItem)kyevalue.Value;
                //button.OnRelease();
            }


        }


    }



    #endregion

    #region CheckTrapsState
    public void CheckTrapsState()
    {
       
      //  Vector2Int CharacterPosID = curCharacter.posID;

     //   foreach (var kyevalue in dicTraps)
       // {
       //     TrapsKeyID = KeyValuePair.key;
       //     if (dicBox.ContainsKey(TrapsKeyID))
            {
       //         TrapsIsPassable = true;
            }
       //     else
            {
       //         TrapsIsPassable = false;
            }


        }


    }



    #endregion


