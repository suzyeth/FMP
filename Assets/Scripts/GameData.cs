using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Playables;

public class BaseRecordData
{
    public int keyID;
}

public class ActionRecordData : BaseRecordData
{
    //Define that keyID=-2 mean character
    public Vector2Int startPos;
    public Vector2Int endPos;
    

    public ActionRecordData(int keyID, Vector2Int startPos, Vector2Int endPos)
    {

        this.keyID = keyID;
        this.startPos = startPos;
        this.endPos = endPos;
        
    }


}

public class DestoryStateRecordData : BaseRecordData
{
    //Define that keyID=-2 mean character
    public Vector2Int PosID;
    public TileType tileType;


    public DestoryStateRecordData(int keyID, Vector2Int PosID, TileType tileType)
    {

        this.keyID = keyID;
        this.PosID = PosID;
        this.tileType=tileType;
        

    }
}

public class GameData
{
    //Init game data
    public GameData()
    {
        energy = 0;
        dicCrystal.Clear();

        skillPoint1 = 0;
        skillPoint2 = 0;
        skillPoint3 = 0;
        skillPoint4 = 0;
    }

    
    #region data destroyedTiles
    public Dictionary<int, bool> destroyedTiles = new Dictionary<int, bool>();

    public void RecordDestroyedTile(int keyID)
    {
        destroyedTiles[keyID] = true;
    }

    public bool IsTileDestroyed(int keyID)
    {
        return destroyedTiles.ContainsKey(keyID) && destroyedTiles[keyID];
    }

    #endregion


    #region Crystal
    public int energy = 0;
    public Dictionary<int, Crystal> dicCrystal = new Dictionary<int, Crystal>();
    public void AddCrystal(int id)
    {
        if (!dicCrystal.ContainsKey(id))
         {
             Crystal crystal = new Crystal(id);
             dicCrystal.Add(id, crystal);


         }
        

    }

    public void ReduceCrystal(int id)
    {
        if (dicCrystal.ContainsKey(id))
        {
            Crystal crystal = new Crystal(id);
            dicCrystal.Remove(id);
            UnityEngine.Debug.Log("GetCrystal" );
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
        return dicCrystal.Count;
        //int count = 0;
        //foreach(var item in dicCrystal)
        {
            //if (!item.Value.isSubmitted)
            //{
            //    count++;
            //}
        }
        //return count;

       


    }
    #endregion

    #region Undo

    private Stack<List<BaseRecordData>> stackActionRecord = new Stack<List<BaseRecordData>>();

    public void AddRecordAction(List<BaseRecordData> listAction)
    {
        stackActionRecord.Push(listAction);
    }

    public void UndoAction()
    {
        if (stackActionRecord.Count > 0)
        {
            List<BaseRecordData> listUndoAction = stackActionRecord.Pop();


            if (listUndoAction != null)
            {
                foreach (var action in listUndoAction)
                {
                    if (action.GetType() == typeof(DestoryStateRecordData))
                    {
                        EventCenter.Instance.EventTrigger("UndoDestroy", action);
                        
                    }
                    else if(action.GetType() == typeof(ActionRecordData))
                    {
                        EventCenter.Instance.EventTrigger("Undo", action);
                    }
                    

                }
            }
        }
    }

    #endregion

    #region SkillPoints

    private int skillPoint1;
    private int skillPoint2;
    private int skillPoint3;
    private int skillPoint4;
    private float skillAllPonit;

    public int SkillPoint1
    {
        get { return skillPoint1; }
        set { skillPoint1 = value; }
    }

    public int SkillPoint2
    {
        get { return skillPoint2; }
        set { skillPoint2 = value; }
    }

    public int SkillPoint3
    {
        get { return skillPoint3; }
        set { skillPoint3 = value; }
    }

    public int SkillPoint4
    {
        get { return skillPoint4; }
        set { skillPoint4 = value; }
    }

    public float SkillAllPonit
    {
        get { return skillAllPonit; }
        set { skillAllPonit = value; }
    }

    #endregion

    #region UseSkills
    private bool Ski1Used=false;
    private bool Ski2Used=false;
    private bool Ski3Used=false;
    private bool Ski4Used=false;

    public void UseSkills1Action()
    {
        //SkillPoint1 = 0;
        Ski1Used = true;
        //EventCenter.Instance.EventTrigger("UseSkills", 1);
    }
    public void UseSkills2Action()
    {
        //SkillPoint2 = 0;
        Ski2Used = true;
        //EventCenter.Instance.EventTrigger("UseSkills", 1);
    }
    public void UseSkills3Action()
    {
        //SkillPoint3 = 0;
        Ski3Used=true;
        //EventCenter.Instance.EventTrigger("UseSkills", 1);
    }
    public void UseSkills4Action()
    {
        //SkillPoint4 = 0;
        Ski4Used = true;
       // EventCenter.Instance.EventTrigger("UseSkills", 1);
    }

    #endregion

    #region SaveLevelData
    private int OldSkillPoint1;
    private int OldSkillPoint2;
    private int OldSkillPoint3;
    private int OldSkillPoint4;
    private float OldSkillAllPonit;
    public void SaveLevelData()
    {
        //使用技能之后等到下一关刷新在重新刷新技能表
        if (Ski1Used)
        {
            SkillPoint1 = 0;
            Ski1Used= false;
        }
        if (Ski2Used)
        {
            SkillPoint2 = 0;
            Ski2Used= false;
        }
        if (Ski3Used)
        {
            SkillPoint3 = 0;
            Ski3Used= false;
        }
        if (Ski4Used)
        {
            SkillPoint4 = 0;
            Ski4Used= false;
        }
        
        EventCenter.Instance.EventTrigger("UseSkills", 1);

        //记录关卡初始内容：水晶数量，allpoint,skilllpoint
        OldSkillPoint1 = skillPoint1;
        OldSkillPoint2 = skillPoint2;
        OldSkillPoint3 = skillPoint3;
        OldSkillPoint4 = skillPoint4;
        OldSkillAllPonit=skillAllPonit;

    }
    public void LoadLevelData()
    {
        //加载关卡初始内容：
        skillPoint1 = OldSkillPoint1  ;
        skillPoint2 = OldSkillPoint2  ;
        skillPoint3 = OldSkillPoint3  ;
        skillPoint4 = OldSkillPoint4  ;
        skillAllPonit = OldSkillAllPonit  ;
        EventCenter.Instance.EventTrigger("UseSkills", 1);
    }
    #endregion
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