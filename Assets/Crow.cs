using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crow: MonoBehaviour
{
	bool Hit;
	Renderer targetRenderer; // 判定したいオブジェクトのrendererへの参照
	void Start()
	{
		targetRenderer = this.GetComponent<Renderer>();
		//targetRenderer.enabled = false;
		Hit = false;
	}

	void Update()
    {
		if(Hit==false)
		transform.position += new Vector3(-3.0f * Time.deltaTime, 0.0f, 0.0f);
	}
	/***void OnBecameVisible()
	{
		targetRenderer.enabled = true;
	}
	void OnBecameInvisible()
	{
		Destroy(this.gameObject);
	}

	/***void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player")
        {
			Hit = true;
			StartCoroutine("HIT",other.gameObject);
			
		}
	}
	private IEnumerator HIT(GameObject Hitobj)
    {
		yield return new WaitForSeconds(3);
		Destroy(Hitobj);
		
	}***/

}

