using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownVisualizer : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Attackee attackee;
    [Range(0, 1)]
    public float cooldownAlpha = .5f;

    private void OnEnable()
    {
        attackee.OnCooldownStarted += StartVisualization;
        attackee.OnCooldownEnded += EndVisualization;
    }

    private void OnDisable()
    {
        attackee.OnCooldownStarted -= StartVisualization;
        attackee.OnCooldownEnded -= EndVisualization;
    }

    private void StartVisualization()
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, cooldownAlpha);
    }

    private void EndVisualization()
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);

    }
}
