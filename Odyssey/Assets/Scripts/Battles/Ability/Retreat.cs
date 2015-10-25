using UnityEngine;
using System.Collections;

// Ability that the unit can cast if they're on a retreatable hex.
// Removes the unit from combat.
public class Retreat : Ability
{
    public Retreat(Unit owner)
		: base("Retreat", "There is no honour in pointless defeat. Retreats this unit from combat.", owner, 0, false,
		       "PowerSmash")
    {
        cast_before_attack = true;
        effects_self = false;
    }


    public override void CastAbility()
    {
        base.CastAbility();

        Debug.Log("Retreat ability on unit: " + caster.u_name);
        caster.RetreatUnit();
    }
}
