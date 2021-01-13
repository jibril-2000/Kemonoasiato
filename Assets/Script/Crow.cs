using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crow: MonoBehaviour
{
	bool Hit;
	Renderer targetRenderer; // 判定したいオブジェクトのrendererへの参照
	bool Keepermove = true;
	public float Maxx;
	public float Minx;

	void Start()
	{
		targetRenderer = this.GetComponent<Renderer>();
		//targetRenderer.enabled = false;
		Hit = false;
		Maxx = this.transform.position.x + Maxx;
		Minx = this.transform.position.x - Minx;
	}

	void Update()
	{
		switch (Keepermove)
		{
			case true:
				transform.position -= new Vector3(0.05f, 0.0f, 0.0f);
				if (transform.position.x <= Minx)
				{
					Keepermove = false;				
					transform.Rotate(new Vector2(0, 180));
				}

				break;

			case false:
				transform.position += new Vector3(0.05f, 0.0f, 0.0f);
				if (transform.position.x >= Maxx)
				{
					Keepermove = true;

					transform.Rotate(new Vector2(0, -180));
				}

				break;
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
}

