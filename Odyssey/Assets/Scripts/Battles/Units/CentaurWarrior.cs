using UnityEngine;
using System.Collections;

public class CentaurWarrior : Unit
{


    public override void AssignAbilities()
    {
        base.AssignAbilities();

        abilities.Add(new BullCharge(this));
    }
}
