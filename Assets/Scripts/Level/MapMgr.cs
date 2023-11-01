using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
                DestoryTile(SceneChange.keyID);
                SceneChange.ChangeScence();



            }

            else if (dicBox.ContainsKey(targetPosCharacter) )
            {
                UnityEngine.Debug.Log("Box");

                //Check whether this block is box
                TileViewItem box = (TileViewItem)dicBox[targetPosCharacter];
                Vector2Int targetPosBox = box.posID + dir;


                if (
                    ( !dicBox.ContainsKey(targetPosBox) || ( dicBox.ContainsKey(targetPosBox) && dicBox[targetPosBox].IsDestroyed ) ) &&

                    ( !dicWall.ContainsKey(targetPosBox) || (dicWall.ContainsKey(targetPosBox) && dicBox[targetPosCharacter].IsDestroyed) )&&
                     //Ice
                    ( !dicIce.ContainsKey(targetPosBox) || ( dicIce.ContainsKey(targetPosBox) && dicIce[targetPosBox].IsDestroyed ) || (dicIce.ContainsKey(targetPosBox) && dicBox[targetPosCharacter].IsDestroyed)) &&
                    //crystal
                    ( !dicCrystal.ContainsKey(targetPosBox) || ( dicCrystal.ContainsKey(targetPosBox) && dicCrystal[targetPosBox].IsDestroyed ) || ( dicCrystal.ContainsKey(targetPosBox) && dicBox[targetPosCharacter].IsDestroyed )  ))
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
                            //The absence of a box in the trap

                            UnityEngine.Debug.Log("Traps.TrapsIsFilled" + Traps.TrapsIsFilled);

                            

                            //Record Move
                            ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                            listAllAction.Add(characterAction);
                            ActionRecordData boxAction = new(box.keyID, box.posID, box.posID + dir);
                            listAllAction.Add(boxAction);


                            DestoryTile(box.keyID);

                            //Record changes in status
                            DestoryStateRecordData trapAction = new(Traps.keyID, targetPosBox, TileType.Traps);
                            listAllAction.Add(trapAction);
                            DestoryStateRecordData BoxAction = new(box.keyID, box.posID, TileType.Box);
                            listAllAction.Add(BoxAction);

                            //Move
                            curCharacter.Move(dir);
                            box.Move(dir);

                            Traps.TrapsIsFilled = true;

                        }
                        else
                        {
                            UnityEngine.Debug.Log("TrapsandBox");

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
                



               
                   /* if ((!dicBox.ContainsKey(targetPosBox) || (dicBox.ContainsKey(targetPosBox) && dicBox[targetPosBox].IsDestroyed)) &&
                         !dicWall.ContainsKey(targetPosBox) &&

                        //Ice
                        (!dicIce.ContainsKey(targetPosBox) || (dicIce.ContainsKey(targetPosBox) && dicIce[targetPosBox].IsDestroyed)) &&
                       //crystal
                       (!dicCrystal.ContainsKey(targetPosBox) || (dicCrystal.ContainsKey(targetPosBox) && dicCrystal[targetPosBox].IsDestroyed)))
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
                                //The absence of a box in the trap

                                UnityEngine.Debug.Log("Traps.TrapsIsFilled" + Traps.TrapsIsFilled);

                                //Move
                                curCharacter.Move(dir);

                                //Record Move
                                ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                                listAllAction.Add(characterAction);



                                DestoryTile(box.keyID);

                                //Record changes in status
                                DestoryStateRecordData trapAction = new(Traps.keyID, targetPosBox, TileType.Traps);
                                listAllAction.Add(trapAction);
                                DestoryStateRecordData BoxAction = new(box.keyID, targetPosCharacter, TileType.Box);
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
                        else
                        {
                            ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                            listAllAction.Add(characterAction);

                            ActionRecordData boxAction = new(box.keyID, box.posID, box.posID + dir);
                            listAllAction.Add(boxAction);

                            curCharacter.Move(dir);
                            box.Move(dir);
                        }



                    
                }*/

                
                
            }

            else if (dicCrystal.ContainsKey(targetPosCharacter))
            {


                UnitViewItem Crystal = (UnitViewItem)dicCrystal[targetPosCharacter];
                if (Crystal.IsDestroyed == false)
                {
                    Crystal.IsDestroyed = true;
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

                    DestoryStateRecordData CrystalAction = new(ItemKeyID, targetPosCharacter, TileType.Crystal);
                    listAllAction.Add(CrystalAction);

                }
                else
                {
                    UnityEngine.Debug.Log("No Crystal");
                    curCharacter.Move(dir);
                    //Record Move
                    ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                    listAllAction.Add(characterAction);
                }


            }

            else if (dicIce.ContainsKey(targetPosCharacter))
            {
                UnityEngine.Debug.Log("Ice");
                IceViewItem Ice = (IceViewItem)dicIce[targetPosCharacter];
                //No effect
                if (Ice.iceIsCracked && Ice.IsDestroyed == false)
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

                    dicIce[targetPosCharacter].IsDestroyed = true;
                    DestoryStateRecordData IceAction = new(Ice.keyID, targetPosCharacter, TileType.Ice);
                    listAllAction.Add(IceAction);

                    



                }
                else if ( !Ice.iceIsCracked && Ice.IsDestroyed == false)
                {
                    Ice.iceIsCracked = true;

                    DestoryStateRecordData IceAction = new(Ice.keyID, targetPosCharacter, TileType.Ice);
                    listAllAction.Add(IceAction);
                }
                else
                {
                    curCharacter.Move(dir);
                    ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                    listAllAction.Add(characterAction);
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

    private void UndoDestroyEvent(object info)
    {
        DestoryStateRecordData recordData = (DestoryStateRecordData)info;

        if (recordData.tileType == TileType.Box)
        {
            UnitViewItem Box = dicAllTile[recordData.keyID];
            Box.IsDestroyed = false;
            Box.inactivation();
            Box.MoveToTarPos(recordData.PosID);


        }
        else if (recordData.tileType == TileType.Crystal)
        {
            UnitViewItem Crystal = (UnitViewItem)dicCrystal[recordData.PosID];
            Crystal.IsDestroyed = false;
            Crystal.inactivation();
            //计数要改变，还没做

           


        }
        else if (recordData.tileType == TileType.Traps)
        {

            TrapsViemItem Traps = (TrapsViemItem)dicTraps[recordData.PosID];
           

            Traps.TrapsIsFilled = false;

        }
        else if (recordData.tileType == TileType.Ice)
        {
            IceViewItem Ice = (IceViewItem)dicIce[recordData.PosID];
            if (Ice.IsDestroyed == false)
            {
                UnityEngine.Debug.Log("Undoice");
                Ice.iceIsCracked=false;


            }
            else
            {
                UnityEngine.Debug.Log("UndoiceIsCracked");
                Ice.inactivation();
                Ice.iceIsCracked = true;
                Ice.IsDestroyed = false;
                
            }



        }

    }

    #endregion

    #region DestoryTile
    public void DestoryTile(int keyID)
    {
        if (dicAllTile.ContainsKey(keyID))
        {
            TileViewItem tileViewItem = dicAllTile[keyID];
            tileViewItem.IsDestroyed = true;
            tileViewItem.inactivation();
            //listTile.Remove(tileViewItem);
            //dicAllTile.Remove(keyID);
            //tileViewItem.IsDestroyed = true;
            //tileViewItem.gameObject.SetActive(false);
            //Destroy(tileViewItem.gameObject);
            ScanAllPos();
        }

    }
    #endregion



    /*public void RegenerateTile(TileType tileType, Vector2Int posID)
    {
        // 首先确定要重新生成的瓷砖的类型和位置
        switch (tileType)
        {
            case TileType.Crystal:
                // 在dicCrystal中查找具有特定posID的瓷砖
                if (dicCrystal.ContainsKey(posID))
                {
                    UnitViewItem existingCrystal = dicCrystal[posID];

                    // 检查瓷砖是否已被销毁
                    if (existingCrystal.IsDestroyed)
                    {
                        // 取消瓷砖的非销毁状态
                        existingCrystal.IsDestroyed = false;

                        // 在这里实例化新的瓷砖游戏对象，你需要根据具体情况创建新的对象
                        //GameObject newCrystalObject = Instantiate(icePrefab);

                        // 设置新的瓷砖的位置和其他属性
                        // 使用posID和其他信息来确定位置

                        // 如果有需要，你可以将新的瓷砖添加到相应的字典中，以便跟踪它
                        // 例如，如果你的字典是Dictionary<Vector2Int, UnitViewItem>
                        //dicCrystal[posID] = newCrystalObject.GetComponent<UnitViewItem>();
                    }
                }
                break;

            case TileType.Box:
                // 同样的方法处理箱子的重新生成
                if (dicBox.ContainsKey(posID))
                {
                    UnitViewItem existingBox = dicBox[posID];
                    if (existingBox.IsDestroyed)
                    {
                        //existingBox.IsDestroyed = false;

                        //GameObject newBoxObject = Instantiate(boxPrefab);

                        // 设置新的瓷砖的位置和其他属性

                        //dicBox[posID] = newBoxObject.GetComponent<UnitViewItem>();
                    }
                }
                break;

                // 其他类型的瓷砖可以类似处理
        }
    }*/

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
        ScanAllPos();

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

