using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeckCon : MonoBehaviour
{
    Animator animator;
    public GameObject Giraffe;
    // Start is called before the first frame update
    void Start()
    {
        animator = Giraffe.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Neckanim"))
        {
            //animator.SetBool("Neckstart", false);
            return;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag=="Namakemono")
        {
            Debug.Log("触れた");
            
                animator.SetBool("Neckstart", true);
            
        }
    }
}
