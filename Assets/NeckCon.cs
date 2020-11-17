using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeckCon : MonoBehaviour
{
    Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponent<Animator>();
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Namakemono"))
        {
            Debug.Log("触れた");
            
                animator.SetBool("Neckstart", true);
            
        }
    }
}
