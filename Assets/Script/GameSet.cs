using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSet : MonoBehaviour
{
    public GameObject Game2Button;
    public GameObject Game3Button;
    public GameObject Game2ButtonText;
    public GameObject Game3ButtonText;
    GameObject StageObj;
    StageSelect script;
    // Start is called before the first frame update
    void Start()
    {
        StageObj = GameObject.Find("StageObj");
        script = StageObj.GetComponent<StageSelect>();
        Game2Button.gameObject.SetActive(false);
        Game3Button.gameObject.SetActive(false);
        Game2ButtonText.gameObject.SetActive(false);
        Game3ButtonText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
        if (script.Stage2Flag == true)
        {
            Game2Button.gameObject.SetActive(true);
            Game2ButtonText.gameObject.SetActive(true);
        }
        if (script.Stage3Flag == true)
        {
            Game3Button.gameObject.SetActive(true);
            Game3ButtonText.gameObject.SetActive(true);
        }
    }
}
