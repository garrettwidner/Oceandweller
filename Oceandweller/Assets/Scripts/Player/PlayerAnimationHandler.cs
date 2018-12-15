using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    public SpriteAnimator spriteAnimator;
    public Player player;
    public Transform spriteTransform;
    public PlayerDigger digger;
    public Transform attackTransform;
    public PlayerAttacker attacker;

    private bool isPlayingSingleGroundedAnimation = false;
    private bool isPlayingSingleAirborneAnimation = false;
    private bool isPlayingAttackAnimation = false;

    public delegate void AnimationAction();
    public AnimationAction OnDigEnded;

    private void OnEnable()
    {
        digger.OnDigStarted += StartDigging;
        attacker.OnAttackTriggered += StartSlashAnimation;
    }

    private void OnDisable()
    {
        digger.OnDigStarted -= StartDigging;
        attacker.OnAttackTriggered -= StartSlashAnimation;

    }

    private void Update()
    {
        if(!isPlayingSingleGroundedAnimation)
        {
            if(isPlayingSingleAirborneAnimation)
            {
                HandleSpriteTurning();
                if(player.IsGrounded)
                {
                    //AirborneAnimationFinished();
                }
                return;
            }


            HandleSpriteTurning();

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

    private void HandleSpriteTurning()
    {
        if (player.IsFacingRight)
        {
            spriteTransform.localScale = new Vector3(1, 1, 1);
            attackTransform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            spriteTransform.localScale = new Vector3(-1, 1, 1);
            attackTransform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void StartSlashAnimation()
    {
        isPlayingSingleAirborneAnimation = true;
        isPlayingAttackAnimation = true;
        spriteAnimator.Play("JumpSlash");
        print("Should play jumping slash animation");
    }

    private void EndSlashAnimation()
    {
        isPlayingAttackAnimation = false;
        AirborneAnimationFinished();
        print("Played slash end");
    }

    private void AirborneAnimationFinished()
    {
        isPlayingSingleAirborneAnimation = false;
        
    }

    private void StartDigging()
    {
        //print("Digging Started");
        isPlayingSingleGroundedAnimation = true;
        spriteAnimator.Play("Dig");
    }

    private void DigAnimationFinished()
    {
        //print("Dig animation was finished");
        isPlayingSingleGroundedAnimation = false;
        if(OnDigEnded != null)
        {
            OnDigEnded();
        }
    }
    
}
