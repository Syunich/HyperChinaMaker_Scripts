using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
public class UIManager : MonoBehaviour
{
    /// <summary>
    /// GameScene全般のUIを扱う。
    /// </summary>

    [SerializeField] CanvasGroup Chuugoku_text;
    [SerializeField] CanvasGroup clearnum_text;
    [SerializeField] CanvasGroup Failed_CG;
    [SerializeField] CanvasGroup Sabun_CG;

    [SerializeField] public CanvasGroup Ranking_infoCG;
    [SerializeField] Color[] Sabun_Color;

    public CanvasGroup StartText;
    public CanvasGroup ClearButtons_CG;
    public static UIManager UIinstance;

    private Vector3 SabunPosition;
    // Start is called before the first frame update
    void Start()
    {
        UIinstance = this;
        SetupUI();
    }

    // ========= Modures ========= //
    
    public void SetupUI()
    {
        Stop_and_hideUI(ClearButtons_CG, 0);
        Stop_and_hideUI(Chuugoku_text, 0);
        Stop_and_hideUI(Failed_CG, 0);
        Stop_and_hideUI(Ranking_infoCG, 0);
        Stop_and_hideUI(Sabun_CG, 0);
        Ranking_infoCG.blocksRaycasts = false;
        FeedInUI(StartText, 0);
        BlinkUI(StartText);
        SabunPosition = Sabun_CG.transform.localPosition; 
        
    }

    public void GameStart_UIMoving()
    {
        Stop_and_hideUI(StartText , 1f);
        Stop_and_hideUI(ClearButtons_CG , 1f);
        Stop_and_hideUI(Chuugoku_text, 1f);
        HideSabun();
    }

    public void GameClear_UIMoving()
    {
        //ボタン群を表示
        FeedInUI(ClearButtons_CG , 1f);
    }
    
    public void GameFailed_UIMoving()
    {
        FeedInUI(Failed_CG, 1f);
    }

    public void BlinkUI(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.DOFade(0, 1.5f).SetLoops(-1 , LoopType.Yoyo).Play();
    }

    public void Stop_and_hideUI(CanvasGroup cg , float sec)
    {
        cg.DOKill();
        cg.DOFade(0, sec).Play();
        try
        {
            EventTrigger[] triggers = cg.GetComponentsInChildren<EventTrigger>();
            foreach(EventTrigger trigger in triggers)
            {
                trigger.enabled = false;
            }
        }
        catch
        {

        }
    }

    public void FeedInUI(CanvasGroup cg, float sec)
    {
        cg.DOKill();
        cg.DOFade(1, sec).Play().OnComplete(() => 
        { 
            try
            {
                EventTrigger[] triggers = cg.GetComponentsInChildren<EventTrigger>();
                foreach (EventTrigger trigger in triggers)
                {
                    trigger.enabled = true;
                }

            }
            catch
            {

            }
        } );
        
    }

    public IEnumerator Indicate_Chuugoku(int tyu_amount)
    {
        //国ボイス用
        int goku_num = 3;
        goku_num += (tyu_amount / 4);
        if (goku_num > 9)
            goku_num = 9;
        //国ボイス用

        Chuugoku_text.alpha = 1;
        string Chu = null;
        Text Chtext = Chuugoku_text.GetComponent<Text>();
        Chtext.text = null;

        for (int i = 0; i < tyu_amount; i++)
        {
            if ((i + 1) % 7 == 0)
            {
                Chu += "\n";
            }

            if (i == tyu_amount - 1)
            {
                ///最後のちゅうの声
              AudioManager.Instance.PlayVoice(2);
            }
            else
            {
                ///連鎖のちゅうの声
                int voicenum = UnityEngine.Random.Range(0, 2);
                AudioManager.Instance.PlayVoice(voicenum);
            }

            AudioManager.Instance.PlaySE(6);

            Chu += "中";
            Chtext.text = Chu;
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlaySE(7);
        AudioManager.Instance.PlayVoice(7);  /////////////////////goku_numをいれとく
        Chu += "国";
        Chtext.text = Chu;

   
    }

    public void ChangeclearnumText(int num)
    {
        clearnum_text.GetComponent<Text>().text = "クリア回数:" + num;
    }

    public void OpenRankingTab()
    {
        GameManager.GMInstance.isrankingopen = true;
        Ranking_infoCG.blocksRaycasts = true;
        FeedInUI(Ranking_infoCG, 0.5f);
    }

    public void CloseRankingTab()
    {
        GameManager.GMInstance.isrankingopen = false;
        Ranking_infoCG.blocksRaycasts = false;
        Stop_and_hideUI(Ranking_infoCG, 0.5f);
    }

    public void OpenRanking(int id)
    {
        AudioManager.Instance.BGMSource.Stop();
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(GameManager.GMInstance.num_of_clear + 1 , id);
        AudioManager.Instance.PlayBGM(1);
    }

    public IEnumerator IndicateSabun(float value)
    {
        Text SabunText = Sabun_CG.GetComponent<Text>();
        SabunText.text = Math.Round(value, 2, MidpointRounding.AwayFromZero).ToString() + "%";
        Sabun_CG.alpha = 0.2f;
        yield return Sabun_CG.transform.DOLocalMoveX(Sabun_CG.transform.localPosition.x - 500, 0.5f).Play().WaitForCompletion();
        if(value < 90)
        {
            AudioManager.Instance.PlaySE(9);
            SabunText.color = Sabun_Color[0];
        }
        else if(value < 94)
        {
            AudioManager.Instance.PlaySE(10);
            SabunText.color = Sabun_Color[1];
        }
        else if(value < 98)
        {
            AudioManager.Instance.PlaySE(11);
            SabunText.color = Sabun_Color[2];
        }
        else if(value < 99)
        {
           AudioManager.Instance.PlaySE(12);
            SabunText.color = Sabun_Color[3];
        }
        else
        {
           AudioManager.Instance.PlaySE(13);
            SabunText.color = Sabun_Color[4];
        }
        yield return Sabun_CG.DOFade(1, 0.5f).Play();
    }

    public void HideSabun()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(Sabun_CG.DOFade(0, 1f))
           .OnComplete(() => Sabun_CG.transform.localPosition = SabunPosition).Play();

    }
}
