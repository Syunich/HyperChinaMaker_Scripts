using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;
using DG.Tweening;

public class Chinaland_Manager : MonoBehaviour
{
    /// <summary>
    /// 全ての中国を扱うクラス
    /// </summary>

    [SerializeField] Vector3 NomalPosition;
    [SerializeField] Vector3 GamePosition;
    [SerializeField] GameObject ChinaPrefab;
    [SerializeField] private int Extend_amout; //中国領土をスポーンするときの拡大倍率

    public Color[] ChinaColor; //中国領土の色(クリア回数により変化)
    public GameObject CurrentChina; //現在一番先頭にきてる中国領土が入る
    public GameObject BeforeChina; //1つ前の中国領土が入る(比較用)

    public float Sabun
    {
        get { return CurrentChina.transform.lossyScale.y * 100.0f / BeforeChina.transform.lossyScale.x; }
    }

    void Start()
    {
        CurrentChina = this.gameObject;
        BeforeChina = this.gameObject;
    }

    public void SetNormalPosition()
    {
        transform.DOMove(NomalPosition, 1.0f).Play();
    }

     public void SetGamePosition()
    {
        transform.DOMove(GamePosition, 1.0f).Play();
    }

     public void StartWave()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(GamePosition, 1.0f))
           .OnComplete(() => SpownChinainflont())
           .Play();
    }

    //大きな中国全土を出現させる(CurrentChinaを上書きする)
    public void SpownChinainflont()
    {
        //スポーン条件(位置、色、大きさ)
        BeforeChina = CurrentChina;
        GameObject gm = Instantiate(ChinaPrefab, GamePosition, Quaternion.Euler(0, 0, 0));
        CurrentChina = gm; 

        gm.transform.localScale = Vector3.one * Extend_amout;
        gm.GetComponent<SpriteRenderer>().color = ChinaColor[GameManager.GMInstance.num_of_clear % (ChinaColor.Length )];

        gm.transform.SetParent(BeforeChina.transform); //現在の中国の子にする

        FeedIn(gm, 2.0f);
        Shrink(gm, 4.0f);

        GameManager.GMInstance.isgaming = true;
    }

    //ゲームオブジェクトを縮小させていく
    public void Shrink(GameObject gm, float sec)
    {
        gm.transform.DOScale(Vector3.zero, sec).SetEase(Ease.InCubic)
            .OnComplete(() => IsCurrntSmall()).Play();
    }

    //ゲームオブジェクトを透明→非透明にする
    public void FeedIn(GameObject gm, float sec)
    {
        SpriteRenderer spr = gm.GetComponent<SpriteRenderer>();
        spr.DOFade(0, 0f).Play(); //一度透明にする
        spr.DOFade(0.6f, sec).Play();
    }

    public void StopChina(GameObject china)
    {
        china.transform.DOKill();
        
        china.GetComponent<SpriteRenderer>().DOFade(1, 0.5f).Play();
    }

    //現在の中国領土が、1つ前の中国領土より小さいならtrueを返す。
    public bool IsCurrntSmall() 
    {
        Vector3 CurrentScale = CurrentChina.transform.lossyScale;
        Vector3 BeforeScale = BeforeChina.transform.lossyScale;
        if(CurrentScale.x <= BeforeScale.x && CurrentScale.x > 0.000005f)
        {
            return true;
        }
        else
        {
            return false;
        }
               
    }

    ///
    /// ここから下チュートリアル用
    ///
    public int count; //チュートリアルの回数によって挙動変更
    Vector3 ChinaScale = Vector3.one * 0.95f;
    public void StartWave_TR()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(GamePosition, 1.0f))
           .OnComplete(() =>StartCoroutine( SpownChinainflont_TR() ))
           .Play();
    }

    IEnumerator SpownChinainflont_TR()
    {
        //スポーン条件(位置、色、大きさ)
        BeforeChina = CurrentChina;
        GameObject gm = Instantiate(ChinaPrefab, GamePosition, Quaternion.Euler(0, 0, 0));
        CurrentChina = gm;

        gm.transform.localScale = Vector3.one * Extend_amout;
        gm.GetComponent<SpriteRenderer>().color = ChinaColor[count++];

        gm.transform.SetParent(BeforeChina.transform); //現在の中国の子にする

        FeedIn(gm, 1.0f);
        if(count == 1)
        {
            yield return StartCoroutine(Shrink_TR(gm, 2.0f, ChinaScale));
            UI_Tutorial.instance.Dialog_Feed();
            Alpha1(CurrentChina , 0.5f);
            yield return new WaitForSeconds(0.5f);
            UI_Tutorial.instance.Feedinright();
            yield return StartCoroutine(UI_Tutorial.instance.Indicate_Dialog(Tutorial_Manager.instance.dialognum));
        }
        if(count == 2)
        {
            yield return StartCoroutine(Shrink_TR(gm, 2.0f, ChinaScale*0.95f));
            UI_Tutorial.instance.Feedinright();
            Alpha1(CurrentChina, 0.5f);
        }
        Tutorial_Manager.instance.isgaming = true;
    }

    IEnumerator Shrink_TR(GameObject gm , float sec , Vector3 scale)
    {
       yield return gm.transform.DOScale(scale, sec).SetEase(Ease.InCubic).Play().WaitForCompletion();
    }

    public void Alpha1(GameObject gm , float sec)
    {
        SpriteRenderer spr = gm.GetComponent<SpriteRenderer>();
        spr.DOFade(1, sec).Play(); 
    }

}
