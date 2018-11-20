using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteShade : MonoBehaviour
{
    public Color normalPlayerColor;
    public Color shadedPlayerColor;

    public SunSensor playerSunSensor;
    public SpriteRenderer playerSpriteRenderer;

    private void OnEnable()
    {
        playerSunSensor.OnShadeStatusChanged += ShadeChanged;
    }

    private void OnDisable()
    {
        playerSunSensor.OnShadeStatusChanged -= ShadeChanged;
    }

    private void ShadeChanged(bool isShaded)
    {
        if(isShaded)
        {
            playerSpriteRenderer.color = shadedPlayerColor;
        }
        else
        {
            playerSpriteRenderer.color = normalPlayerColor;
        }
    }

}
