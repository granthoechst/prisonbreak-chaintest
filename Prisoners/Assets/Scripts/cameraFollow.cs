using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour {

    public Transform target;

    private float smoothSpeed = .05f;
    public Vector3 offset;
    public bool zooming = true;

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        float distance = Mathf.Sqrt(Mathf.Pow(target.position.x - transform.position.x, 2) + 
                                    Mathf.Pow(target.position.y - transform.position.y, 2));
        if (distance > 15)
        {
            zooming = true;
        }
        if (distance < .1)
        {
            zooming = false;
        }
        if (zooming)
        {
            smoothedPosition.z = -19;
            transform.position = smoothedPosition;
        }
    }
}
