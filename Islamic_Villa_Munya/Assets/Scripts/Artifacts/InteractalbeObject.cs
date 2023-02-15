using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractalbeObject : MonoBehaviour
{
    [TextArea]
    [SerializeField] private string artifactInfo = "";
    [TextArea]
    [SerializeField] private string artifactInfoEasy = "Easy";
    [TextArea]
    [SerializeField] private string artifactInfoMedium = "Medium";
    [TextArea]
    [SerializeField] private string artifactInfoHard = "Hard";
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public string GetArtifactInfo()
    {
        return artifactInfo;
    }
    public string GetArtifactInfoEasy()
    {
        return artifactInfoEasy;
    }

    public string GetArtifactInfoMedium()
    {
        return artifactInfoMedium;
    }

    public string GetArtifactInfoHard()
    {
        return artifactInfoHard;
    }
}
