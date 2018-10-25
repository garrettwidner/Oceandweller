using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{

    public int startingIntensity = 10;

    private int intensity;
    public int Intensity
    {
        get
        {
            return intensity;
        }
    }

    public delegate void SunAction(int newIntensity);
    public event SunAction OnIntensityChanged;

    private void OnEnable()
    {
        intensity = startingIntensity;
    }

    private void ChangeIntensity(int change)
    {
        intensity += change;
        if (intensity < 0)
        {
            intensity = 0;
        }

        if(OnIntensityChanged != null)
        {
            OnIntensityChanged(intensity);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            ChangeIntensity(5);
        }
    }
}
