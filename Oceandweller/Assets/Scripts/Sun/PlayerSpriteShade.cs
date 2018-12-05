using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteShade : MonoBehaviour
{
    public Color normalPlayerColor;
    public Color shadedPlayerColor;
    public Color heatedPlayerColor;

    public SunSensor playerSunSensor;
    public SpriteRenderer playerSpriteRenderer;

    private void OnEnable()
    {
        playerSunSensor.OnShadeStatusChanged += ShadeChanged;
        playerSunSensor.OnHeatStatusChanged += HeatChanged;
    }

    private void OnDisable()
    {
        playerSunSensor.OnShadeStatusChanged -= ShadeChanged;
        playerSunSensor.OnHeatStatusChanged -= HeatChanged;
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

    private void HeatChanged (bool isHeated)
    {
        if(isHeated)
        {
            playerSpriteRenderer.color = heatedPlayerColor;
        }
        else
        {
            playerSpriteRenderer.color = normalPlayerColor;
        }
    }

}
