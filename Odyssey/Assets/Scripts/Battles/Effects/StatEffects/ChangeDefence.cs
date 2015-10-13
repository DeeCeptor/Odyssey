using UnityEngine;
using System.Collections;


public class ChangeDefence : Effect
{
    int duration;

    public ChangeDefence(Unit receiver, float adjust_by, int offset_duration)
        : base("Altered Defence", "Defence changed by " + adjust_by, receiver)
    {
        offset = adjust_by;
        duration = offset_duration;

        this.time_remaining = 1;
    }


    public override void ApplyEffect()
    {
        Debug.Log("Applying " + receiver.u_name + " for defence " + offset);
        receiver.AdjustDefence(offset);

        base.ApplyEffect();
    }


    public override Effect Clone(Unit receiving_unit)
    {
        return new ChangeDefence(receiving_unit, offset, duration);
    }
}
