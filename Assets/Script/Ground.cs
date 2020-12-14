using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public GameObject Player;
    [SerializeField] GameObject Canvas;
    Namakemono script;
    [SerializeField]
    Fade fade = null;
    // Start is called before the first frame update
    void Start()
    {
        script = Player.gameObject.GetComponent<Namakemono>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.CompareTag("Namakemono"))
        {
            StartCoroutine("Resporn");//コルーチンを使いたいところにこれを入れる
            
        }

    }
    public IEnumerator Resporn()
    {
        Canvas.GetComponent<FadeTest>().FadeStart();
        yield return new WaitForSeconds(1);//何秒待つのか
        
            Player.gameObject.transform.position = script.Resporn;

    }
}
