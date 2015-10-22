using UnityEngine;
using System.Collections;


public class ChangeAttackRange : Effect
{
    int duration;

    public ChangeAttackRange(Unit receiver, int adjust_by, int offset_duration)
        : base("Altered Attack Range", "Attack range changed by " + adjust_by, receiver)
    {
        offset = adjust_by;
        duration = offset_duration;

        this.time_remaining = 1;
    }


    public override void ApplyEffect()
    {
        Debug.Log("Applying " + receiver.u_name + " for attack range " + offset);
        receiver.AdjustRange((int) offset);

        base.ApplyEffect();
    }


    public override Effect Clone(Unit receiving_unit)
    {
        return new ChangeAttackRange(receiving_unit, (int) offset, duration);
    }
}
