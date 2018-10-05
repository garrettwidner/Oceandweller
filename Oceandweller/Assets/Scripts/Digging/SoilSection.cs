using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilSection : MonoBehaviour
{
    public Type type;
    public enum Type
    {
        Sand,
        Dirt,
        Rock,
        Plate,
        Bedrock
    };

    [Range(1,3)]
    public int lightness;

    [Header("Sprite Setup")]
    public SpriteRenderer spriteRenderer;

    public Sprite bothConnected;
    public Sprite leftConnected;
    public Sprite rightConnected;
    public Sprite noneConnected;

    private SoilSection left;
    private SoilSection right;

    private bool isLeftmost;
    public bool IsLeftMost
    {
        get
        {
            return isLeftmost;
        }
    }
    private bool isRightmost;
    public bool IsRightMost
    {
        get
        {
            return isRightmost;
        }
    }

    private void Start()
    {
        spriteRenderer.sprite = bothConnected;
    }

    public void SetUpSoil(SoilSection soilToLeft, SoilSection soilToRight)
    {
        left = soilToLeft;
        right = soilToRight;

        isLeftmost = false;
        if(left == null)
        {
            isLeftmost = true;
        }

        isRightmost = false;
        if(right = null)
        {
            isRightmost = true;
        }
    }

    public void SoilOnSameLevelWasRemoved(SoilSection soil)
    {
        if(soil == left)
        {
            RemoveLeftSoil();
        }
        if(soil == right)
        {
            RemoveRightSoil();
        }
    }

    public void RemoveSelf()
    {
        Destroy(this);
    }

    private void RemoveLeftSoil()
    {
        left = null;
        if (right != null || isRightmost)
        {
            spriteRenderer.sprite = rightConnected;
        }
        else
        {
            spriteRenderer.sprite = noneConnected;
        }
    }

    private void RemoveRightSoil()
    {
        right = null;
        if(left != null || isLeftmost)
        {
            spriteRenderer.sprite = leftConnected;
        }
        else
        {
            spriteRenderer.sprite = noneConnected;
        }
    }

    
}
