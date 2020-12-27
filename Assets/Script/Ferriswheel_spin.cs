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
        Area.SetActive(true);

        if (Input.GetMouseButton(1))
        {
            transform.Rotate(new Vector3(0, 0, 0.3f));
            red.transform.Rotate(new Vector3(0, 0, -0.3f));
            blue.transform.Rotate(new Vector3(0, 0, -0.3f));
            yellow.transform.Rotate(new Vector3(0, 0, -0.3f));
            green.transform.Rotate(new Vector3(0, 0, -0.3f));
            lightblue.transform.Rotate(new Vector3(0, 0, -0.3f));
        }
        else if(Input.GetMouseButton(0))
        {
            transform.Rotate(new Vector3(0, 0, -0.3f));
            red.transform.Rotate(new Vector3(0, 0, 0.3f));
            blue.transform.Rotate(new Vector3(0, 0, 0.3f));
            yellow.transform.Rotate(new Vector3(0, 0, 0.3f));
            green.transform.Rotate(new Vector3(0, 0, 0.3f));
            lightblue.transform.Rotate(new Vector3(0, 0, 0.3f));
        }
        else
        {
            
        }
    }

    private void OnMouseExit()
    {
        Area.SetActive(false);
    }
}
