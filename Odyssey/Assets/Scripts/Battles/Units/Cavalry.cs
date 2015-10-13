using UnityEngine;
using System.Collections;

public class Cavalry : Unit
{


    public override void AssignAbilities()
    {
        base.AssignAbilities();

        abilities.Add(new HorseOfPoseidon(this));
    }
}
