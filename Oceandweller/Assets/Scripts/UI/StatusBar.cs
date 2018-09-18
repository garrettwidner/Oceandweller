using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBar : MonoBehaviour
{
    [SerializeField] private StatusLevel statusLevel;

    [SerializeField] private RectTransform bar;
    [SerializeField] private RectTransform fill;

    [SerializeField] private bool isVertical = false;

    [Tooltip("This should be the statusLevel on the bar at which the fill 'disappears' when empty")]
    [SerializeField] private float barEmptyPoint = 0;
    [Tooltip("This should be the statusLevel on the bar at which the fill 'disappears' when full")]
    [SerializeField] private float barFullPoint = 100;

    private float maxWidthWhenFull;
    private float minWidth;
    private float containerStartWidth;
    private float barWidthToMaxStatusLevelRatio;

    private void Start()
    {
        if(barEmptyPoint >= barFullPoint)
        {
            Debug.LogWarning("Bar empty point must be below full point. Resetting to 0 and statusLevel max, respectively");
            barEmptyPoint = 0;
            barFullPoint = 100;
        }

        if(isVertical)
        {
            containerStartWidth = bar.rect.height;
            float containerActiveMovementZone = statusLevel.MaxLevel - barEmptyPoint;
            barWidthToMaxStatusLevelRatio = containerStartWidth / containerActiveMovementZone;
        }
        else
        {
            containerStartWidth = bar.rect.width;
            float containerActiveMovementZone = statusLevel.MaxLevel - barEmptyPoint;
            barWidthToMaxStatusLevelRatio = containerStartWidth / containerActiveMovementZone;
        }

        
    }

    private void Update()
    {
        if(isVertical)
        {
            maxWidthWhenFull = bar.rect.height;
            float currentStatRatio = statusLevel.CurrentLevel / statusLevel.MaxLevel;

            float zeroPointModifiedStatLevel = currentStatRatio * (barFullPoint - barEmptyPoint);
            float visibleStatLevel = zeroPointModifiedStatLevel + barEmptyPoint;
            float zeroPointModifiedStatRatio = visibleStatLevel / statusLevel.MaxLevel;

            float statFillWidth = zeroPointModifiedStatRatio * maxWidthWhenFull;
            fill.sizeDelta = new Vector2(fill.sizeDelta.x, statFillWidth);
        }
        else
        {
            maxWidthWhenFull = bar.rect.width;
            float currentStatRatio = statusLevel.CurrentLevel / statusLevel.MaxLevel;

            float zeroPointModifiedStatLevel = currentStatRatio * (barFullPoint - barEmptyPoint);
            float visibleStatLevel = zeroPointModifiedStatLevel + barEmptyPoint;
            float zeroPointModifiedStatRatio = visibleStatLevel / statusLevel.MaxLevel;

            float statFillWidth = zeroPointModifiedStatRatio * maxWidthWhenFull;
            fill.sizeDelta = new Vector2(statFillWidth, fill.sizeDelta.y);
        }
        
    }

}
