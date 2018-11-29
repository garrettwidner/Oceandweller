using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    public SpriteAnimator spriteAnimator;
    public Player player;
    public Transform spriteTransform;
    public PlayerDigger digger;

    private bool isPlayingSingleAnimation = false;

    public delegate void AnimationAction();
    public AnimationAction OnDigEnded;

    private void OnEnable()
    {
        digger.OnDigStarted += StartDigging;
    }

    private void OnDisable()
    {
        digger.OnDigStarted -= StartDigging;
    }

    private void Update()
    {
        if(!isPlayingSingleAnimation)
        {
            if (player.IsFacingRight)
            {
                spriteTransform.localScale = new Vector3(1, 1, 1);

            }
            else
            {
                spriteTransform.localScale = new Vector3(-1, 1, 1);
            }

            if (player.IsGrounded)
            {

                if (player.IsMoving)
                {
                    spriteAnimator.Play("Run");
                }
                else
                {
                    spriteAnimator.Play("Idle");
                }
            }
            else
            {
                spriteAnimator.Play("Jump");
            }
        }
        
    }

    private void DigAnimationFinished()
    {
        print("Dig animation was finished");
        isPlayingSingleAnimation = false;
        if(OnDigEnded != null)
        {
            OnDigEnded();
        }
    }

    private void StartDigging()
    {
        print("Digging Started");
        isPlayingSingleAnimation = true;
        spriteAnimator.Play("Dig");
    }
}
