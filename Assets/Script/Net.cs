using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : MonoBehaviour
{
    //２つの状態をInspector上で設定できるように
    public GameObject Net_open;
    public GameObject Net_close;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ナマケモノが網を通過したら
    void OnTriggerEnter2D(Collider2D collision)
    {
        //開いている網を見えなく、閉じている網を見えるようにする
        if (collision.gameObject.CompareTag("Namakemono"))
        {
            Net_open.SetActive(false);
            Net_close.SetActive(true);
        }
    }
}
