using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kanransya : MonoBehaviour
{
    public GameObject object1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = object1.transform.position;
    }
}
