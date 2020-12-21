using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truckgoal : MonoBehaviour
{
    Animation anim;
    public GameObject karigoal;
    // Start is called before the first frame update
    void Start()
    {
        anim = karigoal.gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Namakemono")
        {
            anim.Play();
        }
    }
}
