using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Manager : MonoBehaviour
{

    /// <summary>
    /// 【GameManagerでやること】
    /// ⓪ゲームのセットアップ(Ref : SetUpGame)
    /// ①ゲーム開始処理(Ref : GameStart)
    /// ②ゲーム成功処理(Ref : GameClear)
    /// GameManagerにはDotweenによる動きの処理は書かないようにする
    /// </summary>

    public static Tutorial_Manager instance;
    [SerializeField] Chinaland_Manager China;
    [SerializeField] FadeInOutManager fadeio;

    private bool IsGaming; //挑戦中かどうかを判別するbool型
    private bool CangoNextGame;
    private bool CanTalk; //会話をはじめられる状態か
    private bool IsgoTitle; //連打防止用

    private int Dialog_number;
    private int Num_of_Clear; //クリア回数を保存する変数
    

    // ==== Propaties ==== //
    public bool isgaming
    {
        get { return IsGaming; } set { IsGaming = value; }
    }
    public bool cangonextgame
    {
        get { return CangoNextGame; }
        set { CangoNextGame = value; }
    }
    public bool cantalk
    {
        get { return CanTalk; }
        set { CanTalk = value; }
    }
    public int num_of_clear
    {
        get { return Num_of_Clear; }
        set { Num_of_Clear = value; }
    }
    public int dialognum
    {
        get { return Dialog_number; }
        set { Dialog_number = value; }
    }

    
    // ==== /Propaties ==== //

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        SetUpGame();
    }
    private void Update()
    {
        ///ここにbool処理を書いていく
        if(Input.GetKeyDown(KeyCode.T) && !IsgoTitle)
        {
            IsgoTitle = true;
            fadeio.StartFadeOut("TitleScene");
        }

        if(Input.GetKeyDown(KeyCode.Space) && isgaming)
        {
            StartCoroutine(GameClear());
            UI_Tutorial.instance.hideDialog_Short();
            UI_Tutorial.instance.Hideright();
        }

        if(Input.GetKeyDown(KeyCode.S) && cangonextgame)
        {
            GameStart();
        }

        if(Input.GetMouseButtonDown(0) && CanTalk)
        {
            CanTalk = false;
            if (Dialog_number == 6 || Dialog_number == 10)
            {
                UI_Tutorial.instance.Dialog_mid_end_UIMoving();
                cangonextgame = true;
            }
            else if(Dialog_number == 7)
            {
                return; //7回目の会話はクリックしても動作しない
            }
            else if (Dialog_number == UI_Tutorial.instance.dialogues.Length)
            {
                UI_Tutorial.instance.hideDialog();
                fadeio.StartFadeOut("TitleScene");
            }
            else
            {
                StartCoroutine(UI_Tutorial.instance.Indicate_Dialog(Dialog_number));
            }

        }
    }

    void SetUpGame()
    {
        IsgoTitle = false;
        IsGaming = false;
        CangoNextGame = false;
        CanTalk = false;
        num_of_clear = 0;
        dialognum = 0;
    }
    public void GameStart()
        {
        AudioManager.Instance.FeedOutBGM(); //音楽ストップ

        CangoNextGame = false;
        UI_Tutorial.instance.GameStart_UIMoving();
        China.StartWave_TR();
        }

    IEnumerator GameClear()
    {
        isgaming = false;
        num_of_clear++;

        yield return new WaitForSeconds(1.0f); //すこし間をあけてから処理はじめ
        yield return StartCoroutine(UI_Tutorial.instance.Indicate_Chuugoku(num_of_clear + 1));
        yield return new WaitForSeconds(0.5f);
        China.SetNormalPosition();
        AudioManager.Instance.PlaySE(4);
        yield return new WaitForSeconds(2.5f);

        UI_Tutorial.instance.Dialog_Feed();
        AudioManager.Instance.PlayBGM(1);

        yield return new WaitForSeconds(1.0f);
        StartCoroutine(UI_Tutorial.instance.Indicate_Dialog(dialognum));

    }
}


