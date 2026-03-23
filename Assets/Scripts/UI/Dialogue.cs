using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;


public class Dialogue : MonoBehaviour
{
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

    public Text dialogText;
    public Text nameText;

    // Key icon text displayed below dialogue
    public Text keyHintText;

    public string[] dialogRows;
    public int dialogIndex;

    string at;
    string comma;

    public float pressTimer;
    float pressTime;

    private int currentMap = -1;
    private GameData gameData;

    // Language: "EN" or "CN", stored in PlayerPrefs
    private string currentLanguage = "EN";
    private const string LANGUAGE_PREF_KEY = "GameLanguage";

    private void Awake()
    {
        inputs = new PlayerInput();
        inputs.Enable();
        gameData = PublicTool.GetGameData();
        currentLanguage = PlayerPrefs.GetString(LANGUAGE_PREF_KEY, "EN");
    }

    private void OnEnable()
    {
        inputs.Gameplay.Dialogue.started += OnDialogChange;
        EventCenter.Instance.AddEventListener("ShowHint", OnShowHintEvent);
        EventCenter.Instance.AddEventListener("ChangeLanguage", OnChangeLanguage);
    }

    private void OnDisable()
    {
        inputs.Gameplay.Dialogue.started -= OnDialogChange;
        EventCenter.Instance.RemoveEventListener("ShowHint", OnShowHintEvent);
        EventCenter.Instance.RemoveEventListener("ChangeLanguage", OnChangeLanguage);
    }

    private void OnShowHintEvent(object arg0)
    {
        string message = arg0 as string;
        if (!string.IsNullOrEmpty(message))
            ShowHint(message);
    }

