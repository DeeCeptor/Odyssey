using UnityEngine;
using System.Collections;

// Terrain effect gotten from standing in a hills space
// Gives +1 attack range and bonus to ranged damage
public class ChangeDefence : Effect
{
    float offset;
    int duration;

    public ChangeDefence(Unit receiver, float adjust_by, int offset_duration)
        : base("Altered Defence", "Defence changed by " + adjust_by, receiver)
    {
        this.terrain_effect = true;
        offset = adjust_by;
        duration = offset_duration;

        this.time_remaining = 1;
    }


    public override void ApplyEffect()
    {
        base.ApplyEffect();

        Debug.Log("Applying " + receiver.u_name + " for defence " + offset);
        receiver.AdjustDefence(offset);
        PlayerInterface.player_interface.RefreshStatsPanel(receiver);
    }


    public override Effect Clone(Unit receiving_unit)
    {
        return new ChangeDefence(receiving_unit, offset, duration);
    }
}
