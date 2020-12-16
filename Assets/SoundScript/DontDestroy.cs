using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
	public GameObject MainBGM;
	private void Awake()
	{
		DontDestroyOnLoad(MainBGM);
	}
	// Use this for initialization

}
