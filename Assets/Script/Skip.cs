using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Skip : MonoBehaviour
{
    public bool start;
    public bool Goal;
   
    private void Start()
    {

    }

    public void SkipMovie()             //　ReplayGame()メソッドです
    {
        if (start == true)
        {
            SceneManager.LoadScene("TranpolineOnly");
        }
        if (Goal == true)
        {
            SceneManager.LoadScene("Title");
        }
        
    }
}
