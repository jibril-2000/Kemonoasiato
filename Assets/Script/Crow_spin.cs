using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow_spin : MonoBehaviour
{
    [SerializeField, Header("巡回場所")]
    private Transform[] _targetpoints;
    [SerializeField, Header("移動速度")]
    private float _speed;

    private int _points = 0;

    private void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        var vec = _targetpoints[_points].position - transform.position;
        //vec.y = 0;
        transform.position += vec.normalized * _speed * Time.deltaTime;

        if(vec.magnitude < 0.1f)
        {
            _points = (_points + 1) % 4;
        }
        else if (vec.magnitude < -0.1f)
        {
            _points = (_points + -1) % 4;
        }
    }
}
