using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameManager : MonoBehaviour
{
    int Flamewidth; //中国フレームの長さ。Startで取得
    void Start()
    {
        Flamewidth = (int)GetComponent<RectTransform>().sizeDelta.x;
        Flamewidth = -Flamewidth;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        MovingFlame();
    }

    //タイトル上下の中国風フレームを動かす関数
    private void MovingFlame()
    {
        transform.localPosition = new Vector3(transform.localPosition.x - 1f , transform.localPosition.y , transform.localPosition.z);
        if(transform.localPosition.x <= Flamewidth)
        {
            transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
        }
    }
}
