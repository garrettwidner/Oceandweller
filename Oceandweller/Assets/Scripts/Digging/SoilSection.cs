using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilSection : MonoBehaviour
{
    public int order; //?


    //1-3, 1 is lightest
    public int lightness;
    public Type type;
    public SoilSection left;
    public SoilSection right;

    [Header("Sprite Setup")]
    public SpriteRenderer spriteRenderer;

    public Sprite bothConnected;
    public Sprite leftConnected;
    public Sprite rightConnected;
    public Sprite noneConnected;

    private bool isLeftmost;
    private bool isRightmost;

    private void Start()
    {
        spriteRenderer.sprite = bothConnected;
    }

    public void SetUpSoil(bool isLeftmostInLayer, bool isRightmostInLayer)
    {
        isLeftmost = isLeftmostInLayer;
        isRightmost = isRightmostInLayer;
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

    public enum Type
    {
        Sand,
        Dirt,
        Rock,
        Plate,
        Caliche
    };
}
