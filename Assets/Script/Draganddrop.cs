using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draganddrop : MonoBehaviour
{
    private Renderer _renderer;
    public GameObject RotationUI;
    public GameObject MoveArea;
    public Vector3 minVec;
    public Vector3 maxVec;
    private Vector3 Pos;
    public bool Moveobj;
    bool Move;
    void Start()
    {
        MoveArea.SetActive(false);
        _renderer = RotationUI.GetComponent<Renderer>(); 
        Pos = this.gameObject.transform.position;
        maxVec.x = Pos.x+maxVec.x ;
        minVec.x = Pos.x - minVec.x;
        _renderer.enabled = false; //trueで表示
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

        if (Move == true)
        {
            if (Input.GetMouseButton(0))
            {
                MoveArea.SetActive(true);
            }
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
    void OnMouseEnter()
    {
        _renderer.enabled = true;
        
    }
    void OnMouseExit()
    {
        StartCoroutine("Negative");
    }
    void OnMouseDrag()
    {
        if (Moveobj == true)
        {
            
            Move = true;
            _renderer.enabled = true;
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
   
    public IEnumerator Negative()
    {

        
        yield return new WaitForSeconds(0.2f);
        _renderer.enabled = false;
        MoveArea.SetActive(false);
        Move = false;
    }

}