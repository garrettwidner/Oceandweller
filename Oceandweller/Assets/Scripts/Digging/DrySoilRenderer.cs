using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrySoilRenderer : SoilRenderer
{
    public bool debug = true;

    private void OnEnable()
    {
        soil.OnWetnessChanged += SetSoilSprite;
        soil.OnDepthChanged += SetSoilSprite;
        soil.OnAdjacentWetnessChanged += SetSoilSprite;
        soil.OnAdjacentMaxDepthReached += SetSoilSprite;
    }

    private void OnDisable()
    {
        soil.OnWetnessChanged -= SetSoilSprite;
        soil.OnDepthChanged -= SetSoilSprite;
        soil.OnAdjacentWetnessChanged -= SetSoilSprite;
        soil.OnAdjacentMaxDepthReached -= SetSoilSprite;
    }

    public override void SetSoilSprite(bool isLeftEmpty, bool isRightEmpty, int depth, bool isWet)
    {
        //print(gameObject.name + " thinks that its depth is " + depth);
        switch (depth)
        {
            case 0:
            case 1:
            case 2:
                SetNormalDepthSprite(depth);
                break;
            case 3:
                SetMaxDepthSprite(isLeftEmpty, isRightEmpty, depth, isWet);
                break;
        }
    }

    private void SetNormalDepthSprite(int depth)
    {
        spriteRenderer.sprite = levels[depth];
    }

    private void SetMaxDepthSprite(bool isLeftEmpty, bool isRightEmpty, int depth, bool isWet)
    {
        //print("Max depth sprite set for " + gameObject.name);
        if (isLeftEmpty && isRightEmpty && !isWet)
        {
            if(debug)
            {
                print(gameObject.name + " thinks BOTH sides are open");
            }

            spriteRenderer.sprite = bothOpen;
        }
        else if (isLeftEmpty && !isWet)
        {
            if (debug)
            {
                print(gameObject.name + " thinks LEFT side is open");
            }

            spriteRenderer.sprite = leftOpen;
        }
        else if (isRightEmpty && !isWet)
        {
            if (debug)
            {
                print(gameObject.name + " thinks RIGHT side is open");
            }

            spriteRenderer.sprite = rightOpen;
        }
        else
        {
            if (debug)
            {
                print(gameObject.name + " thinks NEITHER side is open");
            }

            spriteRenderer.sprite = levels[depth];
        }
    }

}
