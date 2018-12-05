using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTankHandler : MonoBehaviour
{
    public StatusLevel waterTank;
    public SunSensor sunSensor;
    public PlayerDigger digger;
    [Tooltip("Sun intensity gets multiplied by the given number")]
    public float sunIntensityMultiplier = 0.002f;
    public float digCost = 1f;

    private void OnEnable()
    {
        sunSensor.OnHeatWasSensed += SunDrainSensed;
        digger.OnDigStarted += DigHappened;
    }

    private void OnDisable()
    {
        sunSensor.OnHeatWasSensed -= SunDrainSensed;
        digger.OnDigStarted -= DigHappened;
    }

    public void SunDrainSensed(float intensity)
    {
        waterTank.StartImmediateIncrement(-sunIntensityMultiplier * intensity);
    }

    public void DigHappened()
    {
        waterTank.StartImmediateIncrement(-digCost);
    }
}
