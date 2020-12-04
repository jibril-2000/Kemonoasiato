using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beltconveyor : MonoBehaviour
{
    RectTransform canvasRect;
    public GameObject UI;

    private void Awake()
    {
        canvasRect = GameObject.Find("Button").GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDrag()
    {
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

    //マウスが乗っている間、呼び出され続ける
    void OnMouseOver()
    {
        UI.SetActive(true);
    }

    //マウスが離れたとき
    void OnMouseExit()
    {
        UI.SetActive(false);
    }

}
