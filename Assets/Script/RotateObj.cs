using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObj : MonoBehaviour
{
    Vector3 firstPos;
    Vector3 lastPos;

    private Renderer _renderer;
    private Vector3 clickPos;             	// 　マウスのカーソル位置座標
    private float degree;       //　回転角度（オイラー角、一般的な普通の角°で表す）
    private float rotSpeed=0.8f;      //　回頭のスピードを入れる変数　0.8fくらいがよい

    void Start()
    {
        _renderer = GetComponent<Renderer>(); //
        _renderer.enabled = false; //trueで表示
    }

    // Update is called once per frame
    void OnMouseEnter()
    {
        _renderer.enabled = true;
    }

    void OnMouseDrag()
    {

        _renderer.enabled = true;
        clickPos = Input.mousePosition;      // Vector3型変数ｃlickPosに、マウスの現在位置座標を取得する(クリックしませんが)
        clickPos.z = 10.0f;                                   //Z軸の値に適当な値を入れます
                                                              // ScreenToWorldPoint(位置(Vector3))：　スクリーン座標をワールド座標に変換する
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(clickPos);
        //弾の飛ぶ方向をマウスカーソルの位置からcanonPosの位置を引いて、正規化（長さ1の単位ベクトル）します
        Vector3 bulletDir = Vector3.Scale((mouseWorldPos -this.transform.position), new Vector3(1, 1, 0)).normalized;

        degree = Mathf.Atan2(bulletDir.y, bulletDir.x) * Mathf.Rad2Deg;
        //アークタンジェントで求めた角度（ラジアン表示）をオイラー角に変換します
        //playerObj.transform.eulerAngles = new Vector3(0, 0, degree);　
        gameObject.transform.parent.transform.eulerAngles = new Vector3(0f, 0f, Mathf.LerpAngle(gameObject.transform.parent.transform.eulerAngles.z,degree, Time.deltaTime * rotSpeed));

        

    }
    void OnMouseExit()
    {
        _renderer.enabled = false;
    }

}

