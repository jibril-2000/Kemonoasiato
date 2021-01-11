using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundAISAC_BGM_Control : MonoBehaviour
{
    private  Slider BGM_Slider;
    private static float  Slider_num = 0.5f;
    public GameObject BGMVolumeControl;

    // Start is called before the first frame update
    void Start()
    {
        BGM_Slider = BGMVolumeControl.GetComponent<Slider>();
        /*
        //スライダーの最大値最小値設定
        float MaxVolume = 1.0f;
        float MinVolume = 0.0f;

        BGM_Slider.maxValue = MaxVolume;
        BGM_Slider.minValue = MinVolume;

        */
        BGM_Slider.value = Slider_num;


    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        Slider_num = BGMVolumeControl.GetComponent<Slider>().value;
       //スライダーの値をAISACに入れる
        CriAtomExCategory.SetAisac(2, 1, Slider_num); //カテゴリID 2のAISACControlID1にAISAC値をセット
    }
}
