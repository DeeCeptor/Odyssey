using UnityEngine;
using System.Collections;


public class ChangeCounterableAttacks : Effect
{
    int duration;

    public ChangeCounterableAttacks(Unit receiver, int offset_duration)
        : base("Attacks become uncounterable", "Attacks become uncounterable for " + offset_duration + " turn(s)", receiver)
    {
        duration = offset_duration;

        this.time_remaining = 1;
    }


    public override void ApplyEffect()
    {
        Debug.Log("adding duellist");
        receiver.attacks_are_counterable = false;

        base.ApplyEffect();
    }


    public override void RemoveThisEffect()
    {
        base.RemoveThisEffect();

        Debug.Log("removing duellist");
        receiver.attacks_are_counterable = true;
    }


    public override Effect Clone(Unit receiving_unit)
    {
        return new ChangeCounterableAttacks(receiving_unit, duration);
    }
}
