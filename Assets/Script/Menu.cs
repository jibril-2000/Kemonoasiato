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
    GameObject MainBGM;
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

    [System.Obsolete]
    public void Pause()
    {
        Time.timeScale = 0; //時間停止
        pausePanel.SetActive(true);
        TitlePanel.SetActive(true);

        //サウンドAISAC設定
        CriAtomExCategory.SetAisac(1, 0, 1.0f); //カテゴリID 1のAISACControlID0にAISAC値をセット
    }

    [System.Obsolete]
    public void Resume()
    {
        Time.timeScale = 1; //時間再開
        pausePanel.SetActive(false);
        TitlePanel.SetActive(true);

        //サウンドAISAC設定
        CriAtomExCategory.SetAisac(1, 0, 0.0f); //カテゴリID 1のAISACControlID0にAISAC値をセット
    }
}