using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour
{
    public SpriteRenderer soilRenderer;
    public SpriteRenderer wetnessRenderer;

    public Sprite[] soilSprites;
    public Sprite[] wetSprites;

    private bool isWet = false;
    private Soil left;
    private Soil right;
    private int currentDepth;

    private bool isFullyOpen = false;

    private int maxDepth = 3;
    private int minDepth = 0;

    //If wet, when it reaches max depth, needs to spawn water block. 
    //Only once water is taken from a cell will the cell open at the sides to connect with other cells.

    private void Start()
    {
        currentDepth = 0;
        SetSpriteFromCurrentDepth();
    }

    private void SetSpriteFromCurrentDepth()
    {
        soilRenderer.sprite = soilSprites[currentDepth];

        if(isWet)
        {
            wetnessRenderer.enabled = true;
            wetnessRenderer.sprite = wetSprites[currentDepth];
        }
        else
        {
            wetnessRenderer.enabled = false;
        }
    }

    public void Dig(int levelsDug)
    {
        ChangeCurrentDepth(levelsDug);
    }

    private void ChangeCurrentDepth(int change)
    {
        currentDepth += change;
        if(currentDepth > maxDepth)
        {
            currentDepth = maxDepth;
        }
        else if(currentDepth < minDepth)
        {
            currentDepth = minDepth;
        }

        SetSpriteFromCurrentDepth();
    }

    public void MakeSoilWet()
    {
        isWet = true;
        SetSpriteFromCurrentDepth();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            print("Resetting soil level to highest");
            ChangeCurrentDepth(-900);
        }

        if(Input.GetKeyDown(KeyCode.H))
        {
            print("Switching soil wetness state");
            isWet = !isWet;
            SetSpriteFromCurrentDepth();
        }
    }
}
