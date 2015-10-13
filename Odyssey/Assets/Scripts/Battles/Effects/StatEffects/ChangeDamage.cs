using UnityEngine;
using System.Collections;


public class ChangeDamage : Effect
{
    int duration;

    public ChangeDamage(Unit receiver, float constant_adjust_by, float percentage_adjust_by, int offset_duration)
        : base("Altered Damage", "Damage changed", receiver)
    {
        offset = constant_adjust_by;
        percentage_offset = percentage_adjust_by;
        duration = offset_duration;

        this.time_remaining = 1;
    }


    public override void ApplyEffect()
    {
        Debug.Log("Applying " + receiver.u_name + " for damage");
        receiver.AdjustDamage(offset, percentage_offset);

        base.ApplyEffect();
    }


    public override Effect Clone(Unit receiving_unit)
    {
        return new ChangeDamage(receiving_unit, offset, percentage_offset, duration);
    }
}
