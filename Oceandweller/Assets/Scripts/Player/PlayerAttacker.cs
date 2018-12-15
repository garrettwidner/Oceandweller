using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : Attacker
{
    //Tells animator when to attack, sets whether it is engaged based on animation in real time
    private PlayerActions playerActions;

    public delegate void AttackAction();
    public AttackAction OnAttackTriggered;

    private void Start()
    {
        playerActions = PlayerActions.CreateWithDefaultBindings();
    }

    private void Update()
    {
        if(playerActions.Attack.WasPressed)
        {
            if(OnAttackTriggered != null)
            {
                OnAttackTriggered();
            }
        }
    }
    private void StartAttack()
    {

    }

}
