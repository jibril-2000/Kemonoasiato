using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_Control : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //サウンド再生
        CriAtomSource audio = (CriAtomSource)GetComponent("CriAtomSource");
        audio.Play();
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
