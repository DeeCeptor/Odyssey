using UnityEngine;
using System.Collections;

public class Archer : Unit
{


    public override void AssignAbilities()
    {
        base.AssignAbilities();

        abilities.Add(new PiercingShot(this));
    }
}
