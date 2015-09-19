﻿using UnityEngine;
using System.Collections;

// An affect that 
public class Effect 
{
    public string readable_name;    // Name displayed on the unit when this effect is active
    public string readable_description;
    protected Unit receiver;
    protected int time_remaining = -9;     // Set to -1 if the effect is not timed. Else it goes away after timed number of turns.
    protected bool terrain_effect = false;

    // Ability name and the unit it's affecting
    public Effect(string effect_name, string description, Unit receiver)
    {
        readable_name = effect_name;
        readable_description = description;
        this.receiver = receiver;
    }


    // Called to apply the effect to the unit. Often changes the stats of the unit. OVerride this when creating a new effect.
    public virtual void ApplyEffect()
    {

    }


    // Return true if we should remove this effect from the unit
    public bool TurnStart()
    {
        // Only do timed effects if this is a timed effect
        if (time_remaining != -9)
        {
            time_remaining--;

            if (time_remaining <= 0)
            {
                // Effect has worn off
                RemoveThisEffect();
                return true;
            }
        }

        ApplyEffect();
        return false;
    }


    // Return true if we should remove this effect from the unit
    public bool UnitMoved()
    {
        // If this is a terrain effect, remove this effect because we've moved and it no longer applies
        if (terrain_effect)
        {
            RemoveThisEffect();
            return true;
        }
        return false;
    }


    public void RemoveThisEffect()
    {

    }


    // Clone returns a new copy of this effect.
    // Every effect must override this.
    public virtual Effect Clone(Unit receiving_unit)
    {
        return null;
    }
}