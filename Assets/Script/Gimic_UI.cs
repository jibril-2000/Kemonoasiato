using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gimic_UI : MonoBehaviour
{
    public GameObject UI;
    public float interval = 1.5f;
    float timer;
    public float speed = 0.01f;
    /*

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (UI.activeSelf)
        {
            timer += Time.deltaTime;

            if (timer > interval)
            {
                UI.SetActive(false);
                timer = 0;
            }
        }
    }

    //マウスが乗っている間、呼び出され続ける
    void OnMouseOver()
    {
        UI.SetActive(true);
    }
    */

    void OnMouseEnter()
    {
        UI.SetActive(true);
    }

    //マウスが離れたとき
    void OnMouseExit()
    {
        
    }
}
