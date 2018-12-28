using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringer : MonoBehaviour
{
    public StatusLevel health;
    public GameObject parent;

    private void OnEnable()
    {
        health.OnLevelHitZero += Die;
    }

    private void OnDisable()
    {
        health.OnLevelHitZero -= Die;
    }

    private void Die()
    {
        Destroy(parent);
    }
}
