using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [TextArea]
    [SerializeField] private string artifactInfo = "";
    [TextArea]
    [SerializeField] private string artifactInfoEasy = "Easy";
    [TextArea]
    [SerializeField] private string artifactInfoMedium = "Medium";
    [TextArea]
    [SerializeField] private string artifactInfoHard = "Hard";

    [Header("set the scale the inspectable item will have.")]
    [SerializeField] private Vector3 scale = new Vector3(0.35f,0.35f,0.35f);
    [Header("Set the rotation the inspectable item will have.")]
    [SerializeField] private Vector3 rotation = new Vector3(0,0,0);
    [Header("Set the offset on X,Y,Z the object will have.")]
    [SerializeField] private Vector3 positinOffset = new Vector3(0,0,0);
    [Header("Set the look at object the camera will focus on (generally this object.)")]
    [SerializeField] private GameObject LookAt;
    [SerializeField] private int pedestalID;
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
    public Vector3 GetPositionOffset()
    {
        return positinOffset;
    }
    public int GetPedestalID()
    {
        return pedestalID;
    }
}
