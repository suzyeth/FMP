using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoSingleton<GameMgr>
{
    public LevelMgr levelMgr;

    public bool isInit = false;

    public GameData gameData;

    #region Init
    public override void Init()
    {
        StartCoroutine(IE_InitGame());
    }

    public IEnumerator IE_InitGame()
    {
        //yield return StartCoroutine(ExcelDataMgr.Instance.IE_Init());

        //Init Data
        gameData = new GameData();

        //Init Input
        yield return StartCoroutine(InputMgr.Instance.IE_Init());
        //yield return StartCoroutine(SoundMgr.Instance.IE_Init());
        levelMgr.Init();
        Debug.Log("Init Game Manager");
        isInit = true;
    }
    #endregion

}
