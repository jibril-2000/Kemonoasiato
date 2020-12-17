using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public GameObject Player;
    public GameObject Crow;
    [SerializeField] GameObject Canvas;
    Namakemono script;
    Vector2 CrowOri;
    [SerializeField]
    Fade fade = null;
    // Start is called before the first frame update
    void Start()
    {
        script = Player.gameObject.GetComponent<Namakemono>();
        CrowOri = Crow.transform.position;
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
        Player.gameObject.transform.rotation=Quaternion.Euler(0, 0, 0);
        Crow.gameObject.transform.position = CrowOri;
    }
}
