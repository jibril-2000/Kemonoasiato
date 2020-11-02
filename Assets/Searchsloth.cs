using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Searchsloth : MonoBehaviour
{

        [SerializeField]
        private Searchsloth searchsloth;
        [SerializeField]
        private SphereCollider Searcharea;
        [SerializeField]
        private float searchAngle = 130f;

        // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Sloth")
        {
            //　主人公の方向
            var playerDirection = other.transform.position - transform.position;
            //　敵の前方からの主人公の方向
            var angle = Vector3.Angle(transform.forward, playerDirection);
            //　サーチする角度内だったら発見
            if (angle <= searchAngle)
            {
                Debug.Log("主人公発見: " + angle);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Sloth")
        {
            //searchsloth.SetState(Searchsloth.EnemyState.Wait);
        }
    }
#if UNITY_EDITOR
    //　サーチする角度表示
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward, searchAngle * 2f, Searcharea.radius);
    }
#endif
}

