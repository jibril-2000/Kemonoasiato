using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ferriswheel_spin : MonoBehaviour
{
    public GameObject red;
    public GameObject blue;
    public GameObject yellow;
    public GameObject green;
    public GameObject lightblue;
    public GameObject Area;
    public bool click;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (click == true)
        {
            if (Input.GetMouseButton(1))
            {
                transform.Rotate(new Vector3(0, 0, 0.3f));
                red.transform.Rotate(new Vector3(0, 0, -0.3f));
                blue.transform.Rotate(new Vector3(0, 0, -0.3f));
                yellow.transform.Rotate(new Vector3(0, 0, -0.3f));
                green.transform.Rotate(new Vector3(0, 0, -0.3f));
                lightblue.transform.Rotate(new Vector3(0, 0, -0.3f));
                if (Input.GetMouseButtonUp(1))
                {
                    click = false;
                }
            }
            else if (Input.GetMouseButton(0))
            {
                transform.Rotate(new Vector3(0, 0, -0.3f));
                red.transform.Rotate(new Vector3(0, 0, 0.3f));
                blue.transform.Rotate(new Vector3(0, 0, 0.3f));
                yellow.transform.Rotate(new Vector3(0, 0, 0.3f));
                green.transform.Rotate(new Vector3(0, 0, 0.3f));
                lightblue.transform.Rotate(new Vector3(0, 0, 0.3f));
                if (Input.GetMouseButtonUp(0))
                {
                    click = false;
                }
            }
            
            
        }
        
    }
    private void OnMouseEnter()
    {
        Area.SetActive(true);
        click = true;
        
    }

    private void OnMouseDrag()
    {
        Area.SetActive(true);

        
        
    }

    private void OnMouseExit()
    {
        Area.SetActive(false);
        click = false;
        if (Input.GetMouseButton(1) || Input.GetMouseButton(0))
        {
            click = true;
        }
      
       
        
        
    }
}
