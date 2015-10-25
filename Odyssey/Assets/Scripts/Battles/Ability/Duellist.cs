using UnityEngine;
using System.Collections;

public class Duellist : Ability
{
    public Duellist(Unit owner)
        : base("Duellist", "The unit is able to fight an enemy head on, without incurring a counterattack for 1 turn.", 
		       owner, 1, true, "PowerDuel")
    {
        effects_of_ability.Add(new ChangeCounterableAttacks(owner, 1));
        cast_before_attack = true;
    }
}
