using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundFeeder : MonoBehaviour
{
    //BGM
    [SerializeField] Text bgmtext;
    [SerializeField] Slider bgmslider;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.GetInstance().PlayBGM(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnChangedBGMSlider()
    {
        //Sliderの値に応じてBGMを変更
        AudioManager.GetInstance().BGMVolume = bgmslider.value;
        //volumeTextの値をSliderのvalueに変更
        bgmtext.text = string.Format("{0:0}", bgmslider.value * 100);
    }
}
