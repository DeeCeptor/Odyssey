using UnityEngine;
using System.Collections;

public class CentaurArcher : Unit
{


    public override void AssignAbilities()
    {
        base.AssignAbilities();

        abilities.Add(new PiercingShot(this));
    }
}
