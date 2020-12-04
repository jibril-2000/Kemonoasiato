using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearSupin : MonoBehaviour
{
    float rotation_speed = 0; // 回転速度

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.rotation_speed = -5.0f;
        transform.Rotate(0, 0, this.rotation_speed);
    }
}
