using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour
{
    public SpriteRenderer soilRenderer;
    public SpriteRenderer wetnessRenderer;
    public LayerMask soilLayer;

    public bool showsWetnessAtFirstLevel = false;

    public Sprite[] soilSprites;
    public Sprite[] wetSprites;

    public delegate void SoilAction();
    public SoilAction OnMaxDepthReached;

    private BoxCollider2D boxCollider;

    private bool isWet = false;
    private Soil left;
    private Soil right;
    private int currentDepth;

    public bool IsAtMaxDepth
    {
        get
        {
            return isAtMaxDepth;
        }
    }
    private bool isAtMaxDepth = false;

    private int maxDepth = 3;
    private int minDepth = 0;

    //If wet, when it reaches max depth, needs to spawn water block. 
    //Only once water is taken from a cell will the cell open at the sides to connect with other cells.

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        currentDepth = 0;
        FindAdjacentSoils();

        if (left != null)
        {
            left.OnMaxDepthReached += SetSpriteFromCurrentDepth;
        }
        if (right != null)
        {
            right.OnMaxDepthReached += SetSpriteFromCurrentDepth;
        }

        SetSpriteFromCurrentDepth();
    }

    private void OnDisable()
    {
        if (left != null)
        {
            left.OnMaxDepthReached -= SetSpriteFromCurrentDepth;
        }
        if (right != null)
        {
            right.OnMaxDepthReached -= SetSpriteFromCurrentDepth;
        }
    }

    private void FindAdjacentSoils()
    {
        left = PopulateSoil(Vector2.left);
        right = PopulateSoil(Vector2.right);
    }

    private Soil PopulateSoil(Vector2 direction)
    {
        float xDirection = Mathf.Sign(direction.x);
        Vector2 searchPoint = (Vector2)transform.position + (Vector2.right * boxCollider.bounds.size.x * xDirection);
        Collider2D foundSoil = Physics2D.OverlapPoint(searchPoint, soilLayer);

        if (foundSoil != null)
        {
            if (foundSoil.gameObject.tag == gameObject.tag)
            {
                return foundSoil.GetComponent<Soil>();
            }
        }
        return null;
    }

    private void SetSpriteFromCurrentDepth()
    {
        //print("Setting sprite depth for " + gameObject.name);
        soilRenderer.sprite = soilSprites[currentDepth];
        wetnessRenderer.sprite = wetSprites[currentDepth];

        if(currentDepth == 0 && isWet)
        {
            if(showsWetnessAtFirstLevel)
            {
                wetnessRenderer.enabled = true;
                wetnessRenderer.sprite = wetSprites[currentDepth];
            }
        }
        else if (currentDepth != 0 && isWet)
        {
            wetnessRenderer.enabled = true;
            wetnessRenderer.sprite = wetSprites[currentDepth];
        }
        else
        {
            wetnessRenderer.enabled = false;
        }

        if(currentDepth == maxDepth)
        {
            //print("Depth is max depth. Setting accordingly for " + gameObject.name);
            int deepIndex = 0;

            bool isRightOpen = false;
            bool isLeftOpen = false;

            if(left != null && left.isAtMaxDepth && !left.isWet)
            {
                isLeftOpen = true;
            }
            if(right != null && right.isAtMaxDepth && !right.isWet)
            {
                isRightOpen = true;
            }

            print("For " + gameObject.name + " Right open: " + isRightOpen + "  Left open: " + isLeftOpen);

            if(isRightOpen && isLeftOpen)
            {
                deepIndex = 5;
            }
            else if(isLeftOpen)
            {
                deepIndex = 6;
            }
            else if(isRightOpen)
            {
                deepIndex = 4;
            }
            else
            {
                deepIndex = 3;
            }

            soilRenderer.sprite = soilSprites[deepIndex];
            wetnessRenderer.sprite = wetSprites[deepIndex];
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
        
        if(currentDepth == maxDepth)
        {
            isAtMaxDepth = true;
            if(OnMaxDepthReached != null)
            {
                OnMaxDepthReached();
            }
        }
        else
        {
            isAtMaxDepth = false;
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

        if(Input.GetKeyDown(KeyCode.Y))
        {
            print(gameObject.name + " is at max depth: " + IsAtMaxDepth);
            print(gameObject.name + " is wet: " + isWet);
        }

        if(Input.GetKeyDown(KeyCode.H))
        {
            print("Switching soil wetness state");
            isWet = !isWet;
            SetSpriteFromCurrentDepth();
        }
    }
}
