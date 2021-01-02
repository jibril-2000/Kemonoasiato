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
    bool OnMouse = false;
    float alpha = 0;

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
                //UI.SetActive(false);
                timer = 0;
            }
        }
    }

    void OnMouseEnter()
    {
        if (OnMouse == false)
        {

            StartCoroutine("Pop");
            //StopCoroutine("Negate");
            //Invoke("Vanish", 1);
        }
        OnMouse = true;




    }
    //マウスが乗っている間、呼び出され続ける
    void OnMouseOver()
    {
        StopCoroutine("Negate");
    }

    void Vanish()
    {
        UI.SetActive(true);
    }

    //マウスが離れたとき
    void OnMouseExit()
    {
        
        StartCoroutine("Negate");
        OnMouse = false;

    }
    public IEnumerator Pop()
    {
        StopCoroutine("Negate");
        for (float i = 0; i < 1f; i = i + 0.1f)
        {
            UI.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, alpha+=0.1f);

            Debug.Log(UI.GetComponent<Image>().color);
            alpha = alpha + i;
            if (alpha > 1.1f)
            {
                alpha = 1f;
            }
            yield return new WaitForSeconds(0.1f);//何秒待つのか
            
        }
        
    }
    public IEnumerator Negate()
    {
        yield return new WaitForSeconds(0.5f);//何秒待つのか
        for (float i = 0; i <100000; i = i + 0.1f)
        {
            UI.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, alpha-=i);
            Debug.Log(UI.GetComponent<Image>().color);
            alpha = alpha - i;
            if (alpha<-0.1f)
            {
                alpha = 0;
                //StopCoroutine("Negate");
            }
            yield return new WaitForSeconds(0.1f);//何秒待つのか
        }

    }
}
