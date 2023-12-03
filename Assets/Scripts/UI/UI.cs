using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Color = UnityEngine.Color;

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
    private float SK1Amount, TargetSK1Amount;
    private float SK2Amount, TargetSK2Amount;
    private float health3, health33;
    private float health4, health44;

    private const float maxHealth1 = 4f;
    private const float maxHealth2 = 5f;
    private const float maxHealth3 = 7f;
    private const float maxHealth4 = 6f;



    private float CurrentSkillBar1Amount = 0f;
    private float CurrentSkillBar2Amount = 0f;
    private float CurrentSkillBar3Amount = 0f;
    private float CurrentSkillBar4Amount = 0f;
    public GameObject SkillListSubmitted1;
    public GameObject SkillListSubmitted2;
    public GameObject SkillListSubmitted3;
    public GameObject SkillListSubmitted4;


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
    private float CrystalAmount;
    private float CurrentCrystalAmount;
    private const float maxCrystalAmount = 45f;

    private float AllPoint = 0f;


    private bool intial = false;


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
    private bool OpenGiveUpSkillsPage = false;


    private bool GUSSkill1 = false;
    private bool GUSSkill2 = false;
    private bool GUSSkill3 = false;
    private bool GUSSkill4 = false;

    //Starting Level
    public GameObject EphemeralUI;

    public Image SPStartingImage1;
    public Image SPStartingImage2;
    public Image SPSettingImage1;
    public Image SPSettingImage2;
    public float transitionTime = 5f; // 过渡时间


    private bool StartingLeftButtonIsPressd = false;
    private bool StartingRightButtonIsPressd = false;
    private bool EndingLeftButtonIsPressd = false;
    private bool EndingRightButtonIsPressd = false;
    public GameObject StartingPanel;
    private bool hasEnteredGame = false;
    private bool hasEnteredSetting = false;


    // Start is called before the first frame update
    void Start()
    {

        gameData = PublicTool.GetGameData();
       
        CrystalAmount = 0;

        InitCheck();



    }

    // Update is called once per frame
    void Update()
    {

        BarFiller();


        FillProgress();
        ResetProgress();

        // 更新进度条
        UpdateProgress();

    }


    private void InitCheck()
    {
        
        StartingPanel.SetActive(true);
        SkillListPage.SetActive(false);
        PanelPage.SetActive(false);
        SettingPage.SetActive(false);
        EphemeralUI.SetActive(false);
        LevelPage.SetActive(false); 
        MusicPage.SetActive(false);
        ControlPage.SetActive(false);
        CreditsPage.SetActive(false);

        GUSSkillButton1.SetActive(false);
        GUSSkillButton2.SetActive(false);
        GUSSkillButton3.SetActive(false);
        GUSSkillButton4.SetActive(false);
        SkillListSubmitted4.SetActive(false);
        SkillListSubmitted3.SetActive(false);
        SkillListSubmitted2.SetActive(false);
        SkillListSubmitted1.SetActive(false);
        PPSkillImage1Submitted.SetActive(false);
        PPSkillImage2Submitted.SetActive(false);
        PPSkillImage3Submitted.SetActive(false);
        PPSkillImage4Submitted.SetActive(false);
    }

    #region Event

    private void OnEnable()
    {

        EventCenter.Instance.AddEventListener("UseSkills", UseSkillsEvent);
        EventCenter.Instance.AddEventListener("ChangeLevelText", ChangeLevelTextEvent);
        EventCenter.Instance.AddEventListener("PartEnd", PartEndEvent);
        EventCenter.Instance.AddEventListener("PlayerOnButton", PlayerOnButtonEvent);




    }

    private void OnDisable()
    {

        EventCenter.Instance.RemoveEventListener("UseSkills", UseSkillsEvent);
        EventCenter.Instance.RemoveEventListener("ChangeLevelText", ChangeLevelTextEvent);
        EventCenter.Instance.RemoveEventListener("PartEnd", PartEndEvent);
        EventCenter.Instance.RemoveEventListener("PlayerOnButton", PlayerOnButtonEvent);


    }

    private void UseSkillsEvent(object arg0)
    {
        ResetSkillPoints();
        //UnityEngine.Debug.Log("USEDSKILLS");
        ActiveUpSkillImage();
    }

    private void ChangeLevelTextEvent(object arg0)
    {
        LevelPage.SetActive(false);
        
        

        int id = GameMgr.Instance.levelMgr.CurrentMapID();
        if (id == 0)
        {
            InitCheck();
            gameData.RestartClearData();
            CurrentLevelText.text = ("");
           
            
            hasEnteredGame = false;
            hasEnteredSetting = false;

            StartingLeftButtonIsPressd = false;
            StartingRightButtonIsPressd = false;
            EndingLeftButtonIsPressd = false;
            EndingRightButtonIsPressd = false;

            ResetSkillPoints();
            ActiveUpSkillImage();
            CheckImage();
            PPSkillImage4Submitted.SetActive(false);

        }
        else
        {
            EphemeralUI.SetActive(true);
            StartingPanel.SetActive(false);
        }
        if (id == 1)
        {
           // AudioManager.Instance.PlayBackGroundMusic();
        }
        
        CurrentLevelText.text = ("LEVEL  " + id);


    }

    private void PartEndEvent(object arg0)
    {

        OpenGiveingupSkillsPage();
        OpenGiveUpSkillsPage = true;


    }


    private void PlayerOnButtonEvent(object arg0)
    {
        int direction = (int)arg0;
        StartPAGECheckButtonState(direction);


    }





    #endregion



    //SKILL LIST PAGE
    #region AddPoint
    private void BarFiller()

    {
        RefreshSkillBar4(health44);
        if (CurrentSkillBar1Amount != TargetSK1Amount)
        {
            CurrentSkillBar1Amount = TargetSK1Amount;

            RefreshSkillBar1(TargetSK1Amount);
            RefreshCrystalBar(CrystalAmount);

        }
        if (CurrentSkillBar2Amount != TargetSK2Amount)
        {
            CurrentSkillBar2Amount = TargetSK2Amount;
            RefreshSkillBar2(TargetSK2Amount);
            RefreshCrystalBar(CrystalAmount);


        }
        if (CurrentSkillBar3Amount != health33)
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



            gameData.SkillPoint1 = 0;
            gameData.SkillPoint2 = 0;
            gameData.SkillPoint3 = 0;
            gameData.SkillPoint4 = 0;
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
            CrystalAmount = gameData.GetNumActiveCrystal() + AllPoint;
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
        Bar2Text.text = amount + "/" + maxHealth2;
    }

    private void RefreshSkillBar3(float amount)
    {
        Bar3.fillAmount = amount / maxHealth3;
        Bar3Text.text = amount + "/" + maxHealth3;
    }

    private void RefreshSkillBar4(float amount)
    {
        Bar4.fillAmount = amount / maxHealth4;
        Bar4Text.text = amount + "/" + maxHealth4;
    }

    private void RefreshCrystalBar(float amount)
    {

        CrystalBar.fillAmount = amount / maxCrystalAmount;
        scoreText.text = amount + "/" + maxCrystalAmount;

    }


    public void AddHealth1()
    {
        if (TargetSK1Amount < maxHealth1 && CrystalAmount >= 1)
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

        if (StartingPanel.activeSelf)
        {


            GameMgr.Instance.levelMgr.RestartThisMap();
            hasEnteredSetting = false;
            EndingLeftButtonIsPressd = false;
            EndingRightButtonIsPressd = false;



        }
        SettingPage.SetActive(false);
        Debug.Log("SettingPage.SetActive" + SettingPage.activeSelf);

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
            if (!gameData.GiveUpSkills1)
            {
                //UP ui
                SkillImage1Active.SetActive(true);
                //Panel Page
                PPSkillImage1Active.SetActive(true);
                //GIving UP Skills PAGE
                GUSSkillButton1.SetActive(true);
            }


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
            if (!gameData.GiveUpSkills2)
            {
                SkillImage2Active.SetActive(true);
                PPSkillImage2Active.SetActive(true);
                GUSSkillButton2.SetActive(true);
            }

        }
        else
        {
            SkillImage2Active.SetActive(false);
            PPSkillImage2Active.SetActive(false);
            GUSSkillButton2.SetActive(false);
        }

        if (gameData.SkillPoint3 == maxHealth3)
        {
            if (!gameData.GiveUpSkills3)
            {
                SkillImage3Active.SetActive(true);
                PPSkillImage3Active.SetActive(true);
                GUSSkillButton3.SetActive(true);
            }
        }
        else
        {
            SkillImage3Active.SetActive(false);
            PPSkillImage3Active.SetActive(false);
            GUSSkillButton3.SetActive(false);

        }

        if (gameData.SkillPoint4 == maxHealth4)
        {
            if (!gameData.GiveUpSkills4)
            {
                SkillImage4Active.SetActive(true);
                PPSkillImage4Active.SetActive(true);
                GUSSkillButton4.SetActive(true);
            }
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
        if (gameData.GiveUpSkills1 || gameData.GiveUpSkills2 || gameData.GiveUpSkills3 || gameData.GiveUpSkills4)
        {
            if (gameData.GiveUpSkills1)
            {
                SkillImage1Submitted.SetActive(true);
            }

            if (gameData.GiveUpSkills2)
            {
                SkillImage2Submitted.SetActive(true);
            }

            if (gameData.GiveUpSkills3)
            {
                SkillImage3Submitted.SetActive(true);
            }

            if (GUSSkill4 || gameData.GiveUpSkills4)
            {
                SkillImage4Submitted.SetActive(true);
            }

        }

        //givingup-skills-page when chose which skill to give up,上方常显UI图标显示
        if (GUSSkill1 || GUSSkill2 || GUSSkill3 || GUSSkill4)
        {
           
            //选中放弃的技能按钮之后才显现确认按钮，否则是失效状态
            GUSUnableConfirmButton.SetActive(false);
            GUSActiveConfirmButton.SetActive(true);
        }
        
        if (!gameData.GiveUpSkills1)
        {
            if (GUSSkill1)
            {
                SkillImage1Submitted.SetActive(true);
            }
            else
            {
                SkillImage1Submitted.SetActive(false);
            }
        }
        if (!gameData.GiveUpSkills2)
        {
            if (GUSSkill2)
            {
                SkillImage2Submitted.SetActive(true);
            }
            else
            {
                SkillImage2Submitted.SetActive(false);
            }
        }
        if (!gameData.GiveUpSkills3)
        {
            if (GUSSkill3)
            {
                SkillImage3Submitted.SetActive(true);
            }
            else
            {
                SkillImage3Submitted.SetActive(false);
            }
        }
        if (!gameData.GiveUpSkills4)
        {
            if (GUSSkill4)
            {
                SkillImage4Submitted.SetActive(true);
            }
            else
            {
                SkillImage4Submitted.SetActive(false);
            }
        }
      
        //关闭确认按钮
        if (!GUSSkill1 && !GUSSkill2 && !GUSSkill3 && !GUSSkill4)
        {
            GUSUnableConfirmButton.SetActive(true);
            GUSActiveConfirmButton.SetActive(false);
        }

    }

    //givingup-skills-page Click confirmBUTTON change image,close page,next level
    private bool submittedSkill1 = false;
    private bool submittedSkill2 = false;
    private bool submittedSkill3 = false;
    private bool submittedSkill4 = false;
    public void GUSConfirmButton()
    {
        SubmittedSkillImage();
        if (submittedSkill1 || submittedSkill2 || submittedSkill3 || submittedSkill4)
        {
            OpenGiveUpSkillsPage = false;
            GivesUpSkillsPage.SetActive(false);
            if (submittedSkill1)
            { gameData.GiveUpSkills1 = true; }
            if (submittedSkill2)
            { gameData.GiveUpSkills2 = true; }
            if (submittedSkill3)
            { gameData.GiveUpSkills3 = true; }
            if (submittedSkill4)
            { gameData.GiveUpSkills4 = true; }
            GameMgr.Instance.levelMgr.ChangeMap();
            CloseButton1.SetActive(true);
            gameData.SaveLevelData();

        }
        else
        {
            //提示必须选中一项技能进行提交
        }

        GUSSkill1 = false;
        GUSSkill2 = false;
        GUSSkill3 = false;
        GUSSkill4 = false;
        GUSUnableConfirmButton.SetActive(true);
        GUSActiveConfirmButton.SetActive(false);

    }
    //givingup-skills-page when click Confirm Button
    private void SubmittedSkillImage()
    {

        submittedSkill1 = false;
        submittedSkill2 = false;
        submittedSkill3 = false;
        submittedSkill4 = false;
        //上方常显UI已经显示过,点击确认之后,panel page展示页面显示,gusPage
        if (!gameData.GiveUpSkills1)
        {

            if (GUSSkill1 && gameData.SkillPoint1 == maxHealth1)
            {
                //panel page
                PPSkillImage1Submitted.SetActive(true);
                SkillListSubmitted1.SetActive(true);
                //giveupskills page 关闭技能选择按钮，开启图像无法选择
                GUSSkillButton1.SetActive(false);
                GUSSkillSubmitted1.SetActive(true);

                submittedSkill1 = true;

            }
            else
            {
                SkillImage1Submitted.SetActive(false);
                PPSkillImage1Submitted.SetActive(false);
                SkillListSubmitted1.SetActive(false);
            }
        }

        if (!gameData.GiveUpSkills2)
        {
            if (GUSSkill2 && gameData.SkillPoint2 == maxHealth2)
            {
                //SkillImage2Submitted.SetActive(true);
                PPSkillImage2Submitted.SetActive(true);
                SkillListSubmitted2.SetActive(true);
                GUSSkillButton2.SetActive(false);
                GUSSkillSubmitted2.SetActive(true);
                submittedSkill2 = true;

            }
            else
            {
                SkillImage2Submitted.SetActive(false);
                PPSkillImage2Submitted.SetActive(false);
                SkillListSubmitted2.SetActive(false);
            }
        }

        if (!gameData.GiveUpSkills3)
        {
            if (GUSSkill3 && gameData.SkillPoint3 == maxHealth3)
            {
                SkillImage3Submitted.SetActive(true);
                SkillListSubmitted3.SetActive(true);
                PPSkillImage3Submitted.SetActive(true);
                GUSSkillButton3.SetActive(false);
                GUSSkillSubmitted3.SetActive(true);

                submittedSkill3 = true;

            }
            else
            {
                SkillImage3Submitted.SetActive(false);
                PPSkillImage3Submitted.SetActive(false);
                SkillListSubmitted3.SetActive(false);
            }
        }

        if (!gameData.GiveUpSkills4)
        {
            if (GUSSkill4 && gameData.SkillPoint4 == maxHealth4)
            {
                SkillImage4Submitted.SetActive(true);
                SkillListSubmitted4.SetActive(true);
                PPSkillImage4Submitted.SetActive(true);
                GUSSkillButton4.SetActive(false);
                GUSSkillSubmitted4.SetActive(true);
                submittedSkill4 = true;
            }
            else
            {
                SkillImage4Submitted.SetActive(false);
                PPSkillImage4Submitted.SetActive(false);
                SkillListSubmitted4.SetActive(false);
            }
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
        GUSSkill2 = false;
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












    #region StartGames


    public float fillSpeed = 0.3f; // 进度条填充速度
    public float resetSpeed = 0.5f; // 进度条重置速度
    public float requiredFillAmount = 2.0f; // 需要填充的最小量

    private float currentFillAmount1 = 0.0f; // 当前填充的量
    private float currentFillAmount2 = 0.0f;
    private float currentFillAmount3 = 0.0f;
    private float currentFillAmount4 = 0.0f;

    private void StartPAGECheckButtonState(int button)
    {
        //检查button的状态
        if (button == 1)
        {
            StartingLeftButtonIsPressd = true;
            Debug.Log("StartingLeftButtonIsPressd" + StartingLeftButtonIsPressd);
        }
        if (button == 2)
        {
            StartingRightButtonIsPressd = true;
        }
        if (button == 3)
        {
            EndingLeftButtonIsPressd = true;
        }
        if (button == 4)
        {
            EndingRightButtonIsPressd = true;
        }
        if (button == -1)
        {
            StartingLeftButtonIsPressd = false;
            Debug.Log("StartingLeftButtonIsPressd" + StartingLeftButtonIsPressd);
        }
        if (button == -2)
        {
            StartingRightButtonIsPressd = false;
        }
        if (button == -3)
        {
            EndingLeftButtonIsPressd = false;
        }
        if (button == -4)
        {
            EndingRightButtonIsPressd = false;
        }



    }

    private void FillProgress()
    {

        //判断是哪个按钮被按下，stratbuttonLeft=1,stratbuttonRight=2,settingbuttonLeft=3,settingbuttonRight=4
        if (StartingLeftButtonIsPressd)
        {

            //触发开始左按钮
            currentFillAmount1 += fillSpeed * Time.deltaTime;
            currentFillAmount1 = Mathf.Clamp01(currentFillAmount1);
        }
        if (StartingRightButtonIsPressd)
        {

            //触发开始右按钮
            currentFillAmount2 += fillSpeed * Time.deltaTime;
            currentFillAmount2 = Mathf.Clamp01(currentFillAmount2);
        }
        if (EndingLeftButtonIsPressd)
        {

            //触发开始设置左按钮
            currentFillAmount3 += fillSpeed * Time.deltaTime;
            currentFillAmount3 = Mathf.Clamp01(currentFillAmount3);
        }
        if (EndingRightButtonIsPressd)
        {

            //触发开始设置右按钮
            currentFillAmount4 += fillSpeed * Time.deltaTime;
            currentFillAmount4 = Mathf.Clamp01(currentFillAmount4);
        }


        // 在按钮按下时逐渐增加填充量


        // 如果填充达到所需量，可以进入游戏
        //startingPage

    }

    private void ResetProgress()
    {
        // 在按钮释放时逐渐减小填充量

        //判断是哪个按钮被释放，stratbuttonLeft=1,stratbuttonRight=2,settingbuttonLeft=3,settingbuttonRight=4
        if (!StartingLeftButtonIsPressd)
        {

            //释放开始左按钮
            currentFillAmount1 -= fillSpeed * Time.deltaTime;
            currentFillAmount1 = Mathf.Clamp01(currentFillAmount1);
        }
        if (!StartingRightButtonIsPressd)
        {

            //释放开始右按钮
            currentFillAmount2 -= fillSpeed * Time.deltaTime;
            currentFillAmount2 = Mathf.Clamp01(currentFillAmount2);
        }
        if (!EndingLeftButtonIsPressd)
        {

            //释放开始设置左按钮
            currentFillAmount3 -= fillSpeed * Time.deltaTime;
            currentFillAmount3 = Mathf.Clamp01(currentFillAmount3);
        }
        if (!EndingRightButtonIsPressd)
        {

            //释放开始设置右按钮
            currentFillAmount4 -= fillSpeed * Time.deltaTime;
            currentFillAmount4 = Mathf.Clamp01(currentFillAmount4);
        }
    }


    private void UpdateProgress()
    {
        // 更新 UI 中的填充量
        //starting setting 两左两右一共四个

        SPStartingImage1.fillAmount = currentFillAmount1;
        SPStartingImage2.fillAmount = currentFillAmount2;
        SPSettingImage1.fillAmount = currentFillAmount3;
        SPSettingImage2.fillAmount = currentFillAmount4;
        if (SPStartingImage1.fillAmount == 1 && SPStartingImage2.fillAmount == 1 && !hasEnteredGame)
        {

            EnterGame();
            hasEnteredGame = true;

        }

        if (SPSettingImage1.fillAmount == 1 && SPSettingImage2.fillAmount == 1 && !hasEnteredSetting)
        {
            EnterSettingPage();
            hasEnteredSetting = true;
        }


    }

    private void EnterGame()
    {
        VideoPlayerController.Instance.PlayStartVideo();
        StartingPanel.SetActive(false);
        //GameMgr.Instance.levelMgr.ChangeMap();
       
       
    }

    private void EnterSettingPage()
    {
        OpenSettingPage();
        //Debug.Log("Enter SettingPage!");
    }

    #endregion
}





