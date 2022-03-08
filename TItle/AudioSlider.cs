using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SESlider;
    private int count = 0;
    private int countcontainer = 0;
    private void Start()
    {
        BGMSlider.value = AudioManager.Instance.BGMSource.volume;
        SESlider.value = AudioManager.Instance.SESource.volume;
    }

    private void FixedUpdate()
    {
        count++;
    }

    public void ChangeBGMvol()
    {
        AudioManager.Instance.BGMSource.volume = BGMSlider.value;

    }
    public void ChangeSEvol()
    {
        AudioManager.Instance.SESource.volume = SESlider.value;
        if (count - countcontainer > 30)
        {
            AudioManager.Instance.PlaySE(4);
            countcontainer = count;
        }
    }
}
