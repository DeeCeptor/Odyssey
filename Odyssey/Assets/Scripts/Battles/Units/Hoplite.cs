using UnityEngine;
using System.Collections;

public class Hoplite : Unit
{


    public override void AssignAbilities()
    {
        base.AssignAbilities();

        effects_on_unit.Add(new ShieldWall(this));
    }
}
