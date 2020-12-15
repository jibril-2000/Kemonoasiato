using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Namakemono")
        {
            StartCoroutine("NextStage");//コルーチンを使いたいところにこれを入れる
        }

    }
    public IEnumerator NextStage()
    {

        yield return new WaitForSeconds(3);//何秒待つのか

        SceneManager.LoadScene("Game3");

    }
}