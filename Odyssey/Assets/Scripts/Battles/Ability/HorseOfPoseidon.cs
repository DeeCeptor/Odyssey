using UnityEngine;
using System.Collections;

public class HorseOfPoseidon : Ability
{
    public HorseOfPoseidon(Unit owner)
        : base("Horses of Poseidon", "Poseidon favours these beasts, allowing them to effortlessly gallop across the land. +3 Movement for 1 turn.", owner, 1)
    {
        effect_of_ability = new ChangeMovement(owner, 3, 1);
    }
}
