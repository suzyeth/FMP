using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    //public Rigidbody rd;    //public或者private（接口）
    //public int score = 0;   //分数初值
    public Text scoreText;  //定义分数UI
    //public GameObject winText;  //将胜利的UI定位为游戏物体（默认不显示，结束后显示）
    private GameData gameData;

    //SkillList
    public Image Bar1;
    public Image Bar2;
    public Image Bar3;
    public Image Bar4;
    public Text Bar1Text;
    public Text Bar2Text;
    public Text Bar3Text;
    public Text Bar4Text;
    public float SK1Amount, TargetSK1Amount;
    public float SK2Amount, TargetSK2Amount;
    public float health3, health33;
    public float health4, health44;

    private const float maxHealth1 = 1f;
    private const float maxHealth2 = 3f;
    private const float maxHealth3 = 5f;
    private const float maxHealth4 = 7f;
    


    private float CurrentSkillBar1Amount=0f;
    private float CurrentSkillBar2Amount = 0f;
    private float CurrentSkillBar3Amount = 0f;
    private float CurrentSkillBar4Amount = 0f;

    

    //display UpSkillsImage
    public GameObject SkillImage1Active;
    public GameObject SkillImage2Active;
    public GameObject SkillImage3Active;
    public GameObject SkillImage4Active;
    public GameObject SkillImage1Submitted;
    public GameObject SkillImage2Submitted;
    public GameObject SkillImage3Submitted;
    public GameObject SkillImage4Submitted;


    //display craystal amount
    public Image CrystalBar;
    public float CrystalAmount;
    public float CurrentCrystalAmount;
    private const float maxCrystalAmount = 10f;

    public float AllPoint=0f;





    public bool intial = false;
    

    public GameObject SkillListPage;
    public GameObject PanelPage;
    public GameObject SettingPage;

    public bool Skill1 = false;
    public bool Skill2 = false;
    public bool Skill3 = false;
    public bool Skill4 = false;


    // Start is called before the first frame update
    void Start()
    {
        gameData = PublicTool.GetGameData();
        SkillListPage.SetActive(false);
        PanelPage.SetActive(false);
        SettingPage.SetActive(false);
        CrystalAmount = 0;



    }

    // Update is called once per frame
    void Update()
    {

        BarFiller();
       


    }

    #region Event

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener("UseSkills", UseSkillsEvent);
       

    }

    private void OnDisable()
    {
        
        EventCenter.Instance.RemoveEventListener("UseSkills", UseSkillsEvent);

    }

    private void UseSkillsEvent(object arg0)
    {
        ResetSkillPoints();
        UnityEngine.Debug.Log("USEDSKILLS" );
        ActiveUpSkillImage();
    }
    #endregion



    /* private void RefreshAddPoint()
     {
       //退回存档之前的加点

         int currentSkillPoint1 = gameData.SkillPoint1; // Get the current value
         int currentSkillPoint2 = gameData.SkillPoint2;
         int currentSkillPoint3 = gameData.SkillPoint3;
         int currentSkillPoint4 = gameData.SkillPoint4;
         float currentAllPoint = gameData.SkillAllPonit;
         RefreshSkillBar1(currentSkillPoint1);
         RefreshSkillBar2(currentSkillPoint2);
         RefreshSkillBar3(currentSkillPoint3);
         RefreshSkillBar4(currentSkillPoint4);


         //gameData.SkillPoint1 = 10; // Set a new value
     }*/






    //SKILL LIST PAGE
    #region AddPoint
    private void BarFiller()

    {
        RefreshSkillBar4(health44);
        if (CurrentSkillBar1Amount!= TargetSK1Amount)
        {
            CurrentSkillBar1Amount = TargetSK1Amount;
            
            RefreshSkillBar1(TargetSK1Amount);
            RefreshCrystalBar(CrystalAmount);

        }
        if (CurrentSkillBar2Amount != TargetSK2Amount)
        {
            CurrentSkillBar2Amount= TargetSK2Amount;
            RefreshSkillBar2(TargetSK2Amount);
            RefreshCrystalBar(CrystalAmount);

            
        }
        if (CurrentSkillBar3Amount!= health33)
        {
            CurrentSkillBar3Amount = health33;
            RefreshSkillBar3(health33);
            RefreshCrystalBar(CrystalAmount);
           
            
            
        }
        if (CurrentSkillBar4Amount != health44)
        {
            CurrentSkillBar4Amount = health44;
            RefreshSkillBar4(health44);
            RefreshCrystalBar(CrystalAmount);
           
//           

        }
        if (!intial)
        {
            
            

            gameData.SkillPoint1=0;
            gameData.SkillPoint2=0;
            gameData.SkillPoint3=0;
            gameData.SkillPoint4=0;
            gameData.SkillAllPonit = 0;

            RefreshCrystalBar(CrystalAmount);
            RefreshSkillBar4(health44);
            RefreshSkillBar3(health33);
            RefreshSkillBar2(TargetSK2Amount);
            RefreshSkillBar1(TargetSK1Amount);
            intial = true;

        }
        if (CurrentCrystalAmount != gameData.GetNumActiveCrystal())
        {
            ResetSkillPoints();
            //   Identify players picking up crystals in the game
            CrystalAmount =gameData.GetNumActiveCrystal()+AllPoint;
            CurrentCrystalAmount = gameData.GetNumActiveCrystal();
            RefreshCrystalBar(CrystalAmount);
     
        }
        
    }


    private void RefreshSkillBar1(float amount)
    {
        Bar1.fillAmount = amount / maxHealth1;
        Bar1Text.text = amount + "/" + maxHealth1;
    }

    private void RefreshSkillBar2(float amount)
    {
        Bar2.fillAmount = amount / maxHealth2;
        Bar2Text.text = amount+ "/" + maxHealth2;
    }

    private void RefreshSkillBar3(float amount)
    {
        Bar3.fillAmount = amount / maxHealth3;
        Bar3Text.text = amount + "/" + maxHealth3;
    }

    private void RefreshSkillBar4(float amount)
    {
        Bar4.fillAmount = amount / maxHealth4;
        Bar4Text.text = amount  + "/" + maxHealth4 ;
    }

    private void RefreshCrystalBar(float amount)
    {
        
        CrystalBar.fillAmount = amount / maxCrystalAmount;
        scoreText.text = amount + "/" + maxCrystalAmount;
        
    }


    public void AddHealth1()
    {
        if (TargetSK1Amount < maxHealth1 && CrystalAmount>=1)
        {

            TargetSK1Amount += 1;
            //Debug.Log(health1);
            GetAllPoint(-1f);
            
        }


    }
    public void ReduceHealth1()
    {
        if (TargetSK1Amount > 0 && TargetSK1Amount > gameData.SkillPoint1)
        {
            TargetSK1Amount -= 1;
            GetAllPoint(+1f);
            
        }


    }

    public void AddHealth2()
    {
        if (TargetSK2Amount < maxHealth2 && CrystalAmount >= 1)
        {
            TargetSK2Amount += 1;
            GetAllPoint(-1f);
            Debug.Log(TargetSK2Amount);
        }


    }
    public void ReduceHealth2()
    {
        if (TargetSK2Amount > 0 && TargetSK2Amount > gameData.SkillPoint2)
        {
            TargetSK2Amount -= 1;
            GetAllPoint(+1f);
        }


    }

    public void AddHealth3()
    {

        if (health33 < maxHealth3 && CrystalAmount >= 1)
        {
            health33 += 1;
            GetAllPoint(-1f);
        }


    }
    public void ReduceHealth3()
    {
        if (health33 > 0 && health33 > gameData.SkillPoint3)
        {
            health33 -= 1;
            GetAllPoint(+1f);
        }


    }

    public void AddHealth4()
    {
        if (health44 < maxHealth4 && CrystalAmount >= 1)
        {
            health44 += 1;
            GetAllPoint(-1f);
        }



    }
    public void ReduceHealth4()
    {
        if (health44 > 0 && health44 > gameData.SkillPoint4)
        {
            health44 -= 1;
            GetAllPoint(+1f);
        }



    }


    private float GetAllPoint(float point)
    {
        AllPoint = AllPoint + point;
        CrystalAmount = gameData.GetNumActiveCrystal() + AllPoint;
        Debug.Log("CrystalAmount" + CrystalAmount);
        return CrystalAmount;
    }


    public void SLPConfirmButton()
    {
        gameData.SkillPoint1 = (int)TargetSK1Amount;
        gameData.SkillPoint2 = (int)TargetSK2Amount;
        gameData.SkillPoint3 = (int)health33;
        gameData.SkillPoint4 = (int)health44;
        gameData.SkillAllPonit = AllPoint;

        ActiveUpSkillImage();
    }

    public void SLPResetButton()
    {
        ResetSkillPoints();
    }

    public void ResetSkillPoints()
    {
        TargetSK1Amount = gameData.SkillPoint1;
        TargetSK2Amount = gameData.SkillPoint2;
        health33 = gameData.SkillPoint3;
        health44 = gameData.SkillPoint4;



        AllPoint = gameData.SkillAllPonit;
        CrystalAmount = gameData.GetNumActiveCrystal() + AllPoint;
    

    }



    #endregion

   

    #region OpenClosePage

    public void OpenSkillListPage()
    {
        if (!SettingPage.activeSelf)
        {
            SkillListPage.SetActive(true);

            //RefreshAddPoint();
            

            PanelPage.SetActive(false);
        }

    }
    public void OpenPanelPage()
    {
        SkillListPage.SetActive(false);
        PanelPage.SetActive(true);
        ResetSkillPoints();
    }

    public void CloseMainPanel()
    {
        SkillListPage.SetActive(false);
        PanelPage.SetActive(false);
        ResetSkillPoints();
    }

    public void PanelButton()
    {
        if (!SkillListPage.activeSelf)
        {
            SkillListPage.SetActive(true);
        }


    }

    public void CloseSettingPage()
    {
        SettingPage.SetActive(false);
    }
    public void OpenSettingPage()
    {
        if (!SkillListPage.activeSelf && !PanelPage.activeSelf)
        {
            SettingPage.SetActive(true);
        }

    }

    #endregion



    #region GiveUpSkills
    public void GiveUpSkill1()
    {
        Skill1 = true;
        Skill2 = false;
        Skill3 = false;
        Skill4 = false;
        SkillImage1Submitted.SetActive(true);
    }

    public void GiveUpSkill2()
    {
        Skill2 = true;
        Skill1 = false;
        Skill3 = false;
        Skill4 = false;
        SkillImage2Submitted.SetActive(true);
    }

    public void GiveUpSkill3()
    {
        Skill3 = true;
        Skill1 = false;
        Skill2 = false;
        Skill4 = false;
        SkillImage3Submitted.SetActive(true);
    }

    public void GiveUpSkill4()
    {
        Skill4 = true;
        Skill1 = false;
        Skill2 = false;
        Skill3 = false;
        SkillImage4Submitted.SetActive(true);
    }

    #endregion

    #region ActiveUpSkillsImage
    private void ActiveUpSkillImage()
    {
        if (gameData.SkillPoint1 == maxHealth1)
        { SkillImage1Active.SetActive(true); }
        else
        { SkillImage1Active.SetActive(false); }

        if (gameData.SkillPoint2 == maxHealth2)
        { SkillImage2Active.SetActive(true); }
        else
        { SkillImage2Active.SetActive(false); }

        if (gameData.SkillPoint3 == maxHealth3)
        {SkillImage3Active.SetActive(true);}
        else
        { SkillImage3Active.SetActive(false);}

        if (gameData.SkillPoint4 == maxHealth4)
        {SkillImage4Active.SetActive(true);}
        else
        {SkillImage4Active.SetActive(false);}
       


    }


    #endregion


    public void OnExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void UndoButton()
    {
        PublicTool.GetGameData().LoadLevelData();
        GameMgr.Instance.levelMgr.RestartThisMap();
    }



}





