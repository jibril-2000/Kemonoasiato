using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] bgmList;//BGMを読み込む
    [SerializeField] AudioSource audioSourceBGM;//BGMの音の大きさを調節するために読み込む

    //SecneをまたいでもObjectが破壊されないようにする
    static AudioManager Instance = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //BGMのボリューム調節する関数を作成
    public float BGMVolume
    {
        //audioSourceBGMのvolumeをいじることでBGMを調整
        get { return audioSourceBGM.volume; }
        set { audioSourceBGM.volume = value; }
    }

    public static AudioManager GetInstance()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<AudioManager>();
        }
        return Instance;
    }

    private void Awake()
    {
        if (this != GetInstance())
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    //BGMを再生する関数を作成
    public void PlayBGM(int index)
    {
        audioSourceBGM.clip = bgmList[index];
        audioSourceBGM.Play();
    }
}
