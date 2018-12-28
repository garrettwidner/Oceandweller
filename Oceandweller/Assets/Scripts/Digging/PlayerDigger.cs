using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDigger : MonoBehaviour
{
    public LayerMask soilLayer;
    public Transform digLocation;
    public int digStrength = 1;
    public float digTimeAfterAnimationStart = 0.1f;

    public delegate void DigAction();
    public DigAction OnDigStarted;

    private PlayerActions playerActions;

    private void Start()
    {
        playerActions = PlayerActions.CreateWithDefaultBindings();
    }

    public void Dig(Soil soil)
    {
        StartCoroutine(DigPhysicalHole(soil));

        if(OnDigStarted != null)
        {
            OnDigStarted();
        }
    }

    public IEnumerator DigPhysicalHole(Soil soil)
    {
        yield return new WaitForSeconds(digTimeAfterAnimationStart);
        soil.Dig(digStrength);
    }

    public void ChangeWetness(Soil soil)
    {
        soil.ChangeWetness();
    }

    private void Update()
    {
        if(playerActions.Down.IsPressed && playerActions.Attack.WasPressed)
        {
            Collider2D collider = Physics2D.OverlapPoint(digLocation.position, soilLayer);
            if (collider != null)
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
