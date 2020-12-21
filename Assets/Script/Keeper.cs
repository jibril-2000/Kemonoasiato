using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keeper : MonoBehaviour
{
    bool Keepermove = true;
    public float Maxx;
    public float Minx;
    // Start is called before the first frame update
    void Start()
    {
        Maxx = this.transform.position.x + Maxx;
        Minx = this.transform.position.x - Minx;
    }

    // Update is called once per frame
    void Update()
    {
        switch (Keepermove)
        {
            case true:
                transform.position += new Vector3(1.0f * Time.deltaTime, 0.0f, 0.0f);
                if (transform.position.x >= Maxx)
                {
                    Keepermove = false;

                    transform.Rotate(new Vector2(0, 180));
                }

                break;

            case false:
                transform.position -= new Vector3(1.0f * Time.deltaTime, 0.0f, 0.0f);
                if (transform.position.x <= Minx)
                {
                    Keepermove = true;

                    transform.Rotate(new Vector2(0, -180));
                }

                break;

        }
    }
}
