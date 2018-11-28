using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SoilRenderer : MonoBehaviour
{
    public Soil soil;
    public SpriteRenderer spriteRenderer;
    public Sprite[] levels;
    public Sprite leftOpen;
    public Sprite rightOpen;
    public Sprite bothOpen;

    public abstract void SetSoilSprite(bool isLeftEmpty, bool isRightEmpty, int depth, bool isWet);

}
