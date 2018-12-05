using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunSensor : MonoBehaviour
{
    public LayerMask shadeLayer;
    public LayerMask heatLayer;
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

    protected bool isHeated = false;
    public bool IsHeated
    {
        get
        {
            return isHeated;
        }
    }

    protected bool previousShadeStatus;
    protected bool previousHeatedStatus;

    public delegate void SensorAction(bool newStatus);
    public event SensorAction OnShadeStatusChanged;
    public event SensorAction OnHeatStatusChanged;
    public delegate void ContinualAction(float intensity);
    public event ContinualAction OnHeatWasSensed;


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
        CheckIfHeated();
        CheckIfShaded();
        GetSensedIntensity();

        if(sensedIntensity >= 0)
        {
            if(OnHeatWasSensed != null)
            {
                OnHeatWasSensed(sensedIntensity);
            }
        }

        //print("Current intensity for " + transform.parent.gameObject.name + " is " + sensedIntensity);
    }

    public void CheckIfHeated()
    {
        previousHeatedStatus = IsHeated;

        if (IsShaded)
        {
            isHeated = false;

            if (IsHeated != previousHeatedStatus)
            {
                if (OnHeatStatusChanged != null)
                {
                    OnHeatStatusChanged(IsHeated);
                }
            }
            return;
        }

        Collider2D foundHeatCollider = Physics2D.OverlapArea(shadeCheckArea.bounds.min, shadeCheckArea.bounds.max, heatLayer);

        if(foundHeatCollider != null)
        {
            isHeated = true;
        }
        else
        {
            isHeated = false;
        }

        if(IsHeated != previousHeatedStatus)
        {
            if(OnHeatStatusChanged != null)
            {
                OnHeatStatusChanged(IsHeated);
            }
        }
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
        else if(IsHeated)
        {
            sensedIntensity = 2 * sunIntensity;
        }
        else
        {
            sensedIntensity = sunIntensity;
        }
    }

}
