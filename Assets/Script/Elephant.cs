using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elephant : MonoBehaviour
{
    Animation anim;
    // Start is called before the first frame update
    void Start()
    {
        this.anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            this.anim.Play("Elephant");
        }
    }
}
