using UnityEngine;
using System.Collections;

public class Slinger : Unit
{


    public override void AssignAbilities()
    {
        base.AssignAbilities();

        abilities.Add(new ShotOfLegends(this));
    }
}
