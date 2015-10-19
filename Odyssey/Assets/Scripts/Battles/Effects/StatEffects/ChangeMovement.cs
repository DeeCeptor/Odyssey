using UnityEngine;
using System.Collections;


public class ChangeMovement : Effect
{
    int duration;

    public ChangeMovement(Unit receiver, int adjust_movement_by, int offset_duration)
        : base("Altered Movement", "Movement speed changed by " + adjust_movement_by, receiver)
    {
        offset = adjust_movement_by;
        duration = offset_duration;

        this.time_remaining = 1;
    }


    public override void ApplyEffect()
    {
        Debug.Log("Applying " + receiver.u_name + " for movement " + offset);
        receiver.AdjustMovement((int) offset);
        BattleManager.battle_manager.SetMovableTilesOfUnit(receiver);
        receiver.HighlightHexesWeCanMoveTo();

        base.ApplyEffect();
    }


    public override Effect Clone(Unit receiving_unit)
    {
        return new ChangeMovement(receiving_unit, (int) offset, duration);
    }
}
