using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadscnee : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(LoadNow), 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadNow()
    {
        SceneManager.LoadScene(1);
    }
}
