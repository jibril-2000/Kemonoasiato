using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Movie : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] GameObject menu;
    public bool start;
    public bool Goal;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.isLooping = true;
        videoPlayer.loopPointReached += FinishPlayingVideo;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FinishPlayingVideo(VideoPlayer vp)
    {
        videoPlayer.Stop();
        if (start == true)
        {
            SceneManager.LoadScene("TranpolineOnly");
        }
        if (Goal == true)
        {
            SceneManager.LoadScene("Title");
        }
    }
}

