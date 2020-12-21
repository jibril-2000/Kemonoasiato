using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ivy : MonoBehaviour
{
    Rigidbody2D Rigid;
    Vector3 bulletDir ;
    public float speed;
    public bool a;
    bool Touch;
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
            Touch = true;
            other.gameObject.transform.parent = this.gameObject.transform;
            Rigid=other.gameObject.GetComponent<Rigidbody2D>();
            Rigid.bodyType = RigidbodyType2D.Static;
            

        }
        
            
        
    
    }
    void OnTriggerStay2D(Collider2D other) 
    {
        if (Input.GetMouseButton(1))
        {
            other.transform.parent = null;
            Rigid.gravityScale = 1;
            Rigid.AddForce(bulletDir * speed);
            var box=this.GetComponent<BoxCollider2D>();
            box.enabled = false;
           
            StartCoroutine("MOVE");

        }
    }
    private IEnumerator MOVE()
    {
        
        
        yield return new WaitForSeconds(3);
        var box = this.GetComponent<BoxCollider2D>();
        box.enabled = true;

    }
       
}
