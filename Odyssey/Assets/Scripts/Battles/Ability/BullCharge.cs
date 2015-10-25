using UnityEngine;
using System.Collections;

public class BullCharge : Ability
{
    public BullCharge(Unit owner)
        : base("Bull Charge", "A ferocious charge, granting the owner +2 movement and double damage for 1 turn.", 
		       owner, 1, true, "PowerBull")
    {
        cast_before_attack = true;
        effects_of_ability.Add(new ChangeMovement(owner, 2, 1));
        effects_of_ability.Add(new ChangeDamage(owner, 0, 1, 1));
        effects_of_ability.Add(new ChangePiercingDamage(owner, 0, 1, 1));
    }
}
