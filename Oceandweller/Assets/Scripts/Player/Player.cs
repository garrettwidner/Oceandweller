using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Controller2D))]
public class Player : MonoBehaviour 
{
    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = 0.4f;
    public float moveSpeed = 6;

    private float accelerationTimeAirborne = 0.2f;
    private float accelerationTimeGrounded = 0.1f;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;
    
    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;

    private float timeToWallUnstick;

    private float gravity;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    private Vector3 velocity;
    private float velocityXSmoothing;

    //Dig
    private bool canMove = true;
    public PlayerDigger digger;
    public PlayerAnimationHandler animationHandler;

    //Used to smooth errors where the character shows as not grounded for only a single frame
    private bool wasGroundedLastFrame;

    Controller2D controller;
    private PlayerActions playerActions;

    private float minVelocityConsideredMoving = 0.5f;

    public bool IsMoving
    {
        get
        {
            if(Mathf.Abs(velocity.x) > minVelocityConsideredMoving)
            {
                return true;
            }
            return false;
        }
    }

    public bool IsFacingRight
    {
        get
        {
            if(controller.collisions.faceDir == -1)
            {
                return false;
            }
            return true;
        }
    }

    public bool IsGrounded
    {
        get
        {
            if(controller.collisions.below || !wasGroundedLastFrame)
            {
                return true;
            }
            return false;
        }
    }

    private void OnEnable()
    {
        digger.OnDigStarted += Immobilize;
        animationHandler.OnDigEnded += Mobilize;
    }

    private void OnDisable()
    {
        digger.OnDigStarted -= Immobilize;
        animationHandler.OnDigEnded -= Mobilize;
    }

    private void Start()
    {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

        //print("Gravity: " + gravity + " Jump Velocity: " + maxJumpVelocity);

        playerActions = PlayerActions.CreateWithDefaultBindings();
    }

    private void Update()
    {
        //Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 input = new Vector2(playerActions.Move.X, playerActions.Move.Y);
        input = input.ClosestCardinalDirection();

        if(!canMove)
        {
            input = Vector2.zero;
        }

        int wallDirX = (controller.collisions.left) ? -1 : 1;

        float targetVelocityX = input.x * moveSpeed;
        //print(targetVelocityX);
        float accelerationTime = controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTime);

        bool wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;
            if (velocity.y <= -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (input.x != wallDirX && input.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }

        if (playerActions.Jump.WasPressed)
        {
            if (wallSliding)
            {
                if (wallDirX == input.x)
                {
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }
                else if (input.x == 0)
                {
                    velocity.x = -wallDirX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                }
                else
                {
                    velocity.x = -wallDirX * wallLeap.x;
                    velocity.y = wallLeap.y;
                }
            }
            if (controller.collisions.below)
            {
                velocity.y = maxJumpVelocity;

            }
        }
        if (playerActions.Jump.WasReleased)
        {
            if (velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
            }
        }

        velocity.y += gravity * Time.deltaTime;
       
        controller.Move(velocity * Time.deltaTime, input);

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        wasGroundedLastFrame = controller.collisions.below;
    }

    private void Immobilize()
    {
        canMove = false;
    }

    private void Mobilize()
    {
        canMove = true;
    }

}