    private void OnChangeLanguage(object arg0)
    {
        string lang = arg0 as string;
        if (!string.IsNullOrEmpty(lang))
        {
            currentLanguage = lang;
            PlayerPrefs.SetString(LANGUAGE_PREF_KEY, lang);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// Public method for settings UI to switch language
    /// </summary>
    public void SetLanguage(string lang)
    {
        currentLanguage = lang;
        PlayerPrefs.SetString(LANGUAGE_PREF_KEY, lang);
        PlayerPrefs.Save();
    }

    public string GetLanguage()
    {
        return currentLanguage;
    }

    void Start()
    {
        at = "@";
        comma = ",";
        if (keyHintText != null)
            keyHintText.text = "";
    }

    private TextAsset GetDialogAsset(int id)
    {
        switch (id)
        {
            case 1: return dialogLevel1;
            case 2: return dialogLevel2;
            case 3: return dialogLevel3;
            case 4: return dialogLevel4;
            case 5: return dialogLevel5;
            case 6: return dialogLevel6;
            case 7: return dialogLevel7;
            case 8: return dialogLevel8;
            default: return null;
        }
    }

    private void checkMapChange()
    {
        int id = GameMgr.Instance.levelMgr.CurrentMapID();

        if (currentMap != id && id <= 8 && id > 0)
        {
            gameData.WhetherDialogue = true;
            dialogIndex = 0;
            NPCDialog.SetActive(true);

            TextAsset asset = GetDialogAsset(id);
            if (asset != null)
                ReadText(asset);

            ShowDialog();
            currentMap = id;
        }

        if ((id == 0 || id > 8) && currentMap != id)
        {
            NPCDialog.SetActive(false);
            gameData.WhetherDialogue = false;
            ClearVisualHints();
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

    public void ReadText(TextAsset _textAsset)
    {
        dialogRows = _textAsset.text.Split('\n');
    }

    /// <summary>
    /// New CSV format: Tag, ID, Content_EN, Content_CN, Jump, Effect, Image, Name, VisualHint
    /// Falls back to old format (7 columns) if fewer than 9 columns
    /// </summary>
    public void ShowDialog()
    {
        dialogText.text = string.Empty;
        if (keyHintText != null)
            keyHintText.text = "";

        for (int i = 0; i < dialogRows.Length; i++)
        {
            string[] cells = dialogRows[i].Split(',');

            if (cells.Length < 7)
                continue;

            bool isNewFormat = cells.Length >= 9;

            string tag = cells[0].Trim();
            int id;
            if (!int.TryParse(cells[1].Trim(), out id))
                continue;

            if (tag == "#" && id == dialogIndex)
            {
                string content;
                string charName;
                int jumpIndex;

                if (isNewFormat)
                {
                    // New format: Tag, ID, Content_EN, Content_CN, Jump, Effect, Image, Name, VisualHint
                    content = (currentLanguage == "CN") ? cells[3].Trim() : cells[2].Trim();
                    charName = cells[7].Trim();
                    int.TryParse(cells[4].Trim(), out jumpIndex);

                    // Process visual hints
                    string visualHint = cells[8].Trim();
                    ProcessVisualHints(visualHint);
                }
                else
                {
                    // Old format: Tag, ID, Content, Jump, Effect, Image, Name
                    content = cells[2].Trim();
                    charName = cells[6].Trim();
                    int.TryParse(cells[3].Trim(), out jumpIndex);
                    ClearVisualHints();
                }

                UpdateText(content);
                UpdateName(charName);
                dialogIndex = jumpIndex;
                break;
            }
            else if (tag == "END" && id == dialogIndex)
            {
                NPCDialog.SetActive(false);
                gameData.WhetherDialogue = false;
                ClearVisualHints();
            }
        }
    }

    #region Visual Hints

    private void ProcessVisualHints(string hintData)
    {
        // Clear previous hints
        ClearVisualHints();

        if (string.IsNullOrEmpty(hintData))
            return;

        // Multiple hints separated by |
        string[] hints = hintData.Split('|');
        List<string> keyLabels = new List<string>();

        foreach (string hint in hints)
        {
            string trimmed = hint.Trim();
            if (string.IsNullOrEmpty(trimmed))
                continue;

            string[] parts = trimmed.Split(':');
            if (parts.Length < 2)
                continue;

            string type = parts[0].Trim().ToLower();
            string target = parts[1].Trim();

            switch (type)
            {
                case "glow":
                    EventCenter.Instance.EventTrigger("TutorialGlow", target);
                    break;
                case "arrow":
                    EventCenter.Instance.EventTrigger("TutorialArrow", target);
                    break;
                case "key":
                    keyLabels.Add(FormatKeyLabel(target));
                    break;
            }
        }

        // Show key labels in the key hint text
        if (keyLabels.Count > 0 && keyHintText != null)
        {
            keyHintText.text = string.Join("  ", keyLabels.ToArray());
        }
    }

    private string FormatKeyLabel(string key)
    {
        switch (key.ToUpper())
        {
            case "WASD": return "[W][A][S][D] Move";
            case "Z": return "[Z] Undo";
            case "K": return "[K] Pull";
            case "J": return "[J] Skill";
            case "L": return "[L] Teleport";
            default: return "[" + key + "]";
        }
    }

    private void ClearVisualHints()
    {
        EventCenter.Instance.EventTrigger("TutorialClear", 0);
        if (keyHintText != null)
            keyHintText.text = "";
    }

    #endregion

    public void OnClickNext()
    {
        ShowDialog();
    }

    private void OnDialogChange(InputAction.CallbackContext obj)
    {
        if (pressTime <= 0)
        {
            if (isShowingHint)
            {
                HideHint();
                return;
            }
            ShowDialog();
            pressTime = pressTimer;
        }
    }

    #region Hint

    private bool isShowingHint = false;
    private Coroutine hintCoroutine;

    public void ShowHint(string message, float duration = 3f)
    {
        if (gameData.WhetherDialogue) return;

        if (isShowingHint && hintCoroutine != null)
            StopCoroutine(hintCoroutine);

        isShowingHint = true;
        NPCDialog.SetActive(true);
        nameText.text = "";
        dialogText.text = message;

        hintCoroutine = StartCoroutine(AutoHideHint(duration));
    }

    private IEnumerator AutoHideHint(float duration)
    {
        yield return new WaitForSeconds(duration);
        HideHint();
    }

    private void HideHint()
    {
        if (hintCoroutine != null)
        {
            StopCoroutine(hintCoroutine);
            hintCoroutine = null;
        }
        isShowingHint = false;
        NPCDialog.SetActive(false);
    }

    #endregion
}
