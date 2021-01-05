using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    //数値が大きいと影響が大きい
    public float intensity;
    //風の向きと大きさ
    public Vector2 velocity;
    new Rigidbody2D rigidbody;
    Vector3 canonAngle;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        
       
        
        
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        rigidbody = collision.GetComponent<Rigidbody2D>();
        if(rigidbody == null)
        {
            return;
        }
        float canonRad = transform.parent.eulerAngles.z * Mathf.Deg2Rad;        // canonの傾きからラジアンを求めます
        Vector3 canonAngle = new Vector3(Mathf.Cos(canonRad), Mathf.Sin(canonRad), 0).normalized;
        //相対速度
        var relativeVelocity = velocity - rigidbody.velocity;
        
        
        //空気抵抗
        rigidbody.AddForce(canonAngle * intensity);
    }
}
