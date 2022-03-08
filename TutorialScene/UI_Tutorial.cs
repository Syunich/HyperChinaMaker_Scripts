using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
public class UI_Tutorial : MonoBehaviour
{
    /// <summary>
    /// GameScene全般のUIを扱う。
    /// </summary>

    [SerializeField] CanvasGroup Chuugoku_text;
    [SerializeField] CanvasGroup Gotitle_text;
    [SerializeField] CanvasGroup Triangle;
    [SerializeField] CanvasGroup Dialog_CG;
    [SerializeField] SpriteRenderer right_renderer;

     public CanvasGroup StartText;
     public CanvasGroup ClearButtons_CG;

    [SerializeField] Text Dialog_Text;
    [SerializeField] Transform zizii_transform;



    public static UI_Tutorial instance;


    public string[] dialogues;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        SetupUI();
    }

    // ========= Modures ========= //
    
    public void SetupUI()
    {
        Stop_and_hideUI(ClearButtons_CG, 0);
        Stop_and_hideUI(Chuugoku_text, 0);
        Stop_and_hideUI(StartText, 0);
        Stop_and_hideUI(Triangle, 0);
        FeedInUI(Dialog_CG , 0);

        BlinkUI(Gotitle_text);
        UpDown_Transform(zizii_transform);
        
    }

    public void GameStart_UIMoving()
    {
        Stop_and_hideUI(StartText , 1f);
        Stop_and_hideUI(ClearButtons_CG , 1f);
        Stop_and_hideUI(Chuugoku_text, 1f);
        Stop_and_hideUI(Dialog_CG , 1f);

    }

    public void Dialog_mid_end_UIMoving()
    {
        hideDialog();
        FeedInUI(StartText, 1.0f);
        BlinkUI(StartText);
    }


    public void GameClear_UIMoving()
    {
        //ボタン群を表示
        FeedInUI(ClearButtons_CG , 1f);
    }

    public void BlinkUI(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.DOFade(0, 1.5f).SetLoops(-1 , LoopType.Yoyo).Play();
    }

    public void UpDown_Transform(Transform t)
    {
        t.DOMoveY(t.position.y + 10f, 1.0f).SetLoops(-1, LoopType.Yoyo).Play();
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

    public void hideDialog()　//おじさんと画面を消す。
    {
        Vector3 nowzizitransform = zizii_transform.position;

        Sequence seq = DOTween.Sequence();
        seq.Append(zizii_transform.DOMoveX(zizii_transform.position.x - 200, 1.3f)).SetEase(Ease.InCubic)
           .Append(Dialog_CG.DOFade(0, 0.5f))
           .Play()
           .OnComplete(() => { zizii_transform.position = nowzizitransform; zizii_transform.DOKill(); Dialog_Text.text = ""; } );
    }

    public void hideDialog_Short()
    {
        Dialog_CG.DOFade(0 , 0.5f).Play();
        Dialog_Text.text = "";
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
        Chuugoku_text.alpha = 1;

        string Chu = null;
        Text Chtext = Chuugoku_text.GetComponent<Text>();
        Chtext.text = null;

        for(int i  = 0; i < tyu_amount; i++)
        {
            if((i + 1) % 7 == 0)
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
                int voicenum = Random.Range(0, 2);
                AudioManager.Instance.PlayVoice(voicenum);
            }

            AudioManager.Instance.PlaySE(6);

            Chu += "中";
            Chtext.text = Chu;
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlaySE(7);
        AudioManager.Instance.PlayVoice(7);
        Chu += "国";
        Chtext.text = Chu;
    }
    public IEnumerator Indicate_Dialog(int dialog_number)
    {
        AudioManager.Instance.PlaySE(3);

        Tutorial_Manager.instance.cantalk = false;
        string displaytext = null;
        Stop_and_hideUI(Triangle, 0f);

        for (int i = 0; i < dialogues[dialog_number].Length; i++)
        {

            displaytext += dialogues[dialog_number][i];
            Dialog_Text.text = displaytext;

            yield return new WaitForSeconds(0.05f);
        }
        FeedInUI(Triangle, 0.5f);
        AudioManager.Instance.SESource.Stop();

        Tutorial_Manager.instance.dialognum++;

        if(dialog_number < dialogues.Length)
        Tutorial_Manager.instance.cantalk = true;
    } //文字を表示するやつ
    public void Dialog_Feed()　//こっちはダイアログの本体自体を表示するやつ
    {
        FeedInUI(Dialog_CG, 0.5f);
        UpDown_Transform(zizii_transform);
    }

    public void Feedinright()
    {
        AudioManager.Instance.PlaySE(8);

        right_renderer.gameObject.SetActive(true);
        right_renderer.DOColor(new Color(0, 0, 0, 255), 0.5f).Play();
    }

    public void Hideright()
    {
        right_renderer.DOKill();
        right_renderer.gameObject.SetActive(false);
    }
}
