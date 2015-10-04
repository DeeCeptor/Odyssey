using UnityEngine;
using System.Collections;

// Terrain effect gotten from standing in a hills space
// Gives +1 attack range and bonus to ranged damage
public class Hill : Effect
{
    public Hill(Unit receiver)
        : base("Hill", "Increased elevation grants increased range and damage to ranged units.", receiver)
    {
        this.terrain_effect = true;
    }


    public override void ApplyEffect()
    {
        base.ApplyEffect();

        if (receiver.is_ranged_unit)    // Only applies to ranged units
        {
            receiver.AdjustRange(1);
            receiver.AdjustDamage(0, 0.4f);
            receiver.AdjustPiercingDamage(0, 0.4f);
        }
    }


    public override Effect Clone(Unit receiving_unit)
    {
        return new Hill(receiving_unit);
    }
}
