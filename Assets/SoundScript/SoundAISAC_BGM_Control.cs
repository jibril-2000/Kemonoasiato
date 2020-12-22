using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundAISAC_BGM_Control : MonoBehaviour
{
    private Slider BGM_Slider;
    private float Slider_num;

    // Start is called before the first frame update
    void Start()
    {

        BGM_Slider = GetComponent<Slider>();

        //スライダーの最大値最小値設定
        float MaxVolume = 1.0f;
        float MinVolume = 0.0f;

        BGM_Slider.maxValue = MaxVolume;
        BGM_Slider.minValue = MinVolume;


        //BGM音量のデフォルト値
        Slider_num = 0.5f;
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
       //スライダーの値をAISACに入れる
        Slider_num = GetComponent<Slider>().value;
        CriAtomExCategory.SetAisac(2, 1, Slider_num); //カテゴリID 3のAISACControlID3にAISAC値をセット
    }
}
