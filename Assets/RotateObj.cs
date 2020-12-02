using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObj : MonoBehaviour
{
    Vector3 firstPos;
    Vector3 lastPos;

    private Renderer _renderer; 

    void Start()
    {
        _renderer = GetComponent<Renderer>(); //
        _renderer.enabled = false; //trueで表示
    }

    // Update is called once per frame
    void OnMouseEnter()
    {
        _renderer.enabled = true;
    }

    void OnMouseDrag()
    {

        _renderer.enabled = true;
        float sensitivity = 1f; // いわゆるマウス感度
        float mouse_move_x = Input.GetAxis("Mouse X")*2 ;
        float mouse_move_y = Input.GetAxis("Mouse Y")*2 ;
        if (Input.GetMouseButtonDown(0))
        {
            firstPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            lastPos = Input.mousePosition;
        }

            var dic = lastPos - firstPos;

            if (dic.y > 1.0f)
            {
                gameObject.transform.parent.Rotate(0,0, mouse_move_x+ mouse_move_y);
            }
           
        
    }
    void OnMouseExit()
    {
        _renderer.enabled = false;
    }

}

