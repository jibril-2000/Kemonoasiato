using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Namakemonopachinko : MonoBehaviour
{
    public GameObject pachinkostart;

    Rigidbody2D rigid2d;
    Vector2 startPos;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        this.rigid2d = GetComponent<Rigidbody2D>();
        this.speed = 500;
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
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Pachinko")
        {
            if (Input.GetMouseButtonDown(1))
            {
                this.startPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                Vector2 endPos = Input.mousePosition;
                Vector2 startDirection = -1 * (endPos - startPos).normalized;
                this.rigid2d.AddForce(startDirection * speed);
            }
        }
    }


    void FixedUpdate()
    {
        this.rigid2d.velocity *= 0.995f;
    }
}
