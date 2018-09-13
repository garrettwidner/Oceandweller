using UnityEngine;

public class GroundMover : RaycastBoxProjector 
{
    [SerializeField]protected float baseSpeed = .028f;
    [SerializeField]protected float runSpeed = .05f;

    public virtual float CurrentSpeed
    {
        get
        {
            return baseSpeed;
        }
    }

    public bool lockTo8Directions = true;

    public Vector2 FaceDirection
    {
        get
        {
            return collisionInfo.faceDirection;
        }
    }

    protected bool canMove = true;
    public bool CanMove
    {
        get
        {
            return canMove;
        }
    }
    public void AllowMovement()
    {
        canMove = true;
    }
    public void DisallowMovement()
    {
        canMove = false;
    }

    protected bool canTurn = true;
    public bool CanTurn
    {
        get
        {
            return canTurn;
        }
    }
    public void AllowTurning()
    {
        canTurn = true;
    }
    public void DisallowTurning()
    {
        canTurn = false;
    }

    protected bool isMoving = false;
    public bool IsMoving
    {
        get
        {
            return isMoving;
        }
    }

    protected bool wasMoving = false;
    public bool WasMoving
    {
        get
        {
            return wasMoving;
        }
    }

    public float HighestSpeed
    {
        get
        {
            return runSpeed;
        }
    }

    public Vector2 VelocityLastFrame
    {
        get
        {
            return velocityLastFrame;
        }
    }
    protected Vector2 velocityLastFrame = Vector2.zero;

    protected CollisionInfo collisionInfo;

    public bool debugDirectionHits = false;

    protected PlayerActions playerActions;
    protected bool isSetup = false;

    public delegate void GroundMovementAction();
    public GroundMovementAction OnMovementStarted;
    public GroundMovementAction OnMovementStopped;


    public void RunSetup(PlayerActions pActions)
    {
        playerActions = pActions;
        isSetup = true;
    }

    public override void Awake()
    {
        base.Awake();
        collisionInfo.Setup();
        collisionInfo.faceDirection = Vector2.zero;
    }

    protected virtual void Update()
    {

        SetMovingStatus();

        velocityLastFrame = Vector2.zero;

        if(isSetup)
        {
            Vector2 input = new Vector2(playerActions.Move.X, playerActions.Move.Y);
            if (lockTo8Directions)
            {
                input = input.ClosestCardinalOrOrdinalDirection().normalized * input.magnitude;
            }

            if (CanTurn)
            {
                collisionInfo.SetFaceDirection(input);
            }
        }
    }

    protected void SetMovingStatus()
    {
        wasMoving = isMoving;

        if (velocityLastFrame == Vector2.zero)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }

        if(!isMoving && wasMoving)
        {
            if(OnMovementStopped != null)
            {
                OnMovementStopped();
            }
        }
        else if(isMoving && !wasMoving)
        {
            if(OnMovementStarted != null)
            {
                OnMovementStarted();
            }
        }
    }

    public void Move(ref Vector2 velocity)
    {
        if(isSetup)
        {
            Vector2 input = new Vector2(playerActions.Move.X, playerActions.Move.Y);

            Move(ref velocity, input);
        }
        
    }

    public void Move(ref Vector2 velocity, Vector2 input)
    {
        if (isSetup)
        {
            collisionInfo.Reset();
            UpdateRaycastOrigins();
            CalculateRaySpacing();

            if (lockTo8Directions)
            {
                input = input.ClosestCardinalOrOrdinalDirection().normalized * input.magnitude;
            }

            if (CanMove)
            {
                velocity += new Vector2(input.x * CurrentSpeed, input.y * CurrentSpeed);
            }

            Vector2 baseVelocity = velocity;

            if (CanMove)
            {
                if (velocity.x != 0)
                {
                    HorizontalCollisions(ref velocity);
                }
                if (velocity.y != 0)
                {
                    VerticalCollisions(ref velocity);
                }

                if (collisionInfo.horizontal && collisionInfo.slopeAngle != 90 && collisionInfo.slopeAngle != 0 && collisionInfo.slopeAngle != 180 && input.y == 0)
                {
                    MoveAcrossDiagonalWallHorizontally(ref velocity, baseVelocity, Mathf.Abs(baseVelocity.x));
                }
                else if (collisionInfo.vertical && collisionInfo.slopeAngle != 90 && collisionInfo.slopeAngle != 0 && collisionInfo.slopeAngle != 180 && input.x == 0)
                {
                    MoveAcrossDiagonalWallVertically(ref velocity, baseVelocity);
                }

                velocityLastFrame = velocity;
                

                //transform.Translate(new Vector3(velocity.x, velocity.y, 0f));
            }
        }
    }


    protected void HorizontalCollisions(ref Vector2 velocity)
    {
        float directionX = collisionInfo.faceDirection.x;
        float rayLength = Mathf.Abs(velocity.x) + SkinWidth;

        if (Mathf.Abs(velocity.x) < SkinWidth)
        {
            rayLength = 2 * SkinWidth;
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit)
            {
                velocity.x = (hit.distance - SkinWidth) * directionX;
                rayLength = hit.distance;

                collisionInfo.right = directionX == 1 ? true : false;
                collisionInfo.left = directionX == -1 ? true : false;

                if (debugDirectionHits)
                {
                    if (collisionInfo.right) print("Right");
                    if (collisionInfo.left) print("Left");
                }

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                collisionInfo.slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                collisionInfo.slopeNormal = hit.normal;
            }
        }
    }

    protected void VerticalCollisions(ref Vector2 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + SkinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            if (hit)
            {
                velocity.y = (hit.distance - SkinWidth) * directionY;
                rayLength = hit.distance;

                collisionInfo.below = directionY == -1 ? true : false;
                collisionInfo.above = directionY == 1 ? true : false;

                if (debugDirectionHits)
                {
                    if (collisionInfo.below) print("Below");
                    if (collisionInfo.above) print("Above");
                }

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                /*
                if (slopeAngle != collisionInfo.slopeAngle)
                {
                    Debug.LogWarning("Warning: slope angles did not match. Check for errors and edge cases.");
                }
                 * */

                collisionInfo.slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                collisionInfo.slopeNormal = hit.normal;
            }
        }
    }

    protected void MoveAcrossDiagonalWallHorizontally(ref Vector2 velocity, Vector2 baseVelocity, float moveDistance)
    {
        float slopeAngle = collisionInfo.slopeAngle;
        bool wallIsFacingRight = (Mathf.Sign(collisionInfo.slopeNormal.x) > 0) ? true : false;
        bool wallIsFacingDown = (Mathf.Sign(collisionInfo.slopeNormal.y) > 0) ? false : true;

        float signX = wallIsFacingRight ? -1 : 1;
        if (wallIsFacingDown) signX *= -1;
        float signY = wallIsFacingDown ? -1 : 1;

        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance * signY;
        float descendVelocityX = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * signX;

        velocity.y = descendVelocityY;
        velocity.x = descendVelocityX;

        //Debug
        float visualizationMagnifier = 25f;

        Vector3 drawVector = new Vector3(descendVelocityX * visualizationMagnifier, descendVelocityY * visualizationMagnifier, 0);
        Vector3.Angle(drawVector, Vector3.right);
        Debug.DrawRay(transform.position, drawVector, Color.red);

        Debug.DrawRay(transform.position, Vector3.right * descendVelocityX * visualizationMagnifier, Color.yellow);
        Debug.DrawRay(transform.position, Vector3.up * descendVelocityY * visualizationMagnifier, Color.yellow);
    }

    protected void MoveAcrossDiagonalWallVertically(ref Vector2 velocity, Vector2 baseVelocity)
    {
        float moveDistance = Mathf.Abs(baseVelocity.y);
        bool wallIsFacingRight = (Mathf.Sign(collisionInfo.slopeNormal.x) > 0) ? true : false;
        bool wallIsFacingDown = (Mathf.Sign(collisionInfo.slopeNormal.y) > 0) ? false : true;

        float signX = ((wallIsFacingRight && wallIsFacingDown) || (!wallIsFacingRight && !wallIsFacingDown)) ? -1 : 1;
        float signY = (!wallIsFacingDown) ? -1 : 1;

        float descendVelocityY = Mathf.Sin(collisionInfo.slopeAngle * Mathf.Deg2Rad) * moveDistance * signY;
        float descendVelocityX = Mathf.Cos(collisionInfo.slopeAngle * Mathf.Deg2Rad) * moveDistance * signX;

        velocity.y = descendVelocityY;
        velocity.x = descendVelocityX;

        float visualizationMagnifier = 25f;
        Debug.DrawRay(transform.position, new Vector3(descendVelocityX * visualizationMagnifier, descendVelocityY * visualizationMagnifier, 0), Color.green);
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;
        public bool horizontal
        {
            get
            {
                return left || right;
            }
        }
        public bool vertical
        {
            get
            {
                return above || below;
            }
        }
        public Vector2 faceDirection;
        public float slopeAngle;
        public Vector2 slopeNormal;

        public void Setup()
        {
            Reset();
        }

        public void Reset()
        {
            above = below = false;
            left = right = false;
            slopeAngle = 0f;
            slopeNormal = Vector2.zero;
        }

        public void SetFaceDirection(Vector2 velocity)
        {
            if (velocity.x != 0)
            {
                faceDirection.x = (int)Mathf.Sign(velocity.x);
            }
            if (velocity.y != 0)
            {
                faceDirection.y = (int)Mathf.Sign(velocity.y);
            }

            if (velocity.x != 0 && velocity.y == 0 && faceDirection.y != 0)
            {
                faceDirection.y = 0;
            }
            else if (velocity.y != 0 && velocity.x == 0 && faceDirection.x != 0)
            {
                faceDirection.x = 0;
            }
        }
    }
}
