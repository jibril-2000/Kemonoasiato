using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Namakemonopachinko : MonoBehaviour
{
    public GameObject pachinkostart;
    private Vector3 clickPos;               // 　マウスのカーソル位置座標
    public Transform canonPos;  //　 弾が出る場所の座標
    Rigidbody2D rigid2d;
    Vector2 startPos;
    private float degree;       //　回転角度（オイラー角、一般的な普通の角°で表す）
    public float speed;                     //    飛ばす力の大きさの値です 
    new Rigidbody2D rigidbody;
    Vector3 canonAngle;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        clickPos = Input.mousePosition;          // Vector3でマウスの位置座標を取得する
        clickPos.z = 10.0f;                                   // そこでに適当な値を入れておきます
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(clickPos);   //　カーソル位置をゲームスクリーンの座標にします
        Vector3 bulletDir = Vector3.Scale(mouseWorldPos - canonPos.position, new Vector3(1, 1, 0)).normalized;
        //　 変数bulletDir　 muzzleとカーソルの位置を、ｘ、ｙ方向ベクトル化して入れます
        degree = Mathf.Atan2(bulletDir.y, bulletDir.x) * Mathf.Rad2Deg;
        Debug.Log(degree);
         canonAngle = new Vector3(degree, degree, 0).normalized;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Pachinkobody")
        {
            transform.position = pachinkostart.transform.position;
            other.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Pachinko")
        {
            transform.rotation = Quaternion.identity;//向きを元に戻す
        }
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Pachinko")
        {

            if (Input.GetMouseButton(1))
            {
                rigidbody.AddForce(canonAngle * speed);
            }
        }
    }
}
