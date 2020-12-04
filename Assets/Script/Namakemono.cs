using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Namakemono : MonoBehaviour
{
    public GameObject namakemono;
    public GameObject gameover;
    public GameObject find;
    public Rigidbody2D rb2D;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
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
        gameover.SetActive(true);
        namakemono.SetActive(false);
        find.SetActive(false);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Elephant")
        {
            rb2D.AddForce(new Vector2(10, 75));
        }
    }
}
