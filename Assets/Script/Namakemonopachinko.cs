using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Namakemonopachinko : MonoBehaviour
{
    public GameObject pachinkostart;

    Rigidbody2D rigid2d;
    Vector2 startPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Pachinkobody")
        {
            transform.position = pachinkostart.transform.position;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Pachinko")
        {
            transform.rotation = Quaternion.identity;//向きを元に戻す
        }
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Pachinko")
        {

            if (Input.GetMouseButton(1))
            {
                GetComponent<Rigidbody2D>().velocity = (transform.up*9);//ナマケモノから見て上方向に力を加える
            }
        }
    }
}
