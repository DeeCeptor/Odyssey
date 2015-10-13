using UnityEngine;
using System.Collections;

public class ShotOfLegends : Ability
{
    public ShotOfLegends(Unit owner)
        : base("Shot of Legends", "The gods smile, and the shot flies true. +2 Attack Range for 1 turn.", owner, 1, true)
    {
        effects_of_ability.Add(new ChangeAttackRange(owner, 2, 1));
        cast_before_attack = true;
    }
}
