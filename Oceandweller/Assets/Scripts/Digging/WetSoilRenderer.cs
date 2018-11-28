using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WetSoilRenderer : SoilRenderer
{
    public bool showsAllWetness = false;

    public int showWaterAtDepth = 2;

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
        //print("SetSoilSprite() called for " + transform.parent.gameObject.name);
        if(!isWet)
        {
            spriteRenderer.enabled = false;
            return;
        }

        switch(depth)
        {
            case 0:
            case 1:
            case 2:
                SetWetNormalDepthSprite(depth);
                break;
            case 3:
                SetWetMaxDepthSprite(isLeftEmpty, isRightEmpty, depth);
                break;
        }
    }

    private void SetWetNormalDepthSprite(int depth)
    {
        if (showsAllWetness || depth >= showWaterAtDepth)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = levels[depth];
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }

    private void SetWetMaxDepthSprite(bool isLeftEmpty, bool isRightEmpty, int depth)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = levels[depth];
    }
}
