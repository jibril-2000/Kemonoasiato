using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vents_Right : MonoBehaviour
{
    public GameObject vents_left;
    public GameObject vents_right;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {

            vents_left.SetActive(true);
            Debug.Log("a");
            vents_right.SetActive(false);
        }
    }
}
