using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class SwitchSceneIntro : MonoBehaviour
{
    [SerializeField] VideoPlayer LambsIntro;
    // Start is called before the first frame update
    void Start()
    {
        LambsIntro.loopPointReached += SwitchSceneVideo;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SwitchSceneVideo(VideoPlayer vp)
    {
        SceneManager.LoadScene("Menu");
    }
}
