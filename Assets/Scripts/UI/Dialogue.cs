using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    //要不要再次交互
    public bool isDone;
    public GameObject NPCDialog;
    

    public PlayerInput inputs;


    public TextAsset dialogLevel1;
    public TextAsset dialogLevel2;
    public TextAsset dialogLevel3;
    public TextAsset dialogLevel4;
    public TextAsset dialogLevel5;
    public TextAsset dialogLevel6;
    public TextAsset dialogLevel7;
    public TextAsset dialogLevel8;


    //对话内容文本
    public Text dialogText;
    public Text nameText;

    //对话文本-按行分割
    public string[] dialogRows;

    //对话索引
    public int dialogIndex;

    //对话图片
    //public List<Sprite> sprites = new List<Sprite>();
   // public Image speakingProfile;

    string at;
    string comma;

    public float pressTimer;
    float pressTime;

    private int currentMap=0;

   
    private void Awake()
    {
        inputs = new PlayerInput();
        inputs.Enable();
    }
    private void OnEnable()
    {
        inputs.Gameplay.Dialogue.started += OnDialogChange;
    }
    private void OnDisable()
    {
        inputs.Gameplay.Dialogue.started -= OnDialogChange;

    }

    void Start()
    {
       at = "@";
       comma = ",";
       // ReadText(dialogDataFile);
       // ShowDialog();
    }
    
    private void checkMapChange()
    {
        int id = GameMgr.Instance.levelMgr.CurrentMapID();
        
        if (currentMap != id && id<=8)
        {
            if (id == 1)
            { 
                NPCDialog.SetActive(true);
                ReadText(dialogLevel1);
                ShowDialog();
            }
            if (id == 2)
            {
                               
                ReadText(dialogLevel2);
                dialogIndex = 0;
                ShowNPCDialog();
            }
            if (id == 3)
            {
                dialogIndex = 0;
                ReadText(dialogLevel3);
                ShowNPCDialog();
            }
            if (id == 4)
            {
                
                ReadText(dialogLevel4);
                dialogIndex = 0;
                ShowNPCDialog();
            }
            if (id == 5)
            {
                
                ReadText(dialogLevel5);
                dialogIndex = 0;
                ShowNPCDialog();
            }
            if (id == 6)
            {
                
                ReadText(dialogLevel6);
                dialogIndex = 0;
                ShowNPCDialog();
            }
            if (id == 7)
            {
                
                ReadText(dialogLevel7);
                dialogIndex = 0;
                ShowNPCDialog();
            }
            if (id == 8)
            {
                
                ReadText(dialogLevel8);
                dialogIndex = 0;
                ShowNPCDialog();
            }

            currentMap = id;
        }
    }

    private void Update()
    {
        checkMapChange();
        pressTime -= Time.deltaTime;
    }

    public void UpdateText(string _text)
    {
        dialogText.text = Regex.Replace(_text, at, comma);
    }

    public void UpdateName(string text)
    {
        nameText.text = text;
    }

    //public void UpdateImage(int num)
    //{
        //speakingProfile.sprite = sprites[num];
    //}

    public void ReadText(TextAsset _textAsset)
    {
        dialogRows = _textAsset.text.Split('\n');
        //foreach(var row in rows)
        //{
        //    string[] cell = row.Split(',');
        //}

    }

    public void ShowDialog()
    {
        for (int i = 0; i < dialogRows.Length; i++)
        {
            string[] cells = dialogRows[i].Split(',');
            if (cells[0] == "#" && int.Parse(cells[1]) == dialogIndex)
            {
                UpdateText(cells[2]);
                UpdateName(cells[6]);
                //UpdateImage(int.Parse(cells[5]));
                dialogIndex = int.Parse(cells[3]);
                break;
            }
            else if (cells[0] == "END" && int.Parse(cells[1]) == dialogIndex)
            {
                NPCDialog.SetActive(false);
               
            }
        }
    }



    public void OptionEffect(string effect)
    {
        //if (effect == "M1")
        //{
        //    currentDistance = Mathf.Max(currentDistance - 1, 0);
        //}
        //if (effect == "P1")
        //{
        //    currentDistance += 1;
        //}

    }

    public void OnClickNext()
    {
        ShowDialog();
    }

    private void OnDialogChange(InputAction.CallbackContext obj)
    {
        if (pressTime <= 0)
        {
            ShowDialog();
            pressTime = pressTimer;

        }
    }

    public void TriggerAction()
    {
        if (!isDone)
        {
            dialogIndex = 0;
            ShowNPCDialog();
        }
    }

    private void ShowNPCDialog()
    {
        NPCDialog.SetActive(true);
       
        isDone = true;
        this.gameObject.tag = "Untagged";
    }
}
