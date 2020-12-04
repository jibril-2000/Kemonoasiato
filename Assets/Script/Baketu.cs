using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baketu : MonoBehaviour
{
    Animator animator;
    private string fallStr = "Falldown";

    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsTag("Falldown"))
        {
            animator.SetBool("Falldown", false);
            return;
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Namakemono"))
        {
            if (Input.GetMouseButton(1))
            {
                animator.SetBool("Falldown", true);
            }
        }
    }
}
