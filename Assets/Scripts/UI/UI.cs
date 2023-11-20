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

    public Text CurrentLevelText;

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

    //display PanelPage Skills Image
    public GameObject PPSkillImage1Active;
    public GameObject PPSkillImage2Active;
    public GameObject PPSkillImage3Active;
    public GameObject PPSkillImage4Active;
    public GameObject PPSkillImage1Submitted;
    public GameObject PPSkillImage2Submitted;
    public GameObject PPSkillImage3Submitted;
    public GameObject PPSkillImage4Submitted;

    #region SettingPage
    //SettingPage
    public GameObject LevelPage;
    public GameObject MusicPage;
    public GameObject ControlPage;
    public GameObject CreditsPage;

    #endregion

    //display craystal amount
    public Image CrystalBar;
    public float CrystalAmount;
    public float CurrentCrystalAmount;
    private const float maxCrystalAmount = 20f;

    public float AllPoint=0f;


    public bool intial = false;
    

    public GameObject SkillListPage;
    public GameObject PanelPage;
    public GameObject SettingPage;

    //GivingUpPage
    public GameObject GivesUpSkillsPage;
    public GameObject CloseButton1;
    public GameObject GUSSkillButton1;
    public GameObject GUSSkillButton2;
    public GameObject GUSSkillButton3;
    public GameObject GUSSkillButton4;
    public GameObject GUSSkillSubmitted1;
    public GameObject GUSSkillSubmitted2;
    public GameObject GUSSkillSubmitted3;
    public GameObject GUSSkillSubmitted4;
    public GameObject GUSUnableConfirmButton;
    public GameObject GUSActiveConfirmButton;

    //public GameObject CloseButton2;
    //public GameObject SelectAllSkills;
    //public GameObject AllSkillsImage; 
    private bool OpenGiveUpSkillsPage=false;


    public bool GUSSkill1 = false;
    public bool GUSSkill2 = false;
    public bool GUSSkill3 = false;
    public bool GUSSkill4 = false;

    public GameObject EphemeralUI;


    // Start is called before the first frame update
    void Start()
    {

        gameData = PublicTool.GetGameData();
        SkillListPage.SetActive(false);
        PanelPage.SetActive(false);
        SettingPage.SetActive(false);
        CrystalAmount = 0;

        EphemeralUI.SetActive(false);

        

    }

    // Update is called once per frame
    void Update()
    {

        BarFiller();
       


    }


    private void InitCheck()
    {
        GUSSkillButton1.SetActive(false);
        GUSSkillButton2.SetActive(false);
        GUSSkillButton3.SetActive(false);
        GUSSkillButton4.SetActive(false);
    
    }

    #region Event

    private void OnEnable()
    {
       
        EventCenter.Instance.AddEventListener("UseSkills", UseSkillsEvent);
        EventCenter.Instance.AddEventListener("ChangeLevelText", ChangeLevelTextEvent);
        EventCenter.Instance.AddEventListener("PartEnd", PartEndEvent);



    }

    private void OnDisable()
    {
        
        EventCenter.Instance.RemoveEventListener("UseSkills", UseSkillsEvent);
        EventCenter.Instance.RemoveEventListener("ChangeLevelText", ChangeLevelTextEvent);
        EventCenter.Instance.RemoveEventListener("PartEnd", PartEndEvent);

    }

    private void UseSkillsEvent(object arg0)
    {
        ResetSkillPoints();
        UnityEngine.Debug.Log("USEDSKILLS" );
        ActiveUpSkillImage();
    }

    private void ChangeLevelTextEvent(object arg0)
    {
        int id = GameMgr.Instance.levelMgr.CurrentMapID();
        if (id == 1)
        {
            EphemeralUI.SetActive(true);
        }
        CurrentLevelText.text = ("LEVEL  " + id);


    }

    private void PartEndEvent(object arg0)
    {

        OpenGiveingupSkillsPage();
        OpenGiveUpSkillsPage = true;


    }
    #endregion



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

            GivesUpSkillsPage.SetActive(false);
            PanelPage.SetActive(false);
        }

    }
    public void OpenPanelPage()
    {
        if (!OpenGiveUpSkillsPage)
        {
            SkillListPage.SetActive(false);
            PanelPage.SetActive(true);
            //ResetSkillPoints();
        }
        else
        {
            SkillListPage.SetActive(false);
            OpenGiveingupSkillsPage();

        }
        
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

    public void OpenLevelPage()
    {
        LevelPage.SetActive(true);
        SettingPage.SetActive(false);
    }

    public void CloseLevelPage()
    {
        LevelPage.SetActive(false);
        SettingPage.SetActive(true);
    }

    public void OpenMusicPage()
    {
        MusicPage.SetActive(true);
        SettingPage.SetActive(false);
    }

    public void CloseMusicPage()
    {
        MusicPage.SetActive(false);
        SettingPage.SetActive(true);
    }

    public void OpenControlPage()
    {
        ControlPage.SetActive(true);
        SettingPage.SetActive(false);
    }

    public void CloseControlPage()
    {
        ControlPage.SetActive(false);
        SettingPage.SetActive(true);
    }

    public void OpenCreditsPage()
    { 
        CreditsPage.SetActive(true);
        SettingPage.SetActive(false);

    }

    public void CloseCreditsPage()
    {
        CreditsPage.SetActive(false);
        SettingPage.SetActive(true);

    }

    public void OpenGiveingupSkillsPage()
    {
        GivesUpSkillsPage.SetActive(true);
        CloseButton1.SetActive(false);
       

    }

    public void CloseGiveingupSkillsPage()
    {
        GivesUpSkillsPage.SetActive(false);
        CloseButton1.SetActive(true);
        //CloseButton2.SetActive(true);
        GameMgr.Instance.levelMgr.ChangeMap();
        
    }

    #endregion

  
    
    #region ActiveUpSkillsImage
   //when fill up skills Active Image(Up ui & panel page for display which skills are SUbmitted &)
    private void ActiveUpSkillImage()
    {
       
        if (gameData.SkillPoint1 == maxHealth1)
        { 
            //UP ui
            SkillImage1Active.SetActive(true);
            //Panel Page
            PPSkillImage1Active.SetActive(true);
            //GIving UP Skills PAGE
            GUSSkillButton1.SetActive(true);
   
        }
        else
        { 
            SkillImage1Active.SetActive(false);
            PPSkillImage1Active.SetActive(false);
            GUSSkillButton1.SetActive(false);
            //GUSUnableConfirmButton.SetActive(true);
        }

        if (gameData.SkillPoint2 == maxHealth2)
        { 
            SkillImage2Active.SetActive(true);
            PPSkillImage2Active.SetActive(true);
            GUSSkillButton2.SetActive(true);

        }
        else
        { 
            SkillImage2Active.SetActive(false);
            PPSkillImage2Active.SetActive(false);
            GUSSkillButton2.SetActive(false);
        }

        if (gameData.SkillPoint3 == maxHealth3)
        {
            SkillImage3Active.SetActive(true);
            PPSkillImage3Active.SetActive(true);
            GUSSkillButton3.SetActive(true);
        }
        else
        { 
            SkillImage3Active.SetActive(false);
            PPSkillImage3Active.SetActive(false);
            GUSSkillButton3.SetActive(false);
            
        }

        if (gameData.SkillPoint4 == maxHealth4)
        {
            SkillImage4Active.SetActive(true);
            PPSkillImage4Active.SetActive(true);
            GUSSkillButton4.SetActive(true);
        }
        else
        {
            SkillImage4Active.SetActive(false);
            PPSkillImage4Active.SetActive(false);
            GUSSkillButton4.SetActive(false);
        }
       


    }

   

    #endregion

    #region GiveUpSkillsPage
  
  

   

    
    private void CheckImage()
    {


        //givingup-skills-page when chose which skill to give up,上方常显UI图标显示
        if (GUSSkill1|| GUSSkill2|| GUSSkill3|| GUSSkill4)
        {
            if (GUSSkill1)
            {
                SkillImage1Submitted.SetActive(true);
            }
            else
            {
                SkillImage1Submitted.SetActive(false);
            }
            if (GUSSkill2)
            {
                SkillImage2Submitted.SetActive(true);
            }
            else 
            {
                SkillImage2Submitted.SetActive(false);
            }
            if (GUSSkill3)
            {
                SkillImage3Submitted.SetActive(true);
            }
            else
            {
                SkillImage3Submitted.SetActive(false);
            }
            if (GUSSkill4)
            {
                SkillImage4Submitted.SetActive(true);
            }
            else
            {
                SkillImage4Submitted.SetActive(false);
            }
            //选中放弃的技能按钮之后才显现确认按钮，否则是失效状态
            GUSUnableConfirmButton.SetActive(false);
            GUSActiveConfirmButton.SetActive(true);
        }
       
    }

    //givingup-skills-page Click confirmBUTTON change image,close page,next level
    private bool submittedSkill1=false;
    private bool submittedSkill2=false;
    private bool submittedSkill3 = false;
    private bool submittedSkill4 = false;
    public void GUSConfirmButton()
    {
        SubmittedSkillImage();
        if (submittedSkill1 || submittedSkill2 || submittedSkill3 || submittedSkill4)
        {
            OpenGiveUpSkillsPage = false;
            GivesUpSkillsPage.SetActive(false);
            GameMgr.Instance.levelMgr.ChangeMap();
        }
        else 
        { 
        //必须选中一项技能进行提交
        }
       
    }
    //givingup-skills-page when click Confirm Button
    private void SubmittedSkillImage()
    {
        submittedSkill1 = false;
        submittedSkill2 = false;
        submittedSkill3 = false;
        submittedSkill4 = false;

        //上方常显UI已经显示过,点击确认之后,panel page展示页面显示,gusPage
        if (GUSSkill1 && gameData.SkillPoint1 == maxHealth1)
        {
            //panel page
            PPSkillImage1Submitted.SetActive(true);
            //giveupskills page 关闭技能选择按钮，开启图像无法选择
            GUSSkillButton1.SetActive(false);
            GUSSkillSubmitted1.SetActive(true);
            
            submittedSkill1=true;

        }
        else
        {
            SkillImage1Submitted.SetActive(false);
            PPSkillImage1Submitted.SetActive(false);           
        }

        if (GUSSkill2 && gameData.SkillPoint2 == maxHealth2)
        {
            //SkillImage2Submitted.SetActive(true);
            PPSkillImage2Submitted.SetActive(true);
            GUSSkillButton2.SetActive(false);
            GUSSkillSubmitted2.SetActive(true);
            submittedSkill2 = true;

        }
        else
        {
            SkillImage2Submitted.SetActive(false);
            PPSkillImage2Submitted.SetActive(false);
        }

        if (GUSSkill3 && gameData.SkillPoint3 == maxHealth3)
        {
            SkillImage3Submitted.SetActive(true);
            PPSkillImage3Submitted.SetActive(true);
            GUSSkillButton3.SetActive(false);
            GUSSkillSubmitted3.SetActive(true);

            submittedSkill3 = true;

        }
        else
        {
            SkillImage3Submitted.SetActive(false);
            PPSkillImage3Submitted.SetActive(false);          
        }

        if (GUSSkill4 && gameData.SkillPoint4 == maxHealth4)
        {
            SkillImage4Submitted.SetActive(true);
            PPSkillImage4Submitted.SetActive(true);
            GUSSkillButton4.SetActive(false);
            GUSSkillSubmitted4.SetActive(true);
            submittedSkill4= true;
        }
        else
        {
            SkillImage4Submitted.SetActive(false);
            PPSkillImage4Submitted.SetActive(false);
        }


    }

    

    public void GUSSelectSkills1()
    {
        GUSSkill1 = true;
        GUSSkill2 = false;
        GUSSkill3 = false;
        GUSSkill4 = false;
        CheckImage();

    }
    public void GUSSelectSkills2()
    {
        GUSSkill2 = true;
        GUSSkill1 = false;
        GUSSkill3 = false;
        GUSSkill4 = false;
        CheckImage();

    }
    public void GUSSelectSkills3()
    {
        GUSSkill3 = true;
        GUSSkill1 = false;
        GUSSkill2 = false;
        GUSSkill4 = false;
        CheckImage();

    }
    public void GUSSelectSkills4()
    {
        GUSSkill4 = true;
        GUSSkill1 = false;
        GUSSkill2 = false;
        GUSSkill3 = false;
        CheckImage();

    }

    public void GUSCancelSelectSkills1()
    {
        GUSSkill1 = false;
        CheckImage();

    }
    public void GUSCancelSelectSkills2()
    {
        GUSSkill2 = false ;
        CheckImage();


    }

    public void GUSCancelSelectSkills3()
    {
        GUSSkill3 = false;
        CheckImage();
        
    }

    public void GUSCancelSelectSkills4()
    {
        GUSSkill4 = false;
        CheckImage();
        
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





