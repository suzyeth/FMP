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
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
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
    private Dictionary<Vector2Int, UnitViewItem> dicGiveupSkills = new Dictionary<Vector2Int, UnitViewItem>();

    //Store the relationship between keyID ----- Particular tile
    private Dictionary<int, TileViewItem> dicAllTile = new Dictionary<int, TileViewItem>();

    public int keyID = -1;
    private int levelID;
    


    public GameObject IcePrefab;
    public GameObject BoxPrefab;
    public GameObject CrystalPrefab;

    public bool Ski1 =false;
    public bool Ski2 =false;
    public bool Ski3 = false;
    public bool Ski4 = false;

   



    public void Init(int levelID)
    {
        gameData = PublicTool.GetGameData();
        this.levelID = levelID;
        CheckAllCharacter();
        CheckAllTilePos();
        ScanAllPos(true);
    }

    #region Event

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener("CharacterMove", CharacterMoveEvent);
        EventCenter.Instance.AddEventListener("Undo", UndoEvent);
        EventCenter.Instance.AddEventListener("UndoDestroy", UndoDestroyEvent);

        //EventCenter.Instance.AddEventListener("IceBreakingSkill1", IceBreakingEvent);
        //EventCenter.Instance.AddEventListener("ThroughSpikesSkill2", ThroughSpikesEvent);
        EventCenter.Instance.AddEventListener("PullBoxSkill3", PullBoxEvent);
        EventCenter.Instance.AddEventListener("TeleportationSkill4", TeleportatioEvent);



    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener("CharacterMove", CharacterMoveEvent);
        EventCenter.Instance.RemoveEventListener("Undo", UndoEvent);
        EventCenter.Instance.RemoveEventListener("UndoDestroy", UndoDestroyEvent);

        //EventCenter.Instance.RemoveEventListener("IceBreakingSkill1", IceBreakingEvent);
        //EventCenter.Instance.RemoveEventListener("ThroughSpikesSkill2", ThroughSpikesEvent);
        EventCenter.Instance.RemoveEventListener("PullBoxSkill3", PullBoxEvent);
        EventCenter.Instance.RemoveEventListener("TeleportationSkill4", TeleportatioEvent);



    }
    #endregion

    #region CharacterMove

    //CharacterMove
    private void CharacterMoveEvent(object arg0)
    {
        Vector2Int dir = (Vector2Int)arg0;

        IceBreakingEvent(1);
        ThroughSpikesEvent(1);

        if (curCharacter != null)
        {

            List<BaseRecordData> listAllAction = new List<BaseRecordData>();

            Vector2Int startCharacterPosID = curCharacter.posID;
            Vector2Int targetPosCharacter = curCharacter.posID + dir;
            Vector2Int transferCharacter = curCharacter.posID + dir + dir;


            //Add Character Action
            if (Ski4)
            {
                if (!dicWall.ContainsKey(transferCharacter) && !dicBox.ContainsKey(transferCharacter) && !dicIce.ContainsKey(transferCharacter))
                {
                    if (dicTraps.ContainsKey(transferCharacter))
                    {
                        TrapsViemItem Traps = (TrapsViemItem)dicTraps[transferCharacter];
                        if (Traps.TrapsIsFilled)
                        {
                            UnityEngine.Debug.Log("TrapsIsFilled");
                            //Move
                            curCharacter.Move(dir + dir);

                            //Record Move
                            ActionRecordData characterAction = new(-2, startCharacterPosID, transferCharacter);
                            listAllAction.Add(characterAction);
                            //close skill4

                            gameData.UseSkills4Action();
                            Ski4 = false;


                        }
                        else
                        {
                            UnityEngine.Debug.Log("TrapsIsImpassable");
                            //No effect
                            //调用UNDO

                        }
                    }
                    else if (dicSceneChange.ContainsKey(transferCharacter))
                    {
                        UnityEngine.Debug.Log("SceneChange");

                        ChangeScenceViewItem SceneChange = (ChangeScenceViewItem)dicSceneChange[transferCharacter];
                        DestoryTile(SceneChange.keyID, SceneChange.posID);
                        SceneChange.ChangeScence();
                        gameData.SaveLevelData();



                    }
                    else if (dicGiveupSkills.ContainsKey(transferCharacter))
                    {
                     
                            GivingUpSkillsViewItem GiveupSkills = (GivingUpSkillsViewItem)dicGiveupSkills[transferCharacter];
                            DestoryTile(GiveupSkills.keyID, GiveupSkills.posID);
                            GiveupSkills.OpenGiveupSkillsPage();
                            gameData.SaveLevelData();
                        
                    }
                    else if (dicSpikes.ContainsKey(transferCharacter))
                    {

                        if (Ski2)
                        {
                            curCharacter.Move(dir + dir);
                            //Record Move
                            ActionRecordData characterAction = new(-2, startCharacterPosID, transferCharacter);
                            listAllAction.Add(characterAction);
                            gameData.UseSkills2Action();
                            gameData.UseSkills4Action();
                            Ski4 = false;
                        }
                        else
                        {
                            UnityEngine.Debug.Log("Spikes");
                            //No effect
                        }



                    }
                    else if (dicCrystal.ContainsKey(transferCharacter))
                    {


                        TileViewItem Crystal = (TileViewItem)dicCrystal[transferCharacter];

                        gameData.AddCrystal(Crystal.keyID);
                        gameData.GetNumActiveCrystal();

                        DestoryTile(Crystal.keyID, Crystal.posID);
                        // Move
                        curCharacter.Move(dir + dir);
                        //Record Move
                        ActionRecordData characterAction = new(-2, startCharacterPosID, transferCharacter);
                        listAllAction.Add(characterAction);

                        DestoryStateRecordData CrystalAction = new(Crystal.keyID, transferCharacter, TileType.Crystal);
                        listAllAction.Add(CrystalAction);


                        gameData.UseSkills4Action();
                        Ski4 = false;



                    }
                    else if (dicRedDoor.ContainsKey(transferCharacter))
                    {
                        // DoorViewItem redDoor = (DoorViewItem)dicRedDoor[targetPosCharacter];
                        // if(redDoor.IsOpen())
                        if (dicRedDoor[transferCharacter].DoorIsOpened)
                        {
                            // UnityEngine.Debug.Log("RedDoorisopen");
                            //Move
                            curCharacter.Move(dir + dir);
                            //Record Move
                            ActionRecordData characterAction = new(-2, startCharacterPosID, transferCharacter);
                            listAllAction.Add(characterAction);

                            gameData.UseSkills4Action();
                            Ski4 = false;
                        }
                        else
                        {
                            //UnityEngine.Debug.Log("RedDoorisclose");

                            //No effect
                        }

                    }

                    else if (dicBlueDoor.ContainsKey(transferCharacter))
                    {
                        //UnityEngine.Debug.Log("BlueDoor");
                        if (dicBlueDoor[transferCharacter].DoorIsOpened)
                        {
                            // UnityEngine.Debug.Log("BlueDoorisopen");
                            //Move
                            curCharacter.Move(dir + dir);
                            //Record Move
                            ActionRecordData characterAction = new(-2, startCharacterPosID, transferCharacter);
                            listAllAction.Add(characterAction);
                            gameData.UseSkills4Action();
                            Ski4 = false;
                        }
                        else
                        {
                            //UnityEngine.Debug.Log("BlueDoorisclose");
                            //No effect
                        }

                    }

                    else if (dicButton.ContainsKey(transferCharacter))
                    {
                        UnityEngine.Debug.Log("Button");
                        //Move
                        curCharacter.Move(dir + dir);
                        //Record Move
                        ActionRecordData characterAction = new(-2, startCharacterPosID, transferCharacter);
                        listAllAction.Add(characterAction);
                        gameData.UseSkills4Action();
                        Ski4 = false;

                    }

                    //Empty
                    else
                    {

                        //Move
                        curCharacter.Move(dir + dir);
                        //Record Move
                        ActionRecordData characterAction = new(-2, startCharacterPosID, transferCharacter);
                        listAllAction.Add(characterAction);
                        //close skill4


                        gameData.UseSkills4Action();
                        Ski4 = false;



                    }



                }



            }

            else if (dicWall.ContainsKey(targetPosCharacter))
            {

                UnityEngine.Debug.Log("Wall");
                //No effect


            }

            else if (dicSceneChange.ContainsKey(targetPosCharacter))
            {
                UnityEngine.Debug.Log("SceneChange");

                ChangeScenceViewItem SceneChange = (ChangeScenceViewItem)dicSceneChange[targetPosCharacter];
                DestoryTile(SceneChange.keyID, SceneChange.posID);
                SceneChange.ChangeScence();
                gameData.SaveLevelData();



            }

            else if (dicGiveupSkills.ContainsKey(targetPosCharacter))
            {
                UnityEngine.Debug.Log("GivingUpSkillsViewItem");

                GivingUpSkillsViewItem GiveupSkills = (GivingUpSkillsViewItem)dicGiveupSkills[targetPosCharacter];
                DestoryTile(GiveupSkills.keyID, GiveupSkills.posID);
                GiveupSkills.OpenGiveupSkillsPage();
                gameData.SaveLevelData();
            }



            else if (dicBox.ContainsKey(targetPosCharacter))
            {
                UnityEngine.Debug.Log("Box");

                //Check whether this block is box
                TileViewItem box = (TileViewItem)dicBox[targetPosCharacter];
                Vector2Int targetPosBox = box.posID + dir;


                if (!dicBox.ContainsKey(targetPosBox) && !dicIce.ContainsKey(targetPosBox) && !dicCrystal.ContainsKey(targetPosBox)
                    && !dicWall.ContainsKey(targetPosBox)
                    && (!dicSpikes.ContainsKey(box.posID) || (dicSpikes.ContainsKey(box.posID) && Ski2)))
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
                            //UnityEngine.Debug.Log("Box&BlueDoor");
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



                CheckButtonState();
            }

            // pull box
            else if (dicBox.ContainsKey(startCharacterPosID - dir) && Ski3)
            {
                UnityEngine.Debug.Log("PullBox");

                //Check whether this block is box
                TileViewItem box = (TileViewItem)dicBox[startCharacterPosID - dir];



                if (!dicBox.ContainsKey(targetPosCharacter) && !dicIce.ContainsKey(targetPosCharacter) && !dicCrystal.ContainsKey(targetPosCharacter) && !dicWall.ContainsKey(targetPosCharacter))
                {
                    gameData.UseSkills3Action();
                    if (dicTraps.ContainsKey(targetPosCharacter))
                    {
                        TrapsViemItem Traps = (TrapsViemItem)dicTraps[targetPosCharacter];

                        if (!Traps.TrapsIsFilled)
                        {
                            UnityEngine.Debug.Log("TrapsIsUnFilled");

                        }
                        else
                        {


                            curCharacter.Move(dir);
                            box.Move(dir);

                            ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                            listAllAction.Add(characterAction);

                            ActionRecordData boxAction = new(box.keyID, box.posID, box.posID + dir);
                            listAllAction.Add(boxAction);
                            Ski3 = false;
                        }



                    }
                    else if (dicRedDoor.ContainsKey(targetPosCharacter))
                    {
                        if (dicRedDoor[targetPosCharacter].DoorIsOpened)
                        {

                            ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                            listAllAction.Add(characterAction);

                            ActionRecordData boxAction = new(box.keyID, box.posID, box.posID + dir);
                            listAllAction.Add(boxAction);

                            curCharacter.Move(dir);
                            box.Move(dir);
                            Ski3 = false;
                        }
                        else
                        {
                            UnityEngine.Debug.Log("Can't go through reddoor");
                        }


                    }
                    else if (dicBlueDoor.ContainsKey(targetPosCharacter))
                    {
                        if (dicBlueDoor[targetPosCharacter].DoorIsOpened)
                        {

                            ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                            listAllAction.Add(characterAction);

                            ActionRecordData boxAction = new(box.keyID, box.posID, box.posID + dir);
                            listAllAction.Add(boxAction);

                            curCharacter.Move(dir);
                            box.Move(dir);
                            Ski3 = false;
                        }
                        else
                        {
                            UnityEngine.Debug.Log("Can't go through bluedoor");
                        }


                    }
                    else if (dicSpikes.ContainsKey(targetPosCharacter))
                    {
                        if (Ski2)
                        {
                            ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                            listAllAction.Add(characterAction);

                            ActionRecordData boxAction = new(box.keyID, box.posID, box.posID + dir);
                            listAllAction.Add(boxAction);

                            curCharacter.Move(dir);
                            box.Move(dir);
                            Ski3 = false;
                            gameData.UseSkills2Action();
                        }
                        else
                        {
                            UnityEngine.Debug.Log("Spikes");
                            //No effect
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
                        Ski3 = false;
                    }
                }



                CheckButtonState();


            }

            else if (dicSpikes.ContainsKey(targetPosCharacter))
            {

                if (Ski2)
                {
                    curCharacter.Move(dir);
                    //Record Move
                    ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                    listAllAction.Add(characterAction);
                    gameData.UseSkills2Action();
                }
                else
                {
                    UnityEngine.Debug.Log("Spikes");
                    //No effect
                }



            }

            else if (dicCrystal.ContainsKey(targetPosCharacter))
            {


                TileViewItem Crystal = (TileViewItem)dicCrystal[targetPosCharacter];

                gameData.AddCrystal(Crystal.keyID);
                gameData.GetNumActiveCrystal();

                DestoryTile(Crystal.keyID, Crystal.posID);
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
                //UnityEngine.Debug.Log("Ice");
                IceViewItem Ice = (IceViewItem)dicIce[targetPosCharacter];
                //No effect
                if (Ski1)
                {
                    gameData.UseSkills1Action();
                    if (Ice.iceIsCracked)
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
                else
                {
                    UnityEngine.Debug.Log("Ice");
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
                    //UnityEngine.Debug.Log("RedDoorisopen");
                    //Move
                    curCharacter.Move(dir);
                    //Record Move
                    ActionRecordData characterAction = new(-2, startCharacterPosID, targetPosCharacter);
                    listAllAction.Add(characterAction);
                }
                else
                {
                    //UnityEngine.Debug.Log("RedDoorisclose");

                    //No effect
                }

            }

            else if (dicBlueDoor.ContainsKey(targetPosCharacter))
            {
                //UnityEngine.Debug.Log("BlueDoor");
                if (dicBlueDoor[targetPosCharacter].DoorIsOpened)
                {
                    //UnityEngine.Debug.Log("BlueDoorisopen");
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
            ScanAllPos(false);
            
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



        ScanAllPos(false);
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
                
                RegenerateTile(IcePrefab, recordData.PosID,recordData.keyID);

                ScanAllPos(false);
                Ice = (IceViewItem)dicIce[recordData.PosID];
                Ice.iceIsCracked = true;
                //Ice.IsDestroyed = false;

            }






        }
        else if(recordData.tileType == TileType.Box)
        {
            
            RegenerateTile(BoxPrefab, recordData.PosID,recordData.keyID);
            
            UnitViewItem Box = dicBox[recordData.PosID];
            
            //Box.IsDestroyed = false;
           
            Box.MoveToTarPos(recordData.PosID);


        }
        else if (recordData.tileType == TileType.Crystal)
        {
            
            RegenerateTile(CrystalPrefab, recordData.PosID, recordData.keyID);
            TileViewItem Crystal = (TileViewItem)dicCrystal[recordData.PosID];
            //Crystal.IsDestroyed = false;

            //reduce the number of crystal
            gameData.ReduceCrystal(Crystal.keyID);
            gameData.GetNumActiveCrystal();
            //UnityEngine.Debug.Log("the number of crystal is" + gameData.GetNumActiveCrystal());
            //scoreText.text = "Crystal：" + gameData.GetNumActiveCrystal();




        }
        else if (recordData.tileType == TileType.Traps)
        {

            TrapsViemItem Traps = (TrapsViemItem)dicTraps[recordData.PosID];
            Traps.TrapsIsFilled = false;
            //UnityEngine.Debug.Log("Traps.TrapsIsFilled" + Traps.TrapsIsFilled);

        }
        
        CheckButtonState();
        ScanAllPos(false);

    }


   

    private void IceBreakingEvent(object arg0)
    {
        if (gameData.SkillPoint1 == 1)
        {
            Ski1 = true;
            UnityEngine.Debug.Log("IceBreakingSki1" + Ski1);
        }
        else
        {
            Ski1 = false;
        }
           
        
        
    }

    private void ThroughSpikesEvent(object arg0)
    {
        if (gameData.SkillPoint2 == 3)
        {
            Ski2 = true;
            UnityEngine.Debug.Log("ThroughSpikesSki2" + Ski2);
        }
        else
        {
            Ski2 = false;
        }
        
       
    }

    private void PullBoxEvent(object arg0)
    {
        if (gameData.SkillPoint3 ==  5)
        {
            Ski3 = true;
            UnityEngine.Debug.Log("PullBoxSki3" + Ski3);
        }
        else
        {
            Ski3 = false;
        }

       

    }

    private void TeleportatioEvent(object arg0)
    {
        if (gameData.SkillPoint4 ==  7)
        {
            Ski4 = true;
            UnityEngine.Debug.Log("TeleportatioSki4" + Ski4);
        }
        else
        {
            Ski4 = false;
        }
        
    }

    #endregion

    #region DestoryTile
    public void DestoryTile(int keyID,Vector2Int posID)
    {
        
            if (dicAllTile.ContainsKey(keyID))
            {
            UnityEngine.Debug.Log("DestoryTile");

            TileViewItem tileViewItem = dicAllTile[keyID];
            //tileViewItem.IsDestroyed = true;       

            listTile.Remove(tileViewItem);
            dicAllTile.Remove(keyID);
            Destroy(tileViewItem.gameObject);

            ScanAllPos(false);
            }
            else
            {
            // 处理键不存在的情况
            UnityEngine.Debug.LogError("Key " + keyID + " not found in the dictionary.");
            }

            
        

        


    }
    #endregion

    public void RegenerateTile(GameObject Prefab, Vector2Int posID, int keyID)
    {
        GameObject  newObject = Instantiate(Prefab, new Vector3(posID.x, posID.y, 0), Quaternion.identity);
       
        newObject.transform.parent = tfTile;
        Vector2Int crystalPos = posID;
       int crystalKeyID = keyID;
        Vector2Int icePos = posID;
        int iceKeyID = keyID;
        Vector2Int boxPos = posID;
        int boxKeyID = keyID;



        if (Prefab == BoxPrefab)
        {

            TileViewItem BoxTileItem = newObject.GetComponent<TileViewItem>();
            BoxTileItem.posID = boxPos;
            BoxTileItem.keyID = boxKeyID;
            listTile.Add(BoxTileItem);
            dicBox.Add(BoxTileItem.posID, BoxTileItem);
            dicAllTile.Add(boxKeyID, BoxTileItem);
        }
        else if (Prefab == CrystalPrefab)
        {
            TileViewItem CrystalItem = newObject.GetComponent<TileViewItem>();
            CrystalItem.posID = crystalPos;
            CrystalItem.keyID = crystalKeyID;
            listTile.Add(CrystalItem);
            dicCrystal.Add(CrystalItem.posID, CrystalItem);
            dicAllTile.Add(CrystalItem.keyID, CrystalItem);
        }
        else if (Prefab == IcePrefab)
        {
            IceViewItem IceTileItem = newObject.GetComponent<IceViewItem>();
            IceTileItem.posID = icePos;
            IceTileItem.keyID = iceKeyID;
            listTile.Add(IceTileItem);
            dicIce.Add(IceTileItem.posID, (IceViewItem)IceTileItem);
            dicAllTile.Add(IceTileItem.keyID, IceTileItem);
        }

        //TileViewItem newTileItem = newObject.GetComponent<TileViewItem>();
       // newTileItem.posID = posID;


        //listTile.Add(newTileItem);
        ScanAllPos(false);

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

    public void ScanAllPos(bool whetherInital)
    {

        if (whetherInital)
        {
            ScanInitalPos();
        }
        else 
        {
            ScanUpadatePos();
        }


    }

    private void ScanInitalPos()
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
        dicGiveupSkills.Clear();



        keyID = -1;
        foreach (var tile in listTile)
        {
            //Only record when map start
            keyID++;
            int levelKeyID = int.Parse(levelID.ToString() + keyID.ToString());
            tile.keyID = levelKeyID;

            //Pos Refresh Every time
            dicAllTile.Add(levelKeyID, tile);
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
                case TileType.GivingUpSkills:
                    dicGiveupSkills.Add(tile.posID, tile);
                    break ;
            }
        }
    }

    private void ScanUpadatePos()
    {

        
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
        dicGiveupSkills.Clear();



        
        foreach (var tile in listTile)
        {
            

            //Pos Refresh Every time
            
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
                case TileType.GivingUpSkills:
                    dicGiveupSkills.Add(tile.posID, tile);
                    break;
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

        ScanAllPos(false);
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
        dicGiveupSkills.Clear();
    }

}

