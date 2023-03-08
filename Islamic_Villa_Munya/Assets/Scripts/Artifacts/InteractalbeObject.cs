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
    [SerializeField] private Vector3 scale = new Vector3(0.35f,0.35f,0.35f);
    [SerializeField] private Vector3 rotation = new Vector3(0,0,0);
    [SerializeField] private float yOffset = 0;
    [SerializeField] private GameObject LookAt;
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
    public Vector3 GetScale()
    {
        return scale;
    }
    public Vector3 GetRotation()
    {
        return rotation;
    }
    public Transform GetLookAt()
    {
        return LookAt.transform;
    }
    public float GetYOffset()
    {
        return yOffset;
    }
}
