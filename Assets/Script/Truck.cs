using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Truck : MonoBehaviour
{
    GameObject StageObj;
    
    public GameObject truck_body;
    GameObject MainBGM;
    // Start is called before the first frame update
    void Start()
    {
        StageObj=GameObject.Find("StageObj");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Namakemono")
        {
            //サウンド再生
            CriAtomSource audio = (CriAtomSource)GetComponent("CriAtomSource");
            audio.Play();
            //BGMを止める
            MainBGM = GameObject.Find("Main Camera");
            MainBGM.GetComponent<CriAtomSource>().Stop();

            truck_body.transform.Translate(3.0f * Time.deltaTime, 0.0f, 0.0f);
        }
        StartCoroutine("NextStage");//コルーチンを使いたいところにこれを入れる
    }
    public IEnumerator NextStage()
    {

        yield return new WaitForSeconds(3);//何秒待つのか
        SceneManager.sceneLoaded += SceneLoaded;
        SceneManager.LoadScene("Game2");

    }
    void SceneLoaded(Scene nextScene, LoadSceneMode mode)
    {
        StageObj.GetComponent<StageSelect>().Stage2Flag = true;
    }
}

