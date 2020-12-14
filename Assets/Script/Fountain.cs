using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fountain : MonoBehaviour
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
        //はじめは何も作動してないようにする
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Water"))
        {          
            animator.SetBool("Water", false);
            return;
        }
        
    }

    //マウスが噴水に合わせてあるとき
    private void OnMouseOver()
    {
        //右クリックで
        if (Input.GetMouseButton(1))
        {
            animator.SetBool("Water", true);
        }

        //左クリックで
        if(Input.GetMouseButton(0))
        {
            animator.SetBool("Reverse", true);
            animator.SetBool("Water", false);
        }
        else
        {
            animator.SetBool("Reverse", false);
        }
    }
}
