using UnityEngine;
using System.Collections;

// Terrain effect gotten from standing in a hills space
// Gives +1 attack range and bonus to ranged damage
public class ChangeMovement : Effect
{
    int movement_offset;
    int duration;

    public ChangeMovement(Unit receiver, int adjust_movement_by, int offset_duration)
        : base("Altered Movement", "Movement speed changed by " + adjust_movement_by, receiver)
    {
        this.terrain_effect = true;
        movement_offset = adjust_movement_by;
        duration = offset_duration;

        this.time_remaining = 1;
    }


    public override void ApplyEffect()
    {
        base.ApplyEffect();

        Debug.Log("Applying " + receiver.u_name + " for movement " + movement_offset);
        receiver.AdjustMovement(movement_offset);
        BattleManager.battle_manager.SetMovableTilesOfUnit(receiver);
        receiver.HighlightHexesWeCanMoveTo();
    }


    public override Effect Clone(Unit receiving_unit)
    {
        return new ChangeMovement(receiving_unit, movement_offset, duration);
    }
}
