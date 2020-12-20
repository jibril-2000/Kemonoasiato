using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimic_OFF_Button : MonoBehaviour
{
    [SerializeField] private GameObject UI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        UI.SetActive(false);
    }
}
