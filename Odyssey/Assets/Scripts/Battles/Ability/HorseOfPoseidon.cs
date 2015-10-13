using UnityEngine;
using System.Collections;

public class HorseOfPoseidon : Ability
{
    public HorseOfPoseidon(Unit owner)
        : base("Horses of Poseidon", "Poseidon favours these beasts, allowing them to effortlessly gallop across the land. +3 Movement for 1 turn.", owner, 1, true)
    {
        effects_of_ability.Add(new ChangeMovement(owner, 3, 1));
        cast_before_move = true;
    }
}
