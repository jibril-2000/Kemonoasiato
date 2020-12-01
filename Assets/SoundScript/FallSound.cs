using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallSound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Namakemono")
        {
            //サウンド再生
            CriAtomSource audio = (CriAtomSource)GetComponent("CriAtomSource");
            audio.Play();
        }
    }
}
