using UnityEngine;
using System.Collections;

// Terrain effect gotten from standing in a ruins space
public class Ruins : Effect 
{
    public Ruins(Unit receiver)
        : base("Ruins", "Ruins provide ample cover and defencive positions. Boosts defence and ranged defence of any unit standing here.", receiver)
    {
        this.terrain_effect = true;
    }


    public override void ApplyEffect()
    {
        base.ApplyEffect();

        // Gives substantial defense to the unit
        receiver.AdjustDefence(0.30f);
        receiver.AdjustRangedDefence(0.30f);
    }


    public override Effect Clone(Unit receiving_unit)
    {
        return new Ruins(receiving_unit);
    }
}
