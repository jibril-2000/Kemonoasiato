using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tranpoline : MonoBehaviour
{
    [SerializeField] GameObject Tranpoline;
    private Animator Animcon;
    [SerializeField] GameObject Animobj;
    private int Count = 0;
    // Start is called before the first frame update
    void Start()
    {
        Animcon = Animobj.GetComponent<Animator>();
    }
    void Update()
    {
        if (Animcon.GetCurrentAnimatorStateInfo(0).IsTag("Tranpoline"))
        {
            Animcon.SetBool("TouchTranpoline", false);

            return;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Namakemono")
        {
            Animcon.SetBool("TouchTranpoline", true);
            //サウンド再生
            CriAtomSource audio = (CriAtomSource)GetComponent("CriAtomSource");
            audio.Play();
            Tranpoline.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
