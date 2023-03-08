using UnityEngine;

public class Key : MonoBehaviour
{
    public Door doorToUnlock;
    float startY;

    private void Start()
    {
        startY = transform.position.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.SetHasKey(true);
        doorToUnlock.unlockNextPress = true;
        //Destroy(lockRB);
        Destroy(gameObject, 0.3f);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, 60 * Time.deltaTime, Space.World);
        transform.position = new Vector3(transform.position.x, startY + Mathf.Sin(Time.time * 1.5f) * 0.15f, transform.position.z);
    }
}
