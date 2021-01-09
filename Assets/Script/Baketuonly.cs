using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baketuonly : MonoBehaviour
{
    public GameObject BaketuUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Vanish()
    {
        Destroy(BaketuUI, 3);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Namakemono"))
        {
            BaketuUI.SetActive(true);
            Invoke("Vanish", 1);
        }
    }
}
