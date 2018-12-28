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

    public delegate void CooldownAction();
    public event CooldownAction OnCooldownStarted;
    public event CooldownAction OnCooldownEnded;

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
        print(gameObject.transform.parent.name + " was damaged for " + damage + " damage.");

        isCoolingDown = true;
        CancelInvoke("EndCooldown");
        Invoke("EndCooldown", coolDownTime);

        if(OnCooldownStarted != null)
        {
            OnCooldownStarted();
        }
    }

    protected void EndCooldown()
    {
        isCoolingDown = false;
        print("Cooldown ended");

        if(OnCooldownEnded != null)
        {
            OnCooldownEnded();
        }
    }

}
