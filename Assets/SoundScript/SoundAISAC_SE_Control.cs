using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundAISAC_SE_Control : MonoBehaviour
{
    private Slider SE_Slider;
    private static float SESlider_num = 0.5f;
    public GameObject SEVolumeControl;

    // Start is called before the first frame update
    void Start()
    {

        SE_Slider = SEVolumeControl.GetComponent<Slider>();
        /*
        //スライダーの最大値最小値設定
        float MaxVolume = 1.0f;
        float MinVolume = 0.0f;

        SE_Slider.maxValue = MaxVolume;
        SE_Slider.minValue = MinVolume;
        */

        SE_Slider.value = SESlider_num;
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        //スライダーの値をAISACに入れる
        SESlider_num = SEVolumeControl.GetComponent<Slider>().value;
        CriAtomExCategory.SetAisac(3, 2, SESlider_num); //カテゴリID 3のAISACControlID3にAISAC値をセット
    }
}
