using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keeper : MonoBehaviour
{
    bool Keepermove = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (Keepermove)
        {
            case true:
                transform.position += new Vector3(1.0f * Time.deltaTime, 0.0f, 0.0f);
                if (transform.position.x >= 8.0)
                    Keepermove = false;
                break;

            case false:
                transform.position -= new Vector3(1.0f * Time.deltaTime, 0.0f, 0.0f);
                if (transform.position.x <= 4.0)
                    Keepermove = true;
                break;
        }
    }
}