using UnityEngine;
using System.Collections;

public class Sweep : Ability
{
	public static float defence_bonus = 0.10f;
	
	public Sweep(Unit owner)
		: base("Sweep", "Allows the unit to attack a total of 3 times this turn.", 
		       owner, 1, false, "PowerSweep")
	{
		cast_before_attack = true;
	}
	
	
	public override void CastAbility()
	{
		base.CastAbility();

		caster.remaining_attacks_this_turn = 3;
	}
}
