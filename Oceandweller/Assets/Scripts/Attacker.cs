using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    protected bool isEngaged = true;
    public float damageDone;
    public BoxCollider2D attackCollider;

    private void Start()
    {
        isEngaged = true;
    }

    public float GetDamage()
    {
        if(isEngaged)
        {
            return damageDone;
        }
        return 0f;
    }

    public void Engage()
    {
        isEngaged = true;
    }

    public void Disengage()
    {
        isEngaged = false;
    }
}
