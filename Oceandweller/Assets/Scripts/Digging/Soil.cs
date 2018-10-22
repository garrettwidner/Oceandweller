using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public Sprite[] dry;
    public Sprite[] wet;

    private bool isWet = false;
    private Soil left;
    private Soil right;
    private int currentDepth;

    private bool isOpen = false;

    private int maxStartDepth = 3;

    private int maxDepth = 7;
    private int minDepth = 0;

    //If wet, when it reaches max depth, needs to spawn water block. 
    //Only once water is taken from a cell will the cell open at the sides to connect with other cells.

    private void Start()
    {
        currentDepth = Random.Range(minDepth, maxStartDepth + 1);
        print(currentDepth);
        SetSpriteFromCurrentDepth();
    }

    private void SetSpriteFromCurrentDepth()
    {
        spriteRenderer.sprite = isWet ? wet[currentDepth] : dry[currentDepth];

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
