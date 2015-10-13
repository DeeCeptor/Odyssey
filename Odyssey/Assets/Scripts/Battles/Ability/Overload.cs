using UnityEngine;
using System.Collections;

public class Overload : Ability
{
    public Overload(Unit owner)
        : base("Kill Order", "The automaton is ordered to kill, and it will do so until its joints become stiff. Damage boost that decreases each turn. Lasts 3 turns.", owner, 1, true)
    {
        effects_of_ability.Add(new ChangeDamage(owner, 30, 0, 3));
        effects_of_ability.Add(new ChangeDamage(owner, 30, 0, 2));
        effects_of_ability.Add(new ChangeDamage(owner, 30, 0, 1));
        cast_before_attack = true;
    }
}
