using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunSensor : MonoBehaviour
{
    protected Sun sun;

    protected int intensity;

    protected void OnEnable()
    {
        FindSunInScene();
        sun.OnIntensityChanged += SunIntensityChanged;
    }

    protected void OnDisable()
    {
        sun.OnIntensityChanged -= SunIntensityChanged;
    }

    protected void FindSunInScene()
    {
        sun = FindObjectOfType<Sun>();
        intensity = sun.Intensity;

        //print("Sensed starting sun intensity is: " + intensity);
    }


    protected void SunIntensityChanged(int newIntensity)
    {
        //print("Sensed that intensity changed to " + newIntensity);
        intensity = newIntensity;
    }





}
