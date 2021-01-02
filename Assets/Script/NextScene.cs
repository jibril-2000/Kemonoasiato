using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    GameObject StageObj;
    public bool Game2;
    public bool TranpolineOnly;
    public bool TranandBelt;
    void Start()
    {
        StageObj = GameObject.Find("StageObj");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Namakemono")
        {
            if (Game2 == true)
            {
                StartCoroutine("Game3Load");//コルーチンを使いたいところにこれを入れる
            }
            if (TranpolineOnly == true)
            {
                StartCoroutine("TranandBeltLoad");
                
            }
            if (TranandBelt == true)
            {
                StartCoroutine("GameLoad");
                
            }
        }
    }
    public IEnumerator Game3Load()
    {

        yield return new WaitForSeconds(3);//何秒待つのか
        SceneManager.sceneLoaded += SceneLoaded;
        SceneManager.LoadScene("Game3");

    }
    public IEnumerator TranandBeltLoad()
    {

        yield return new WaitForSeconds(3);//何秒待つのか
        SceneManager.LoadScene("TranandBelt");

    }
    public IEnumerator GameLoad()
    {

        yield return new WaitForSeconds(3);//何秒待つのか
        SceneManager.LoadScene("Game");

    }
    void SceneLoaded(Scene nextScene, LoadSceneMode mode)
    {
        StageObj.GetComponent<StageSelect>().Stage3Flag = true;
    }
}