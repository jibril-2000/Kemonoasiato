using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSet : MonoBehaviour
{
    public GameObject Game2Button;
    public GameObject Game3Button;
    GameObject StageObj;
    StageSelect script;
    // Start is called before the first frame update
    void Start()
    {
        StageObj = GameObject.Find("StageObj");
        script = StageObj.GetComponent<StageSelect>();
        Game2Button.gameObject.SetActive(false);
        Game3Button.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
        if (script.Stage2Flag == true)
        {
            Game2Button.gameObject.SetActive(true);
        }
        if (script.Stage3Flag == true)
        {
            Game3Button.gameObject.SetActive(true);
        }
    }
}
