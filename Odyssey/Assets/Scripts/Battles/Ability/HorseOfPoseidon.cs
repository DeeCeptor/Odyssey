using UnityEngine;
using System.Collections;

public class HorseOfPoseidon : Ability
{
    public HorseOfPoseidon(Unit owner)
        : base("Fleet", "Grants incredible swiftness. +3 Movement for 1 turn.", 
		       owner, 1, true, "PowerPoseidon")
    {
        effects_of_ability.Add(new ChangeMovement(owner, 3, 1));
        cast_before_move = true;
    }
}
