using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunSensor : MonoBehaviour
{
    private Sun sun;

    private int intensity;

    private void OnEnable()
    {
        sun = FindObjectOfType<Sun>();
        intensity = sun.Intensity;

        sun.OnIntensityChanged += SunIntensityChanged;

        print("Sensed starting sun intensity is: " + intensity);
    }

    private void Start()
    {
        
    }

    private void SunIntensityChanged(int newIntensity)
    {
        print("Sensed that intensity changed to " + newIntensity);
        intensity = newIntensity;
    }





}
