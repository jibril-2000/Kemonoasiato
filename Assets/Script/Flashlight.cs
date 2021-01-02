using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
   
    public GameObject find;
    public Keeper keeper; //Keeperをアタッチ
    public GameObject KeeperAnim;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Namakemono")
        {
            find.SetActive(true); //「！」画像を出す
            KeeperAnim.GetComponent<Animator>().enabled = false;
            keeper = keeper.GetComponent<Keeper>();
            keeper.enabled = false; //Keeperに付いたスクリプトを無効化する

            Invoke("Restart", 2.0f); //2.0秒後に「void Restart(){}」を有効にする
        }
    }
    void Restart()
    {
        keeper.enabled = true;
        find.SetActive(false);
        KeeperAnim.GetComponent<Animator>().enabled = true ;
    }
}