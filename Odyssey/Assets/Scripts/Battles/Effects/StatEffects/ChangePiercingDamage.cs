using UnityEngine;
using System.Collections;


public class ChangePiercingDamage : Effect
{
    int duration;

    public ChangePiercingDamage(Unit receiver, float constant_adjust_by, float percentage_adjust_by, int offset_duration)
        : base("Altered Piercing Damage", "Piercing Damage changed", receiver)
    {
        offset = constant_adjust_by;
        percentage_offset = percentage_adjust_by;
        duration = offset_duration;

        this.time_remaining = 1;
    }


    public override void ApplyEffect()
    {
        Debug.Log("Applying " + receiver.u_name + " for piercing damage");
        receiver.AdjustPiercingDamage(offset, percentage_offset);

        base.ApplyEffect();
    }


    public override Effect Clone(Unit receiving_unit)
    {
        return new ChangePiercingDamage(receiving_unit, offset, percentage_offset, duration);
    }
}
