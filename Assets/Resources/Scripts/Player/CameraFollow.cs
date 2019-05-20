using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed;
    public Vector3 actualOffset;
   
    Vector3 desiredPosition = Vector3.zero;
    Vector3 smoothedPosition = Vector3.zero;

    private void FixedUpdate()
    {
        if (target)
        {
            desiredPosition = target.position + actualOffset;
            smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }   
    }
}
