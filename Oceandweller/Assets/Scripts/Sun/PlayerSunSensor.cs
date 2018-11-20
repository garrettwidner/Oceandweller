using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSunSensor : SunSensor
{
    public StatusLevel waterTank;
    [Tooltip("This multiplies the sun's intensity by the given number")]
    public float intensityMultiplier = .1f;

    protected override void Update()
    {
        base.Update();

        waterTank.StartImmediateIncrement(-intensityMultiplier * sensedIntensity);
    }



}
