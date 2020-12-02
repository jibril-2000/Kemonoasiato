using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ivy : MonoBehaviour
{
    Rigidbody2D Rigid;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
            Rigid.bodyType = RigidbodyType2D.Dynamic;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("a");
        if (other.gameObject.tag == "Namakemono")
        {
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
            var box=this.GetComponent<BoxCollider2D>();
            box.enabled = false;
            //StartCoroutine("MOVE", other.gameObject);

        }
    }
    private IEnumerator MOVE(GameObject other)
    {
        
        
        yield return new WaitForSeconds(10);
        

    }
       
}
