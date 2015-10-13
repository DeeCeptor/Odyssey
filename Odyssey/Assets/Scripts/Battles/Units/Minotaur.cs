using UnityEngine;
using System.Collections;

public class Minotaur : Unit
{


    public override void AssignAbilities()
    {
        base.AssignAbilities();

        abilities.Add(new BullCharge(this));
    }
}
