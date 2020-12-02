using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Quit : MonoBehaviour
{


    private void Start()
    {

    }

    public void quit()             //　ReplayGame()メソッドです
    {

      UnityEngine.Application.Quit();

    }
}
