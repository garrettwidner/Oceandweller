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
            playerSpriteRenderer.color = new Color(shadedPlayerColor.r, shadedPlayerColor.g, shadedPlayerColor.b, playerSpriteRenderer.color.a);
        }
        else
        {
            playerSpriteRenderer.color = new Color(normalPlayerColor.r, normalPlayerColor.g, normalPlayerColor.b, playerSpriteRenderer.color.a);
        }
    }

    private void HeatChanged (bool isHeated)
    {
        if(isHeated)
        {
            playerSpriteRenderer.color = new Color(heatedPlayerColor.r, heatedPlayerColor.g, heatedPlayerColor.b, playerSpriteRenderer.color.a);
        }
        else
        {
            playerSpriteRenderer.color = new Color(normalPlayerColor.r, normalPlayerColor.g, normalPlayerColor.b, playerSpriteRenderer.color.a);
        }
    }

}
