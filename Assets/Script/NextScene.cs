using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    GameObject StageObj;
    void Start()
    {
        StageObj = GameObject.Find("StageObj");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Namakemono")
        {
            StartCoroutine("NextStage");//コルーチンを使いたいところにこれを入れる
        }

    }
    public IEnumerator NextStage()
    {

        yield return new WaitForSeconds(3);//何秒待つのか
        SceneManager.sceneLoaded += SceneLoaded;
        SceneManager.LoadScene("Game3");

    }
    void SceneLoaded(Scene nextScene, LoadSceneMode mode)
    {
        StageObj.GetComponent<StageSelect>().Stage3Flag = true;
    }
}