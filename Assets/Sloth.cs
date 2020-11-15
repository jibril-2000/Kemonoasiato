using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sloth : MonoBehaviour
{
    public GameObject sloth;
    public GameObject gameover;
    public GameObject find;
    // Start is called before the first frame update
    void Start()
    {
        
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
        sloth.SetActive(false);
        find.SetActive(false);
    }
}
