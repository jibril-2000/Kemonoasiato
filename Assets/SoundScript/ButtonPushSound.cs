using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPushSound : MonoBehaviour
{
    public CriAtomSource criAtomSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void PlaySound()
    {
        //サウンド再生
        criAtomSource.Play();

    }
}
