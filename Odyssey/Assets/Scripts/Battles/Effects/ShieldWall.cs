using UnityEngine;
using System.Collections;

// This unit gains defence for every hoplite around it
public class ShieldWall : Effect 
{
	public ShieldWall(Unit receiver)
        : base("Shield Wall", "Receive extra defence for every adjacent hoplite", receiver)
    {
        
    }


    public override void ApplyEffect()
    {
        base.ApplyEffect();

        foreach (Edge edge in receiver.location.neighbours)
        {
            if (edge.destination.occupying_unit != null 
                && edge.destination.occupying_unit.u_name == "Hoplite")
            {
                receiver.AdjustDefence(0.05f);
                receiver.AdjustRangedDefence(0.05f);
            }
        }
    }


    public override Effect Clone(Unit receiving_unit)
    {
        return new ShieldWall(receiving_unit);
    }
}
