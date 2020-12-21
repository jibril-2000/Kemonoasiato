using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Namakemono : MonoBehaviour
{
    
    public Rigidbody2D rb2D;
    public Vector3 Resporn;
    Animation anim;
    public GameObject Groud;
    public GameObject CatchNet;
    // Start is called before the first frame update
    void Start()
    {
        anim = CatchNet.gameObject.GetComponent<Animation>();
        rb2D = GetComponent<Rigidbody2D>();
        Resporn = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Flashlight")
        {
            Invoke("Vanish", 1);
        }
    }

    void Vanish()
    {
        anim.Play();
        Groud.GetComponent<Ground>().StartCoroutine("Resporn");

    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Elephant")
        {
            rb2D.AddForce(new Vector2(10, 75));
        }
    }
}
