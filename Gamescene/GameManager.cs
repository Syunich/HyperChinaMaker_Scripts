using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 【GameManagerでやること】
    /// ⓪ゲームのセットアップ(Ref : SetUpGame)
    /// ①ゲーム開始処理(Ref : GameStart)
    /// ②ゲーム成功処理(Ref : GameClear)
    /// ③ゲーム失敗処理(Ref : GameFailed)
    /// GameManagerにはDotweenによる動きの処理は書かないようにする
    /// </summary>
    [Header("これは中中国モードのオブジェクトか")] public bool IsthistyuutyuuObj;

    public static GameManager GMInstance;
    [SerializeField] Chinaland_Manager China;
    [SerializeField] FadeInOutManager fadeio;


    private bool IsGaming; //挑戦中かどうかを判別するbool型
    private bool IsFailed; //失敗したかどうかを判別するbool型
    private bool CangoNextGame;
    private bool IsRankingOpen;
    private bool NowSceneChange;
    private int Num_of_Clear; //クリア回数を保存する変数
    

    // ==== Propaties ==== //
    public bool isgaming
    {
        get { return IsGaming; } set { IsGaming = value; }
    }
    public int num_of_clear
    {
        get { return Num_of_Clear; } set { Num_of_Clear = value; }
    }
    public bool cangonextgame
    {
        get { return CangoNextGame; }
        set { CangoNextGame = value; }
    }

    public bool isrankingopen
    {
        get { return IsRankingOpen; }
        set { IsRankingOpen = value; }
    }
    // ==== Propaties ==== //

    private void Awake()
    {
        GMInstance = this;
    }
    void Start()
    {
        SetUpGame();
    }
    private void Update()
    {
          if(CangoNextGame && Input.GetKeyDown(KeyCode.S) && !IsFailed)
        {
            if (!IsFailed && !IsRankingOpen)
            {
                GameStart();
            }
        }

          if(!CangoNextGame && Input.GetKeyDown(KeyCode.S) && IsFailed && !NowSceneChange)
        {
            ////タイトルに戻る処理
            NowSceneChange = true;
            fadeio.StartFadeOut("TitleScene");
        }

        if (!CangoNextGame && Input.GetKeyDown(KeyCode.R) && IsFailed && !NowSceneChange)
        {
            ////リトライ
            NowSceneChange = true;
            if (fadeio.IsthisGameSceneObj)
            {
                fadeio.StartFadeOut("GameScene");
            }
            else if(fadeio.IsthistyutyuObj)
            {
                fadeio.StartFadeOut("Tyuutyuugoku_mode");
            }

        }


        if (isgaming && Input.GetKeyDown(KeyCode.Space))
        {
            China.StopChina(China.CurrentChina);
            if(China.IsCurrntSmall())
            {
              StartCoroutine(GameClear());
            }
            else
            {
               StartCoroutine(GameFailed());
            }
        }
    }

    void SetUpGame()
    {
        NowSceneChange = false;
        IsGaming = false;
        IsFailed = false;
        IsRankingOpen = false;
        CangoNextGame = false;
        num_of_clear = 0;
    }
    public void GameStart()
        {
        CangoNextGame = false;
        UIManager.UIinstance.GameStart_UIMoving();
        China.StartWave();
        }

    IEnumerator GameClear()
    {
        isgaming = false;
        num_of_clear++;

        yield return new WaitForSeconds(1.0f); 
        yield return StartCoroutine(UIManager.UIinstance.Indicate_Chuugoku(num_of_clear + 1));
        yield return new WaitForSeconds(0.5f);

        AudioManager.Instance.PlaySE(4);

        China.SetNormalPosition();

        if (!IsthistyuutyuuObj)
        {
            //ノーマルモードの処理
            UIManager.UIinstance.GameClear_UIMoving();
            UIManager.UIinstance.ChangeclearnumText(num_of_clear);
            if (!Tyuutyu_Flag.ClearNormal)
                Tyuutyu_Flag.ClearNormal = true;

        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(UIManager.UIinstance.IndicateSabun(China.Sabun));

        if(IsthistyuutyuuObj)
        {
            ///中中国モードの時の処理
            yield return new WaitForSeconds(1.0f);
            AudioManager.Instance.FeedOutBGM();
            yield return new WaitForSeconds(1.0f);
            naichilab.RankingLoader.Instance.SendScoreAndShowRanking(China.Sabun / 100f, 1);
            AudioManager.Instance.PlayBGM(1);
            yield break;
        }

        CangoNextGame = true;
    }
    IEnumerator GameFailed()
    {
        isgaming = false;
        AudioManager.Instance.FeedOutBGM();
        yield return new WaitForSeconds(1.5f); //すこし間をあけてから処理はじめ
        AudioManager.Instance.PlaySE(5);
        UIManager.UIinstance.GameFailed_UIMoving();
        IsFailed = true;
    }

}


