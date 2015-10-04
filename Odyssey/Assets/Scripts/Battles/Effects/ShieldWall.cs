using UnityEngine;
using System.Collections;

public class ShieldWall : Effect 
{
	public ShieldWall(Unit receiver)
        : base("Shield Wall", "Receive extra defence for every adjacent hoplite", receiver)
    {
        
    }


    public override void ApplyEffect()
    {
        base.ApplyEffect();


    }


    public override Effect Clone(Unit receiving_unit)
    {
        return new ShieldWall(receiving_unit);
    }
}
