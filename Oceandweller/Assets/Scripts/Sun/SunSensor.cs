using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunSensor : MonoBehaviour
{
    public LayerMask shadeLayer;
    public BoxCollider2D shadeCheckArea;

    protected Sun sun;

    protected int sunIntensity;
    protected int sensedIntensity;

    protected bool isShaded = false;
    public bool IsShaded
    {
        get
        {
            return isShaded;
        }
    }

    protected bool previousShadeStatus;

    public delegate void SensorAction(bool newStatus);
    public event SensorAction OnShadeStatusChanged;


    protected virtual void OnEnable()
    {
        FindSunInScene();
        sun.OnIntensityChanged += SunIntensityChanged;
    }

    protected virtual void OnDisable()
    {
        sun.OnIntensityChanged -= SunIntensityChanged;
    }

    protected void Start()
    {
        sunIntensity = sun.Intensity;
    }

    protected virtual void FindSunInScene()
    {
        sun = FindObjectOfType<Sun>();
        sunIntensity = sun.Intensity;

        //print("Sensed starting sun intensity is: " + intensity);
    }

    protected virtual void SunIntensityChanged(int newIntensity)
    {
        //print("Sensed that intensity changed to " + newIntensity);
        sunIntensity = newIntensity;
    }

    protected virtual void Update()
    {
        CheckIfShaded();
        GetSensedIntensity();
        //print("Current intensity for " + transform.parent.gameObject.name + " is " + sensedIntensity);
    }

    public void CheckIfShaded()
    {
        previousShadeStatus = IsShaded;

        Collider2D foundShadeCollider = Physics2D.OverlapArea(shadeCheckArea.bounds.min, shadeCheckArea.bounds.max, shadeLayer);

        if (foundShadeCollider != null)
        {
            isShaded = true;
        }
        else
        {
            isShaded = false;
        }

        if(IsShaded != previousShadeStatus)
        {
            if(OnShadeStatusChanged != null)
            {
                OnShadeStatusChanged(IsShaded);
            }
        }


    }

    public void GetSensedIntensity()
    {
        if(IsShaded)
        {
            sensedIntensity = 0;
        }
        else
        {
            sensedIntensity = sunIntensity;
        }
    }

}
