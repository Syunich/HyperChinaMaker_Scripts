using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject Tyuutyuu_Button;
    private void Awake()
    {
        if(Tyuutyu_Flag.ClearNormal)
        {
            Tyuutyuu_Button.SetActive(true);
        }
        else
        {
            Tyuutyuu_Button.SetActive(false);
        }
    }

}
