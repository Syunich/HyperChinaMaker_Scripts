using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sensu_Manager : MonoBehaviour
{
   public SENSUTYPE sensutype;
    CanvasGroup canvasgroup;

    private void Awake()
    {
        DG.Tweening.DOTween.SetTweensCapacity(tweenersCapacity: 2000, sequencesCapacity: 2000);
        canvasgroup = GetComponent<CanvasGroup>();
        canvasgroup.alpha = 0; //最初は透明化しておく
        canvasgroup.blocksRaycasts = false;
    }

    public void SetFadeOut() //これから隠しますよ～って時の最初の場所とか透明度設定
    {
        switch(sensutype)
        {
            case SENSUTYPE.LEFT:
                transform.rotation = Quaternion.Euler(0, 0, 80); break;
            case SENSUTYPE.RIGHT:
                transform.rotation = Quaternion.Euler(0, 0, -80); break;
        }
        canvasgroup.alpha = 1;
        canvasgroup.blocksRaycasts = true;
    }

    public void SetFadeIn() // これから晴れますよ~って時の最初の場所とか透明度設定
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        canvasgroup.alpha = 1;
        canvasgroup.blocksRaycasts = true;
    }

    public IEnumerator StartFadeOut_Moving() //隠すときの動き
    {
        yield return transform.DORotate(new Vector3(0, 0, 0), 1.5f, RotateMode.Fast).SetEase(Ease.OutCubic).Play().WaitForCompletion();
    }

    public IEnumerator StartFadeIn_Moving() //晴れるときの動き
    {
        switch (sensutype)
        {
            case SENSUTYPE.LEFT:
            yield return transform.DORotate(new Vector3(0, 0, -140), 1.5f, RotateMode.Fast).SetEase(Ease.InCubic).Play().WaitForCompletion(); break;

            case SENSUTYPE.RIGHT:
            yield return transform.DORotate(new Vector3(0, 0, 140), 1.5f, RotateMode.Fast).SetEase(Ease.InCubic).Play().WaitForCompletion(); break;
        }
    }

    public void CompleteFadeOut() //隠し完了時のパラメーター
    {
        SetFadeIn();
    }

    public void CompleteFadeIn() //晴れ完了時のパラメーター
    {
        switch (sensutype)
        {
            case SENSUTYPE.LEFT:
                transform.rotation = Quaternion.Euler(0, 0, 80); break;
            case SENSUTYPE.RIGHT:
                transform.rotation = Quaternion.Euler(0, 0, -80); break;
        }
        canvasgroup.alpha = 1;
        canvasgroup.blocksRaycasts = false;
    }
}

public enum SENSUTYPE
{
    LEFT ,
    RIGHT
}


