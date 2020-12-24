using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gondola : MonoBehaviour
{
    Rigidbody2D Rigid;
    Vector3 GondolaPos;
    GameObject Set;
    bool Touch;
    bool Return;
    float Velocity;
    float degree;
    public GameObject parentObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        GondolaPos = parentObj.gameObject.transform.position;
        //GondolaPos.y += 0.00001f;
        if (Touch == true)
        {
            if (Input.GetMouseButton(1))
                Rigid.bodyType = RigidbodyType2D.Dynamic;
            
          
            
            
            //this.transform.eulerAngles = new Vector3(0f, 0f, Mathf.LerpAngle(this.transform.eulerAngles.z, degree, Time.deltaTime * 1f));

           
        }
        if (Return == true)
        {
            StartCoroutine("Godolafinish");
            
            Return = false;
        }
        //if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Gondola"))
        {
            //this.GetComponent<Animator>().SetBool("GondolaStart", false);
            
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Namakemono"))
        {
            Set = other.gameObject;
            other.gameObject.transform.parent = this.gameObject.transform;
            Rigid = other.gameObject.GetComponent<Rigidbody2D>();
            Rigid.bodyType = RigidbodyType2D.Static;
            other.gameObject.transform.position = GondolaPos;
            this.GetComponent<PolygonCollider2D>().enabled=false;
            Touch = true;
        }

    }
    

    void OnMouseOver()
    {
        if (Touch == true)
        {
            if (Input.GetMouseButtonDown(1))
            {
                //StartCoroutine("GodolaStart");


                Set.transform.parent = null;
                StartCoroutine("GodolaStart");
                Rigid.gravityScale = 1;
                




            }

        }
        

    }
    public IEnumerator GodolaStart()
    {
        /***Touch = true;
        this.GetComponent<Animator>().enabled = true;
        this.GetComponent<Animator>().SetBool("GondolaStart", true);
        
        this.GetComponent<PolygonCollider2D>().enabled = true;
        this.GetComponent<Animator>().enabled = false;***/
        yield return new WaitForSeconds(0.5f);//何秒待つのか
        degree = this.transform.localEulerAngles.z - 90;
        for (int i = 0; i < 36; i++)
        {
            transform.Rotate(new Vector3(0, 0, -2.5f));
            yield return new WaitForSeconds(0.01f);//何秒待つのか
        }
        Touch = false;
        yield return new WaitForSeconds(1f);//何秒待つのか
        Return = true;
        
    }
    public IEnumerator Godolafinish()
    {
        
        yield return new WaitForSeconds(0.5f);//何秒待つのか
        degree = this.transform.localEulerAngles.z - 90;
        for (int i = 0; i < 36; i++)
        {
            transform.Rotate(new Vector3(0, 0, 2.5f));
            yield return new WaitForSeconds(0.01f);//何秒待つのか
        }
        Touch = false;
        yield return new WaitForSeconds(1f);//何秒待つのか
        this.GetComponent<PolygonCollider2D>().enabled = true;
       
        
    }
}
