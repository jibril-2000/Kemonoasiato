using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject TitlePanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button TitleButton;
    // Start is called before the first frame update
    void Start()
    {
        pausePanel.SetActive(false);
        TitlePanel.SetActive(false);
        pauseButton.onClick.AddListener(Pause);
        resumeButton.onClick.AddListener(Resume);
        TitleButton.onClick.AddListener(Resume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Pause()
    {
        Time.timeScale = 0; //時間停止
        pausePanel.SetActive(true);
        TitlePanel.SetActive(true);
    }

    private void Resume()
    {
        Time.timeScale = 1; //時間再開
        pausePanel.SetActive(false);
        TitlePanel.SetActive(true);
    }
}
