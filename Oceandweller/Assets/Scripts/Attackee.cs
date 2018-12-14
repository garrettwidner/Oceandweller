using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackee : MonoBehaviour
{
    public StatusLevel statusLevel;
    public LayerMask damageLayers;
    public BoxCollider2D boxCollider;
    public float coolDownTime = 2f;
    protected bool isCoolingDown = false;

    private void Update()
    {
        if(isCoolingDown)
        {
            return;
        }

        Collider2D foundCollider = Physics2D.OverlapArea(boxCollider.bounds.min, boxCollider.bounds.max, damageLayers);
        if(foundCollider != null)
        {
            Attacker foundAttacker = foundCollider.GetComponent<Attacker>();
            if (foundAttacker != null)
            {
                ReceiveDamageAndCooldown(foundAttacker);
            }
        }
    }

    protected void ReceiveDamageAndCooldown(Attacker attacker)
    {
        float damage = -attacker.GetDamage();
        statusLevel.StartImmediateIncrement(damage);
        print(gameObject.name + " was damaged for " + damage + " damage.");

        isCoolingDown = true;
        CancelInvoke("EndCooldown");
        Invoke("EndCooldown", coolDownTime);
    }

    protected void EndCooldown()
    {
        isCoolingDown = false;
        print("Cooldown ended");
    }

}
