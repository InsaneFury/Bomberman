using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public delegate void OnObjectAction(DestructibleObject d);
    public static OnObjectAction OnObjectDestroy;

    public int score = 100;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Explosion"))
        {
            DestroyObj();
            Destroy(gameObject);
        }
    }

    void DestroyObj()
    {
        if (OnObjectDestroy != null)
        {
            OnObjectDestroy(this);
        }
    }
}
