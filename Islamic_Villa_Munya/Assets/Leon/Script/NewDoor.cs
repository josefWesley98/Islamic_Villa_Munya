using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[ExecuteInEditMode]
public class NewDoor : MonoBehaviour
{
    public bool selectHinge = false;
    public bool selectOpenPreview = false;

    GameObject hingeObject, previewObject;
    Mesh mesh;

    void Start()
    {
        if (Application.isPlaying)
            return;

        mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;

        if(hingeObject == null || previewObject == null)
            GenerateDoorSet();
    }

    void GenerateDoorSet()
    {
        if(hingeObject != null)
            Destroy(hingeObject);
        if (previewObject != null)
            Destroy(previewObject);

        hingeObject = new GameObject("Door Hinge");
        hingeObject.transform.position = transform.position + Vector3.right;

        previewObject = new GameObject("Door Open Preview");
        previewObject.transform.position = transform.position;
        previewObject.transform.rotation = transform.rotation;
        previewObject.transform.localScale = transform.localScale;
        previewObject.transform.parent = hingeObject.transform;
        //doorHolder.hideFlags = HideFlags.HideInHierarchy;
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying)
            return;

        if (hingeObject == null || previewObject == null)
            GenerateDoorSet();
    }

    private void OnValidate()
    {
        if (selectHinge)
        {
            Selection.activeGameObject = hingeObject;
            selectHinge = false;
        }
        if (selectOpenPreview)
        {
            Selection.activeGameObject = previewObject;
            selectOpenPreview = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (hingeObject == null || previewObject == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hingeObject.transform.position, 0.2f);
        Handles.Label(hingeObject.transform.position + Vector3.up * 0.3f, "Door Hinge");

        Color c = new Color(0, 100, 200, 0.5f);
        Gizmos.color = c;
        Gizmos.DrawMesh(mesh, previewObject.transform.position, previewObject.transform.rotation, previewObject.transform.localScale);
        //Gizmos.color = Color.green;
        //Gizmos.DrawLine(hingePos, openPos);
        //Gizmos.DrawSphere(openPos, 0.05f);
        //Handles.Label(openPos + Vector3.up * 0.2f, "Open");
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawLine(transform.position, hingePos);
        //Gizmos.DrawSphere(transform.position, 0.05f);
        //Handles.Label(transform.position + Vector3.up * 0.2f, "Closed");
    }
}
