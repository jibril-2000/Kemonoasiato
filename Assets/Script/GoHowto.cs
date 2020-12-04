using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoHowto : MonoBehaviour
{


    private void Start()
    {

    }

    public void HowtoPlay()             //　ReplayGame()メソッドです
    {
        SceneManager.LoadScene("How to Play");
    }
}