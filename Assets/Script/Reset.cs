using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    private string sceneName;       //　文字の変数sceneNameを用意します

    private void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;  //　変数sceneNameに「現在のscene」のファイル名をいれます
    }

    public void ReplayGame()             //　ReplayGame()メソッドです
    {
        SceneManager.LoadScene(sceneName);   //　変数sceneNameに入れられたsceneをロードします
    }
}
