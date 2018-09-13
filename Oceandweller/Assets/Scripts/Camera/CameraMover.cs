using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public float movementSpeed = 8f;

    [Header("Locking to Objects")]
    public Vector2 offsetWhenLocked;
    public bool lockToTransformOnStart = true;
    public Transform lockedTransformOnStart;

    private bool isLockedToObject;
    private Transform lockedObject;

    //Lerping
    private Vector2 startPoint;
    private Vector2 endPoint;
    private Transform transformEndpoint;
    private float journeyLength;
    private float startTime;

    private bool isLerping = false;
    private bool lockToTransform = false;

    //Events
    public delegate void CameraMovementAction();
    public event CameraMovementAction OnCameraFinishedMoving;

    public void LockToObject(Transform objectTransform)
    {
        isLockedToObject = true;
        transform.parent = objectTransform;
        transform.localPosition = new Vector3(offsetWhenLocked.x, offsetWhenLocked.y, transform.localPosition.z);
    }

    public void UnlockFromObject()
    {
        isLockedToObject = false;
        transform.parent = null;
    }

    public void MoveCameraTo(Transform location, float speed, bool lockToTransformOnLerpEnd = false)
    {
        MoveCameraTo(location.position, speed);
        transformEndpoint = location;
        lockToTransform = lockToTransformOnLerpEnd;
    }

    public void MoveCameraTo(Vector2 location, float speed)
    {
        isLerping = true;
        movementSpeed = speed;
        transformEndpoint = null;
        startTime = Time.time;
        startPoint = transform.position;
        endPoint = location;

        journeyLength = Vector2.Distance(startPoint, endPoint);

        //print("Start" + startTime.ToString("F4"));
        //print("Start " + startPoint.ToString("F4"));
        //print("End " + endPoint.ToString("F4"));
        //print("Journey Length = " + journeyLength.ToString("F4"));
    }

    public void StopCameraMovement()
    {
        isLerping = false;
    }

    private void Start()
    {
        if(lockToTransformOnStart && lockedTransformOnStart != null)
        {
            LockToObject(lockedTransformOnStart);
        }
    }

    private void Update()
    {
        if(isLerping)
        {
            if(transformEndpoint != null)
            {
                journeyLength = Vector2.Distance(startPoint, transformEndpoint.transform.position);
                endPoint = transformEndpoint.position;

                if (journeyLength == 0.0f)
                {
                    return;
                }
            }
            float distCovered = (Time.time - startTime) * movementSpeed;
            float fracJourney = distCovered / journeyLength;
            Vector3 newPosition = Vector3.Lerp(startPoint, endPoint, fracJourney);
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);

            if(fracJourney > 1)
            {
                isLerping = false;
                if(lockToTransform && transformEndpoint != null)
                {
                    LockToObject(transformEndpoint);
                }
                if(OnCameraFinishedMoving != null)
                {
                    OnCameraFinishedMoving();
                }
            }
        }
    }
}
