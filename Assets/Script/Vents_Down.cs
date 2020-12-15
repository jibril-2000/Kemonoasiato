using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vents_Down : MonoBehaviour
{
    public GameObject vents_up;
    public GameObject vents_down;

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
            vents_up.SetActive(true);
            vents_down.SetActive(false);
        }
    }
}
