using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class FadeInOutManager : MonoBehaviour
{
    [Header("最初からフェードインが完了しているかどうか")] public bool firstFadeInComp;
    [Header("これはゲームシーンのオブジェクトか")] public bool IsthisGameSceneObj;
    [Header("これはチュートリアルのオブジェクトか")] public bool IsthisTutorialObj;
    [Header("これはチュ国モードのオブジェクトか")] public bool IsthistyutyuObj;

    Sensu_Manager[] senses;
    private int frameCount = 0;
    private bool fadeIn = false;
    private bool fadeOut = false;
    private bool compFadeIn = false;
    private bool compFadeOut = false;
    private bool FirstAnim = true;

    private string SceneName;
    public bool IsFadeInComplete
    {
        get { return compFadeIn; }
    }
    public bool IsFadeOutComplete
    {
        get { return compFadeOut; }
    }

    private void Start()
    {
        SceneName = "TitleScene";
       senses = GetComponentsInChildren<Sensu_Manager>();
            if (firstFadeInComp)
            {
                FadeInComplete();
            }
            else
            {
                StartFadeIn();
            }
    }
    void Update()
    {

        //シーン移行時の処理の重さでTime.deltaTimeが大きくなってしまうから2フレーム待つ
        if (frameCount > 2 && FirstAnim)
        {
            if (fadeIn)
            {
               StartCoroutine(FadeInUpdate());
            }
            else if (fadeOut)
            {
                StartCoroutine(FadeOutUpdate());
            }
        }
        ++frameCount;
    }

    public void StartFadeIn()
    {
        AudioManager.Instance.PlaySE(0);

        if (fadeIn || fadeOut)
        {
            return;
        }
        fadeIn = true;
        compFadeIn = false;
        foreach (Sensu_Manager sense in senses)
        {
            sense.SetFadeIn();
        }
    }
    public void StartFadeOut(string Scene_name)
    {
        AudioManager.Instance.PlaySE(1);
        AudioManager.Instance.FeedOutBGM();

        SceneName = Scene_name;
        if (fadeIn || fadeOut)
        {
            return;
        }
        fadeOut = true;
        compFadeOut = false;
        foreach(Sensu_Manager sense in senses)
        {
            sense.SetFadeOut();
        }
    }
    IEnumerator FadeInUpdate()
    {
        FirstAnim = false;
        StartCoroutine(senses[0].StartFadeIn_Moving());
        yield return StartCoroutine(senses[1].StartFadeIn_Moving());
        FadeInComplete();

    }

    //フェードアウト中
    IEnumerator FadeOutUpdate()
    {
        FirstAnim = false;
        StartCoroutine(senses[0].StartFadeOut_Moving());
        yield return StartCoroutine(senses[1].StartFadeOut_Moving());
        FadeOutComplete();
    }

    //フェードイン完了
    private void FadeInComplete()
    {
        fadeIn = false;
        compFadeIn = true;
        FirstAnim = true;
        foreach (Sensu_Manager sense in senses)
        {
            sense.CompleteFadeIn();
        }
        if(IsthisGameSceneObj || IsthistyutyuObj)
        {
            AudioManager.Instance.PlayBGM(2);
            GameManager.GMInstance.cangonextgame = true;
        }
        else if(IsthisTutorialObj)
        {
            AudioManager.Instance.PlayBGM(1);
            StartCoroutine(UI_Tutorial.instance.Indicate_Dialog(0));
        }
        else
        {
            ///タイトルなので、BGM起動
            AudioManager.Instance.PlayBGM(0);
        }
    }

    //フェードアウト完了
    private void FadeOutComplete()
    {

        FirstAnim = true;
        fadeOut = false;
        compFadeOut = true;
        foreach (Sensu_Manager sense in senses)
        {
            sense.CompleteFadeOut();
        }
        DOTween.KillAll();
        SceneManager.LoadScene(SceneName);
    }
}

