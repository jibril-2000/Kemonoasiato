using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    
    public GameObject Groud;
    private void Start()
    {
        
    }

    public void ReplayGame()             //　ReplayGame()メソッドです
    {
        Groud.GetComponent<Ground>().StartCoroutine("Resporn");
    }
}
