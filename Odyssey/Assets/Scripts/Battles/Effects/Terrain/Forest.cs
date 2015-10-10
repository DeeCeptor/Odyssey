using UnityEngine;
using System.Collections;

// Terrain effect gotten from standing in a forest space
// Gives considerable ranged defence
public class Forest : Effect
{
    public Forest(Unit receiver)
        : base("Forest", "Arrows and stones are caught and deflected by thick tree limbs, providing excellent cover.", receiver)
    {
        this.terrain_effect = true;
    }


    public override void ApplyEffect()
    {
        base.ApplyEffect();

        receiver.AdjustDefence(0.10f);
        receiver.AdjustRangedDefence(0.50f);
    }


    public override Effect Clone(Unit receiving_unit)
    {
        return new Forest(receiving_unit);
    }
}
