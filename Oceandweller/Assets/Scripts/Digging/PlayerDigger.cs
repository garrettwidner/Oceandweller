using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDigger : MonoBehaviour
{
    public LayerMask soilLayer;
    public Transform digLocation;
    public int digStrength = 1;

    public void Dig(Soil soil)
    {
        soil.Dig(digStrength);
    }

    public void ChangeWetness(Soil soil)
    {
        soil.ChangeWetness();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            Collider2D collider = Physics2D.OverlapPoint(digLocation.position, soilLayer);
            if(collider != null)
            {
                Dig(collider.GetComponent<Soil>());
            }
        }
        else if(Input.GetKeyDown(KeyCode.U))
        {
            Collider2D collider = Physics2D.OverlapPoint(digLocation.position, soilLayer);
            if (collider != null)
            {
                ChangeWetness(collider.GetComponent<Soil>());
            }
        }
    }
}
