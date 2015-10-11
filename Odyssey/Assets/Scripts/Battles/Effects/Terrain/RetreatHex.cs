using UnityEngine;
using System.Collections;

// Player units get the ability to retreat when on a retreatable hex
public class RetreatHex : Effect
{
    public RetreatHex(Unit receiver)
        : base("", "", receiver)
    {
        this.terrain_effect = true;
    }


    public override void ApplyEffect()
    {
        base.ApplyEffect();

        // Check if the unit can already retreat
        foreach (Ability ability in receiver.abilities)
        {
            if (ability.ability_name == "Retreat")
            {
                return;
            }
        }

        // Add the retreat ability to the unit
        receiver.abilities.Add(new Retreat(receiver));
    }


    public override void RemoveThisEffect()
    {
        base.RemoveThisEffect();

        // Remove the retreat ability from the unit
        for (int x = 0; x < receiver.abilities.Count; x++)
        {
            if (receiver.abilities[x].ability_name == "Retreat")
            {
                receiver.abilities.RemoveAt(x);
            }
        }
    }


    public override Effect Clone(Unit receiving_unit)
    {
        return new RetreatHex(receiving_unit);
    }
}
