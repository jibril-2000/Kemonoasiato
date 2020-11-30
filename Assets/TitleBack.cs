using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleBack : MonoBehaviour
{
    

    private void Start()
    {
        
    }

    public void TitleLord()             //　ReplayGame()メソッドです
    {
        SceneManager.LoadScene("Title"); 
    }
}
