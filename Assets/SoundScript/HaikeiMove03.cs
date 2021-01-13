using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaikeiMove03 : MonoBehaviour
{
    public GameObject Haikei01, Haikei02, Haikei03, Camera;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 CameraPos = Camera.transform.position;

        Haikei01.transform.position = new Vector3(CameraPos.x / 2f  -20f, CameraPos.y + 0.5f, 0f);
        Haikei02.transform.position = new Vector3(CameraPos.x / 3 - 5f, CameraPos.y - 1, 0f);
        Haikei03.transform.position = new Vector3(CameraPos.x / 4 - 20f, CameraPos.y - 1, 0f);
    }
}
