using UnityEngine;
using System.Collections;

public class ChangeRangedDefence : Effect
{
    int duration;

    public ChangeRangedDefence(Unit receiver, float adjust_by, int offset_duration)
        : base("Altered Ranged Defence", "Ragned defence changed by " + adjust_by, receiver)
    {
        offset = adjust_by;
        duration = offset_duration;

        this.time_remaining = 1;
    }


    public override void ApplyEffect()
    {
        Debug.Log("Applying " + receiver.u_name + " for ranged  defence " + offset);
        receiver.AdjustRangedDefence(offset);

        base.ApplyEffect();
    }


    public override Effect Clone(Unit receiving_unit)
    {
        return new ChangeRangedDefence(receiving_unit, offset, duration);
    }
}
