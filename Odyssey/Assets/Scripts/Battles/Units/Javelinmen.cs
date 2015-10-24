using UnityEngine;
using System.Collections;

public class Javelinmen : Unit
{


    public override void AssignAbilities()
    {
        base.AssignAbilities();

        abilities.Add(new PiercingShot(this));
    }
}
