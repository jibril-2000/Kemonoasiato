using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimic_UI : MonoBehaviour
{
    public GameObject UI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //マウスが乗っている間、呼び出され続ける
    void OnMouseOver()
    {
        UI.SetActive(true);
    }

    //マウスが離れたとき
    void OnMouseExit()
    {
        UI.SetActive(false);
    }
}
