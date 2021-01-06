using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
   
    public bool GameStart;
    public bool Stage1; 
    public bool Stage2;
    public bool Stage3;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    public void OnClick()
    {
        if (GameStart == true)
        { 
            SceneManager.LoadScene("StageSelect");
        }
        if (Stage1 == true)
        {
            SceneManager.LoadScene("Start");
        }
        if (Stage2 == true)
        {
            SceneManager.LoadScene("Game2");
        }
        if (Stage3 == true)
        {
            SceneManager.LoadScene("Game3");
        }
    }
}
