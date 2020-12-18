using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    
    public  bool Stage2Flag;
    public  bool Stage3Flag;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("Title");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
   
}
