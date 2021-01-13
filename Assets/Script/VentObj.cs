using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentObj : MonoBehaviour
{
    private Renderer _renderer;
    public GameObject RotationUI;
    public GameObject MoveArea;
    public GameObject Hide01, Hide02, Hide03, Hide04, NoHide;
    bool Move;
    public bool Moveobj;
    public Vector3 minVec;
    public Vector3 maxVec;
    private Vector3 Pos;
    SpriteRenderer H01, H02, H03, H04, H05;
    // Start is called before the first frame update
    void Start()
    {
        Pos = this.gameObject.transform.position;
        _renderer = RotationUI.GetComponent<Renderer>();
        _renderer.enabled = false; //trueで表示
        maxVec.y = Pos.y + maxVec.y;
        minVec.y = Pos.y - minVec.y;
        maxVec.x = Pos.x + maxVec.x;
        minVec.x = Pos.x - minVec.x;

        //αを０にしておく
        H05 = NoHide.GetComponent<SpriteRenderer>();
        H05.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Pos = this.gameObject.transform.position;
        if (Pos.y > maxVec.y)
        {
            Pos.x = maxVec.y - 1f;
            this.gameObject.transform.position = new Vector3(Pos.x, Pos.y, Pos.z);
        }
        if (Pos.y < minVec.y)
        {
            Pos.x = minVec.y + 1f;
            this.gameObject.transform.position = new Vector3(Pos.x, Pos.y, Pos.z);
        }
        if (Pos.x > maxVec.x)
        {
            Pos.x = maxVec.x - 1f;
            this.gameObject.transform.position = new Vector3(Pos.x, Pos.y, Pos.z);
        }
        if (Pos.x < minVec.x)
        {
            Pos.x = minVec.x + 1f;
            this.gameObject.transform.position = new Vector3(Pos.x, Pos.y, Pos.z);
        }
        if (Move == true)
        {
            if (Input.GetMouseButton(0))
            {
                MoveArea.SetActive(true);

                //αの値を変えて見えるオブジェクトを変更する
                H01 = Hide01.GetComponent<SpriteRenderer>();
                H01.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                H02 = Hide02.GetComponent<SpriteRenderer>();
                H02.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                H03 = Hide03.GetComponent<SpriteRenderer>();
                H03.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                H04 = Hide04.GetComponent<SpriteRenderer>();
                H04.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                H05 = NoHide.GetComponent<SpriteRenderer>();
                H05.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }
    }
    void OnMouseEnter()
    {
        _renderer.enabled = true;
        StopCoroutine("Negative");
    }
    void OnMouseExit()
    {
        StartCoroutine("Negative");
    }
    void OnMouseDrag()
    {
        if (Moveobj == true)
        {
            StopCoroutine("Negative");
            Move = true;
            _renderer.enabled = true;
            Vector3 objectPointInScreen
                = Camera.main.WorldToScreenPoint(this.transform.position);

            Vector3 mousePointInScreen
                = new Vector3(Input.mousePosition.x,
                              Input.mousePosition.y,
                              objectPointInScreen.z);

            Vector3 mousePointInWorld = Camera.main.ScreenToWorldPoint(mousePointInScreen);
            mousePointInWorld.z = this.transform.position.z;
            this.transform.position = mousePointInWorld;

        }
    }
    public IEnumerator Negative()
    {
        yield return new WaitForSeconds(0.2f);
        _renderer.enabled = false;
        MoveArea.SetActive(false);
        Move = false;

        //αの値を変えて見えるオブジェクトを変更する
        H01 = Hide01.GetComponent<SpriteRenderer>();
        H01.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        H02 = Hide02.GetComponent<SpriteRenderer>();
        H02.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        H03 = Hide03.GetComponent<SpriteRenderer>();
        H03.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        H04 = Hide04.GetComponent<SpriteRenderer>();
        H04.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        H05 = NoHide.GetComponent<SpriteRenderer>();
        H05.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }
}
