using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour
{
    public SpriteRenderer soilRenderer;
    public SpriteRenderer wetnessRenderer;
    public LayerMask soilLayer;

    public bool startsWet = false;

    public delegate void SoilInfoAction(bool isLeftEmpty, bool isRightEmpty, int depth, bool wet);
    public SoilInfoAction OnMaxDepthReached;
    public SoilInfoAction OnDepthChanged;
    public SoilInfoAction OnWetnessChanged;

    public SoilInfoAction OnAdjacentMaxDepthReached;
    public SoilInfoAction OnAdjacentWetnessChanged;

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

    public bool IsEmpty
    {
        get
        {
            if(IsAtMaxDepth && !isWet)
            {
                return true;
            }
            return false;
        }
    }

    public bool IsLeftEmpty
    {
        get
        {
            if(left != null && left.IsEmpty)
            {
                return true;
            }
            return false;
        }
    }

    public bool IsRightEmpty
    {
        get
        {
            if(right!= null && right.IsEmpty)
            {
                return true;
            }
            return false;
        }
    }

    public bool IsWet
    {
        get
        {
            return isWet;
        }
    }

    private int maxDepth = 3;
    private int minDepth = 0;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        currentDepth = 0;
        FindAdjacentSoils();

        if (startsWet)
        {
            isWet = true;
        }

        if (left != null)
        {
            left.OnMaxDepthReached += AdjacentMaxDepthReached;
            left.OnWetnessChanged += AdjacentWetnessChanged;
        }

        if (right != null)
        {
            right.OnMaxDepthReached += AdjacentMaxDepthReached;
            right.OnWetnessChanged += AdjacentWetnessChanged;
        }

        DepthChanged();
    }

    private void OnDisable()
    {
        if (left != null)
        {
            left.OnMaxDepthReached -= AdjacentMaxDepthReached;
            left.OnWetnessChanged -= AdjacentWetnessChanged;
        }
        if (right != null)
        {
            right.OnMaxDepthReached -= AdjacentMaxDepthReached;
            right.OnWetnessChanged -= AdjacentWetnessChanged;
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

    private void AdjacentMaxDepthReached(bool isLeftEmpty, bool isRightEmpty, int depth, bool isWet)
    {
        if(OnAdjacentMaxDepthReached != null)
        {
            OnAdjacentMaxDepthReached(IsLeftEmpty, IsRightEmpty, currentDepth, IsWet);
        }
    }

    private void AdjacentWetnessChanged(bool isLeftEmpty, bool isRightEmpty, int depth, bool isWet)
    {
        if(OnAdjacentWetnessChanged != null)
        {
            OnAdjacentWetnessChanged(IsLeftEmpty, IsRightEmpty, currentDepth, IsWet);
        }
    }

    private void DepthChanged()
    {
        if (OnDepthChanged != null)
        {
            OnDepthChanged(IsLeftEmpty, IsRightEmpty, currentDepth, IsWet);
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
                OnMaxDepthReached(IsLeftEmpty, IsRightEmpty, currentDepth, isWet);
            }
        }
        else
        {
            isAtMaxDepth = false;
        }

        if (OnDepthChanged != null)
        {
            OnDepthChanged(IsLeftEmpty, IsRightEmpty, currentDepth, isWet);
        }
    }

    public void MakeSoilWet()
    {
        if(!IsWet)
        {
            isWet = true;
            if (OnWetnessChanged != null)
            {
                OnWetnessChanged(IsLeftEmpty, IsRightEmpty, currentDepth, IsWet);
            }
        }

    }

    public void MakeSoilDry()
    {
        if(IsWet)
        {
            isWet = false;
            if (OnWetnessChanged != null)
            {
                OnWetnessChanged(IsLeftEmpty, IsRightEmpty, currentDepth, IsWet);
            }
        }
    }

    public void ChangeWetness()
    {
        if (isWet)
        {
            MakeSoilDry();
        }
        else
        {
            MakeSoilWet();
        }
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

        if(Input.GetKeyDown(KeyCode.L))
        {
            print("-------------------");
        }


    }
}
