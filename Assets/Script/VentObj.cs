using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentObj : MonoBehaviour
{
    private Renderer _renderer;
    public GameObject RotationUI;
    // Start is called before the first frame update
    void Start()
    {
        _renderer = RotationUI.GetComponent<Renderer>();
        _renderer.enabled = false; //trueで表示
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseEnter()
    {
        _renderer.enabled = true;

    }
    void OnMouseExit()
    {
        StartCoroutine("Negative");
    }
    public IEnumerator Negative()
    {
        yield return new WaitForSeconds(0.2f);
        _renderer.enabled = false;
    }
}
