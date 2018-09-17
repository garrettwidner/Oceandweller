using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundableCamera : MonoBehaviour
{
    public Camera cam;

    public Collider2D startRoom;
    public bool startRoomIsLarge;
    public Transform trackedObject;

    private Collider2D cameraBounds;
    private Transform trackedTransform;

    private Vector2 desiredCameraPosition;

    private bool isInLargeRoom = false;
    private bool shouldLerp = false;
    private Vector2 smoothVelocity;
    private float lerpLeeway = 0.001f;
    private float lerpTime = .3f;

    private void Start()
    {
        if(startRoomIsLarge)
        {
            LerpToLargeRoom(startRoom, trackedObject);
        }
        else
        {
            SnapToSmallRoom(startRoom);
        }
    }

    private void SetSmallRoomBools(Collider2D boundsCollider)
    {
        isInLargeRoom = false;
        cameraBounds = boundsCollider;
        trackedTransform = null;
    }

    private void SnapToSmallRoom(Collider2D boundsCollider)
    {
        SetSmallRoomBools(boundsCollider);

        transform.position = boundsCollider.bounds.center;
    }

    private void LerpToSmallRoom(Collider2D boundsCollider)
    {
        SetSmallRoomBools(boundsCollider);

        StartLerp();
    }

    private void LerpToLargeRoom(Collider2D boundsCollider, Transform trackedObject)
    {
        isInLargeRoom = true;
        cameraBounds = boundsCollider;
        trackedTransform = trackedObject;

        StartLerp();
    }

    private void StartLerp()
    {
        shouldLerp = true;
        smoothVelocity = Vector3.zero;
    }



    private void Update()
    {
        SetDesiredCameraPosition(); 
        
        if(isInLargeRoom)
        {
            ConstrainCameraToBounds();
        }

        if (shouldLerp)
        {
            LerpToNewTrackingLocation();
        }
        else if(isInLargeRoom)
        {
            KeepCameraOnTarget();
        }
    }

    private void SetDesiredCameraPosition()
    {
        if(cameraBounds != null)
        {
            if (!isInLargeRoom)
            {
                desiredCameraPosition = cameraBounds.bounds.center;
                return;
            }
            else
            {
                desiredCameraPosition = trackedTransform.position;
            }
        }
    }

    private void ConstrainCameraToBounds()
    {
        Bounds camBounds = cameraBounds.bounds;
        Vector2 roomUpperRight = camBounds.center + camBounds.extents;
        Vector2 roomLowerLeft = camBounds.center - camBounds.extents;

        Vector2 corner = Vector2.zero;
        Vector2 desiredCameraOffset = Vector2.zero;
        bool useOffsetX = false;
        bool useOffsetY = false;

        //Find real world screen size
        Vector2 halfScreenSize = new Vector2();
        Vector2 screenPoint1 = cam.ViewportToWorldPoint(Vector2.zero);
        Vector2 screenPoint2 = cam.ViewportToWorldPoint(Vector2.one);
        halfScreenSize.x = Mathf.Abs(screenPoint1.x - screenPoint2.x);
        halfScreenSize.y = Mathf.Abs(screenPoint1.y - screenPoint2.y);
        halfScreenSize /= 2;

        Vector2 prospectiveCameraLowerLeft = desiredCameraPosition - halfScreenSize;
        Vector2 prospectiveCameraUperRight = desiredCameraPosition + halfScreenSize;

        if (prospectiveCameraLowerLeft.y < roomLowerLeft.y)
        {
            useOffsetY = true;
            corner.y = roomLowerLeft.y;
            desiredCameraOffset.y = halfScreenSize.y;
        }
        else if (prospectiveCameraUperRight.y > roomUpperRight.y)
        {
            useOffsetY = true;
            corner.y = roomUpperRight.y;
            desiredCameraOffset.y = -halfScreenSize.y;
        }

        if (prospectiveCameraLowerLeft.x < roomLowerLeft.x)
        {
            useOffsetX = true;
            corner.x = roomLowerLeft.x;
            desiredCameraOffset.x = halfScreenSize.x;
        }
        else if (prospectiveCameraUperRight.x > roomUpperRight.x)
        {
            useOffsetX = true;
            corner.x = roomUpperRight.x;
            desiredCameraOffset.x = -halfScreenSize.x;
        }

        if (useOffsetX)
        {
            desiredCameraPosition.x = corner.x + desiredCameraOffset.x;
        }
        if(useOffsetY)
        {
            desiredCameraPosition.y = corner.y + desiredCameraOffset.y;
        }
    }

    private void LerpToNewTrackingLocation()
    {
        transform.position = Vector2.SmoothDamp((Vector2)transform.position, desiredCameraPosition, ref smoothVelocity, lerpTime);
        if (Vector2.Distance(transform.position, desiredCameraPosition) < lerpLeeway)
        {
            shouldLerp = false;
        }
    }

    private void KeepCameraOnTarget()
    {
        transform.position = desiredCameraPosition;
    }






}
