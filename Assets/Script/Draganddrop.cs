using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draganddrop : MonoBehaviour
{
    public Vector3 minVec;
    public Vector3 maxVec;
    private Vector3 Pos;
    public bool Moveobj;
    void Start()
    {
        Pos = this.gameObject.transform.position;
        maxVec.x = Pos.x+maxVec.x ;
        minVec.x = Pos.x - minVec.x;
    }

    private void Update()
    {
        Pos = this.gameObject.transform.position;
        if (Pos.x>maxVec.x) 
        {
            Pos.x = maxVec.x - 1f;
            this.gameObject.transform.position = new Vector3(Pos.x, Pos.y, Pos.z);
        }
        if (Pos.x < minVec.x)
        {
            Pos.x = minVec.x + 1f;
            this.gameObject.transform.position = new Vector3(Pos.x, Pos.y, Pos.z);
        }

    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Namakemono")
        {
            Moveobj = false;
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Namakemono")
        {
            Moveobj = true;
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Namakemono")
        {
            Moveobj = false;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Namakemono")
        {
            Moveobj = true;
        }
    }
    void OnMouseDrag()
    {
        if (Moveobj == true)
        {
            Vector3 objectPointInScreen
                = Camera.main.WorldToScreenPoint(this.transform.position);

            Vector3 mousePointInScreen
                = new Vector3(Input.mousePosition.x,
                              Input.mousePosition.y,
                              objectPointInScreen.z);

            Vector3 mousePointInWorld = Camera.main.ScreenToWorldPoint(mousePointInScreen);
            mousePointInWorld.z = this.transform.position.z;
            this.transform.position = mousePointInWorld;
        }
    }

}