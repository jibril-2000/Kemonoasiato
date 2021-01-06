using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fauntain_water : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //噴水の水の当たり判定を消す
        GetComponent<Collider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //右クリックで当たり判定出現
        if (Input.GetMouseButton(1))
        {
            GetComponent<Collider2D>().enabled = true;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Namakemono"))
        {
            //左クリックで当たり判定消す
            if (Input.GetMouseButton(0))
            {
                GetComponent<Collider2D>().enabled = false;
            }
        }       
    }
}
