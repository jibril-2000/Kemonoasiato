using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Truck : MonoBehaviour
{
    GameObject StageObj;
    public GameObject WindowLight;
    public GameObject truck_body;
    public GameObject GoalLight;
    GameObject MainBGM;
    public GameObject TireNoMove, TireMove01, TireMove02, TireMove03, TruckBody, TruckBodyShake;
    // Start is called before the first frame update
    void Start()
    {
        StageObj=GameObject.Find("StageObj");
        WindowLight.SetActive(false);
        GoalLight.SetActive(true);

        TireMove01.SetActive(false);
        TireMove02.SetActive(false);
        TireMove03.SetActive(false);
        TireNoMove.SetActive(true);
        TruckBody.SetActive(true);
        TruckBodyShake.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Namakemono")
        {
            WindowLight.SetActive(true);
            GoalLight.SetActive(false);
            //サウンド再生
            CriAtomSource audio = (CriAtomSource)GetComponent("CriAtomSource");
            audio.Play();
            //BGMを止める
            MainBGM = GameObject.Find("Main Camera");
            MainBGM.GetComponent<CriAtomSource>().Stop();

            TireMove01.SetActive(true);
            TireMove02.SetActive(true);
            TireMove03.SetActive(true);
            TireNoMove.SetActive(false);
            TruckBody.SetActive(false);
            TruckBodyShake.SetActive(true);

            Invoke("Truck_Transfome", 0.4f);
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

    void Truck_Transfome()
    {
        truck_body.transform.Translate(3.0f * Time.deltaTime, 0.0f, 0.0f);
    }
}

