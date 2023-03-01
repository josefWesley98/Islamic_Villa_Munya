using UnityEngine;

public class Key : MonoBehaviour
{
    public Door doorToUnlock;

    private void OnTriggerEnter(Collider other)
    {
        doorToUnlock.unlockNextPress = true;
        Destroy(gameObject, 0.3f);
    }
}
