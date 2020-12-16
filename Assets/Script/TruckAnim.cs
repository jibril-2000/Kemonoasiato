using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckAnim : MonoBehaviour
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
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("TruckUP"))
        {
            animator.SetBool("Truck", false);
            return;
        }
    }

     private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetBool("Truck", true);
        }
    }
}

