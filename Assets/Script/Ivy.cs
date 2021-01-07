using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ivy : MonoBehaviour
{
    Rigidbody2D Rigid;
    Vector3 bulletDir ;
    public float speed;
    public bool a;
    bool Touch;
    public GameObject UI;
    float alpha = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Touch == true)
        {
            if (Input.GetMouseButton(1))
                Rigid.bodyType = RigidbodyType2D.Dynamic;
        }
        switch (a)
        {
            case true:
                bulletDir = new Vector3(-1, 0, 0);
                break;

            case false:
                bulletDir = new Vector3(1, 0, 0);
                break;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("a");
        if (other.gameObject.tag == "Namakemono")
        {
            StartCoroutine("Pop");
            Touch = true;
            other.gameObject.transform.parent = this.gameObject.transform;
            Rigid=other.gameObject.GetComponent<Rigidbody2D>();
            Rigid.bodyType = RigidbodyType2D.Static;
            
        }
        
            
        
    
    }
    void OnTriggerStay2D(Collider2D other) 
    {
        UI.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f,1.0f);
        if (Input.GetMouseButton(1))
        {
            
            other.transform.parent = null;
            Rigid.gravityScale = 1;
            Rigid.AddForce(bulletDir * speed);
            other.transform.eulerAngles = new Vector3(0, 0, 0);
            var box=this.GetComponent<BoxCollider2D>();
            box.enabled = false;
            StartCoroutine("Negate");
            StartCoroutine("MOVE");
            
        }

        
    }
    void OnTriggerExit2D(Collider2D other)
    {
       

            //
            

        
    }
    private IEnumerator MOVE()
    {
        
        
        yield return new WaitForSeconds(5);
        var box = this.GetComponent<BoxCollider2D>();
        box.enabled = true;

    }
    public IEnumerator Pop()
    {
        StopCoroutine("Negate");
        for (float i = 0; i < 1f; i = i + 0.1f)
        {
            UI.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, alpha += 0.1f);

            //Debug.Log(UI.GetComponent<Image>().color);
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
        for (float i = 0; i < 1f; i = i + 0.1f)
        {
            UI.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, alpha -= i);
            //Debug.Log(UI.GetComponent<Image>().color);
            alpha = alpha - i;
            if (alpha < -0.1f)
            {
                alpha = 0;
                //StopCoroutine("Negate");
            }
            yield return new WaitForSeconds(0.1f);//何秒待つのか
        }

    }

}
