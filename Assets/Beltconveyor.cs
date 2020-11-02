using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beltconveyor : MonoBehaviour
{
    public bool IsOn = false;
    public float TargetDriveSpeed = 3.0f;
    public float CurrentSpeed { get { return _currentSpeed; } }
    public Vector3 DriveDirection = Vector3.forward;
    [SerializeField] private float _forcePower = 50f;
    private float _currentSpeed = 0;
    private List<Rigidbody> _rigidbodies = new List<Rigidbody>();

    // Start is called before the first frame update
    void Start()
    {
        //方向は正規化
        DriveDirection = DriveDirection.normalized;
    }

    void FixedUpdate()
    {
        _currentSpeed = IsOn ? TargetDriveSpeed : 0;

        _rigidbodies.RemoveAll(r => r == null);

        foreach(var r in _rigidbodies)
        {
            var objectSpeed = Vector3.Dot(r.velocity, DriveDirection);
            if(objectSpeed < Mathf.Abs(TargetDriveSpeed))
            {
                r.AddForce(DriveDirection * _forcePower,
                           ForceMode.Acceleration);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        var rigidBody = collision.gameObject.GetComponent<Rigidbody>();
        _rigidbodies.Add(rigidBody);
    }

    void OnCollisionExit(Collision collision)
    {
        var rigidbody = collision.gameObject.GetComponent<Rigidbody>();
        _rigidbodies.Remove(rigidbody);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
