using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    public SpriteAnimator spriteAnimator;
    public Player player;
    public Transform spriteTransform;

    private void Update()
    {
        if(player.IsFacingRight)
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
