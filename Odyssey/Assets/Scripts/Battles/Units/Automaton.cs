using UnityEngine;
using System.Collections;

public class Automaton : Unit
{


    public override void AssignAbilities()
    {
        base.AssignAbilities();

        abilities.Add(new Overload(this));
    }
}
