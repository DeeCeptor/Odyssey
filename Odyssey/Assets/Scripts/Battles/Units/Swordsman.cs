using UnityEngine;
using System.Collections;

public class Swordsman : Unit
{


    public override void AssignAbilities()
    {
        base.AssignAbilities();

        abilities.Add(new Duellist(this));
    }
}
