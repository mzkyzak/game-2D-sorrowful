using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KameraFollow : MonoBehaviour
{
    public Transform target; 
    public Vector2 minLimit; 
    public Vector2 maxLimit; 
    public float smoothSpeed; 

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position;

        float clampedX = Mathf.Clamp(targetPosition.x, minLimit.x, maxLimit.x);
        float clampedY = Mathf.Clamp(targetPosition.y, minLimit.y, maxLimit.y);

        Vector3 smoothPosition = Vector3.Lerp(transform.position,
        new Vector3(clampedX, clampedY, transform.position.z), smoothSpeed);

        transform.position = smoothPosition;
    }
}
