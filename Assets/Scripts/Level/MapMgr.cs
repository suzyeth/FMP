using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.WSA;
using static UnityEngine.ParticleSystem;
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
    private Dictionary<Vector2Int, ButtonViewItem> dicButton = new Dictionary<Vector2Int, ButtonViewItem>();
    private Dictionary<Vector2Int, UnitViewItem> dicWall = new Dictionary<Vector2Int, UnitViewItem>();
    //energy
    private Dictionary<Vector2Int, UnitViewItem> dicCrystal = new Dictionary<Vector2Int, UnitViewItem>();
    private Dictionary<Vector2Int, IceViewItem> dicIce = new Dictionary<Vector2Int, IceViewItem>();
    private Dictionary<Vector2Int, RedDoorViewItem> dicRedDoor = new Dictionary<Vector2Int, RedDoorViewItem>();
    private Dictionary<Vector2Int, BlueDoorViewItem> dicBlueDoor = new Dictionary<Vector2Int, BlueDoorViewItem>();
    private Dictionary<Vector2Int, TrapsViemItem> dicTraps = new Dictionary<Vector2Int, TrapsViemItem>();
    private Dictionary<Vector2Int, UnitViewItem> dicSceneChange = new Dictionary<Vector2Int, UnitViewItem>();
    private Dictionary<Vector2Int, UnitViewItem> dicSpikes = new Dictionary<Vector2Int, UnitViewItem>();

    //Store the relationship between keyID ----- Particular tile
    private Dictionary<int, TileViewItem> dicAllTile = new Dictionary<int, TileViewItem>();

    public int keyID = -1;
    public int ItemKeyID = -1;


    public GameObject IcePrefab;
    public GameObject BoxPrefab;
    public GameObject CrystalPrefab;





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
        EventCenter.Instance.AddEventListener("UndoDestroy", UndoDestroyEvent);


    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener("CharacterMove", CharacterMoveEvent);
        EventCenter.Instance.RemoveEventListener("Undo", UndoEvent);
        EventCenter.Instance.RemoveEventListener("UndoDestroy", UndoDestroyEvent);



    }
    #endregion

    #region CharacterMove

    //CharacterMove
    private void CharacterMoveEvent(object arg0)
    {
        Vector2Int dir = (Vector2Int)arg0;

        if (curCharacter != null)
        {

            List<BaseRecordData> listAllAction = new List<BaseRecordData>();

            Vector2Int startCharacterPosID = curCharacter.posID;
            Vector2Int targetPosCharacter = curCharacter.posID + dir;

            //Add Character Action

            if (dicWall.ContainsKey(targetPosCharacter))
            {

                UnityEngine.Debug.Log("Wall");
                //No effect


            }

            else if (dicSpikes.ContainsKey(targetPosCharacter))
            {

                UnityEngine.Debug.Log("Spikes");
                //No effect


            }

            else if (dicSceneChange.ContainsKey(targetPosCharacter))
            {
                UnityEngine.Debug.Log("SceneChange");
                
                ChangeScenceViewItem SceneChange = (ChangeScenceViewItem)dicSceneChange[targetPosCharacter];
                DestoryTile(SceneChange.keyID, SceneChange.posID);
                SceneChange.ChangeScence();



            }

            else if (dicBox.ContainsKey(targetPosCharacter) )
            {
                UnityEngine.Debug.Log("Box");

                //Check whether this block is box
                TileViewItem box = (TileViewItem)dicBox[targetPosCharacter];
                Vector2Int targetPosBox = box.posID + dir;
   
                   if (!dicBox.ContainsKey(targetPosBox) && !dicIce.ContainsKey(targetPosBox) && !dicCrystal.ContainsKey(targetPosBox) && !dicWall.ContainsKey(targetPosBox))
                    {

                        
                        if (dicTraps.ContainsKey(targetPosBox))
                        {
                            TrapsViemItem Traps = (TrapsViemItem)dicTraps[targetPosBox];

                            if (!Traps.TrapsIsFilled)
                            {
                                //The absence of a box in the trap

                                

                                //Move
                                curCharacter.Move(dir);

                                //Record Move
                                ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                                listAllAction.Add(characterAction);



                                DestoryTile(box.keyID, box.posID);

                                //Record changes in status
                                DestoryStateRecordData trapAction = new(Traps.keyID, targetPosBox, TileType.Traps);
                                listAllAction.Add(trapAction);
                                DestoryStateRecordData BoxAction = new(box.keyID, box.posID, TileType.Box);
                                listAllAction.Add(BoxAction);

                                Traps.TrapsIsFilled = true;

                            }
                            else
                            {


                                curCharacter.Move(dir);
                                box.Move(dir);

                                ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                                listAllAction.Add(characterAction);

                                ActionRecordData boxAction = new(box.keyID, box.posID, box.posID + dir);
                                listAllAction.Add(boxAction);
                            }



                        }
                        else if (dicRedDoor.ContainsKey(targetPosBox))
                        {
                            if (dicRedDoor[targetPosBox].DoorIsOpened)
                            {
                                
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
                        else if (dicBlueDoor.ContainsKey(targetPosBox))
                        {
                            if (dicBlueDoor[targetPosBox].DoorIsOpened)
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
                        else if (dicSpikes.ContainsKey(targetPosBox))
                        {
                        ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                        listAllAction.Add(characterAction);

                        ActionRecordData boxAction = new(box.keyID, box.posID, box.posID + dir);
                        listAllAction.Add(boxAction);

                        curCharacter.Move(dir);
                        box.Move(dir);
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

            else if (dicCrystal.ContainsKey(targetPosCharacter))
            {


                TileViewItem Crystal = (TileViewItem)dicCrystal[targetPosCharacter];
                
                    UnityEngine.Debug.Log("Crystal");
                    gameData.AddCrystal(1);
                    gameData.GetNumActiveCrystal();

                    //Get the keyID of crystal 
                    ScanCrystalkeyID(targetPosCharacter);
                    //?尚未实现输出能量数量

                    DestoryTile(ItemKeyID, Crystal.posID);
                    // Move
                    curCharacter.Move(dir);
                    //Record Move
                    ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                    listAllAction.Add(characterAction);

                    DestoryStateRecordData CrystalAction = new(Crystal.keyID, targetPosCharacter, TileType.Crystal);
                    listAllAction.Add(CrystalAction);

               


            }

            else if (dicIce.ContainsKey(targetPosCharacter))
            {
                UnityEngine.Debug.Log("Ice");
                IceViewItem Ice = (IceViewItem)dicIce[targetPosCharacter];
                //No effect
                if (Ice.iceIsCracked )
                {

                    //Attachment of properties to a single prefabricated body
                    UnityEngine.Debug.Log("IceDisappear");
                    
                    DestoryTile(Ice.keyID, Ice.posID);

                    //Move
                    curCharacter.Move(dir);
                    //Record Move
                    ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                    listAllAction.Add(characterAction);

                    
                    DestoryStateRecordData IceAction = new(Ice.keyID, targetPosCharacter, TileType.Ice);
                    listAllAction.Add(IceAction);

                    



                }
                else 
                {
                    Ice.iceIsCracked = true;

                    DestoryStateRecordData IceAction = new(Ice.keyID, targetPosCharacter, TileType.Ice);
                    listAllAction.Add(IceAction);
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
                if (dicRedDoor[targetPosCharacter].DoorIsOpened)
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
                if (dicBlueDoor[targetPosCharacter].DoorIsOpened)
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
        CheckButtonState();

    }

    private void UndoDestroyEvent(object info)
    {
        DestoryStateRecordData recordData = (DestoryStateRecordData)info;
         if (recordData.tileType == TileType.Ice)
        {
            IceViewItem Ice;
            if (dicIce.TryGetValue(recordData.PosID, out IceViewItem ice))
            {
                Ice = (IceViewItem)dicIce[recordData.PosID];
                Ice.iceIsCracked = false;
            }
            else
            {
                // 处理字典中不存在该键的情况

                
                //RegenerateTile(IcePrefab, recordData.PosID);
                //Ice.inactivation();
                RegenerateTile(IcePrefab, recordData.PosID);

                ScanAllPos();
                Ice = (IceViewItem)dicIce[recordData.PosID];
                Ice.iceIsCracked = true;
                //Ice.IsDestroyed = false;

            }






        }
        else if(recordData.tileType == TileType.Box)
        {
            
            RegenerateTile(BoxPrefab, recordData.PosID);
            
            UnitViewItem Box = dicBox[recordData.PosID];
            
            //Box.IsDestroyed = false;
            //Box.inactivation();
            Box.MoveToTarPos(recordData.PosID);


        }
        else if (recordData.tileType == TileType.Crystal)
        {
            
            RegenerateTile(CrystalPrefab, recordData.PosID);
            

            UnitViewItem Crystal = (UnitViewItem)dicCrystal[recordData.PosID];
            //Crystal.IsDestroyed = false;
            //Crystal.inactivation();
            //计数要改变，还没做




        }
        else if (recordData.tileType == TileType.Traps)
        {

            TrapsViemItem Traps = (TrapsViemItem)dicTraps[recordData.PosID];
            Traps.TrapsIsFilled = false;
            UnityEngine.Debug.Log("Traps.TrapsIsFilled" + Traps.TrapsIsFilled);

        }
        
        CheckButtonState();
        ScanAllPos();

    }

    #endregion

    #region DestoryTile
    public void DestoryTile(int keyID,Vector2Int posID)
    {
        if (dicAllTile.ContainsKey(keyID))
        {
            if (dicBox.ContainsKey(posID))
            {
                dicBox.Remove(posID);
            }
            else if (dicCrystal.ContainsKey(posID))
            {
                dicCrystal.Remove(posID);
            }
            else if (dicIce.ContainsKey(posID))
            {
                dicIce.Remove(posID);
            }
            TileViewItem tileViewItem = dicAllTile[keyID];
            tileViewItem.IsDestroyed = true;
            //tileViewItem.inactivation();
            //tileViewItem.gameObject.SetActive(false);
            listTile.Remove(tileViewItem);
            dicAllTile.Remove(keyID);
            
            
            Destroy(tileViewItem.gameObject);
            ScanAllPos();
        }

    }
    #endregion

    public void RegenerateTile(GameObject Prefab, Vector2Int posID)
    {
        GameObject  newObject = Instantiate(Prefab, new Vector3(posID.x, posID.y, 0), Quaternion.identity);
       
        newObject.transform.parent = tfTile;
        Vector2Int crystalPos = posID;
        Vector2Int icePos = posID;
        Vector2Int boxPos = posID;


        if (Prefab == BoxPrefab)
        {

            TileViewItem BoxTileItem = newObject.GetComponent<TileViewItem>();
            BoxTileItem.posID = boxPos;
            listTile.Add(BoxTileItem);
            dicBox.Add(BoxTileItem.posID, BoxTileItem);
        }
        else if (Prefab == CrystalPrefab)
        {
            TileViewItem CrystalItem = newObject.GetComponent<TileViewItem>();
            CrystalItem.posID = crystalPos;
            listTile.Add(CrystalItem);
            dicCrystal.Add(CrystalItem.posID, CrystalItem);
        }
        else if (Prefab == IcePrefab)
        {
            IceViewItem IceTileItem = newObject.GetComponent<IceViewItem>();
            IceTileItem.posID = icePos;
            listTile.Add(IceTileItem);
            dicIce.Add(IceTileItem.posID, (IceViewItem)IceTileItem);
        }

        //TileViewItem newTileItem = newObject.GetComponent<TileViewItem>();
       // newTileItem.posID = posID;


        //listTile.Add(newTileItem);
        ScanAllPos();

    }



   
        

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
        dicSpikes.Clear();



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
                    dicButton.Add(tile.posID, (ButtonViewItem)tile);
                    break;
                case TileType.RedDoor:
                    dicRedDoor.Add(tile.posID, (RedDoorViewItem)tile);
                    break;
                case TileType.BlueDoor:
                    dicBlueDoor.Add(tile.posID, (BlueDoorViewItem)tile);
                    break;
                case TileType.Wall:
                    dicWall.Add(tile.posID, tile);
                    break;
                case TileType.Crystal:
                    dicCrystal.Add(tile.posID, tile);
                    break;
                case TileType.Ice:
                    dicIce.Add(tile.posID, (IceViewItem)tile);
                    break;
                case TileType.Traps:
                    dicTraps.Add(tile.posID, (TrapsViemItem)tile);
                    break;
                case TileType.Spikes:
                    dicSpikes.Add(tile.posID, tile);
                    break;
                case TileType.SceneChange:
                    dicSceneChange.Add(tile.posID, tile);
                    break;
            }
        }
    }

    #endregion

    #region Get the ItemID 
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
        

        foreach (var kyevalue in dicButton)
        {
            Vector2Int ButtonKeyID = kyevalue.Key;

            if (dicBox.ContainsKey(ButtonKeyID))
            {

                ButtonViewItem button = (ButtonViewItem)kyevalue.Value;
                
                button.ButtonIsPressed = true;
                button.OnPress();
            }
            else if (ButtonKeyID == CharacterPosID)
            {

                ButtonViewItem button = (ButtonViewItem)kyevalue.Value;
                
                button.ButtonIsPressed = true;
                button.OnPress();
            }
            else
            {

                ButtonViewItem button = (ButtonViewItem)kyevalue.Value;
                
                button.ButtonIsPressed = false;
                button.OnRelease();
            }


        }

        ScanAllPos();
    }



    #endregion



    public void ClearDataChangScence()
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
        dicSpikes.Clear();
    }

}

