using UnityEngine;
using System.Collections;

// Ability that the unit can cast if they're on a retreatable hex.
// Removes the unit from combat.
public class Retreat : Ability
{
    public Retreat(Unit owner)
        : base("Retreat", "Retreats this unit from combat. There is no honour in pointless defeat.", owner, 0)
    {
        effects_self = false;
    }


    public override void CastAbility()
    {
        base.CastAbility();

        Debug.Log("Retreat ability on unit: " + caster.u_name);
        caster.RetreatUnit();
    }
}
