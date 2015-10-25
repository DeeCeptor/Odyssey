using UnityEngine;
using System.Collections;

public class PiercingShot : Ability
{
    public PiercingShot(Unit owner)
        : base("Piercing Shot", "A well-placed shot. All damage is converted to piercing damage for 1 turn.", 
		       owner, 1, true, "PowerPiercing")
    {
        float normal_damage = caster.GetDamage();
        effects_of_ability.Add(new ChangeDamage(owner, -normal_damage, 0, 1));
        effects_of_ability.Add(new ChangePiercingDamage(owner, normal_damage, 0, 1));
        cast_before_attack = true;
    }
}
