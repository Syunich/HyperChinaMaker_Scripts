using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Button_Manager : MonoBehaviour
{
    private int counter;

    private void FixedUpdate()
    {
        counter++;
    }

    public void OnPointEnter()
    {
        if (counter > 10)
        {
            AudioManager.Instance.PlaySE(2);
        }
    }

    public void OnPointExit()
    {

    }

}
