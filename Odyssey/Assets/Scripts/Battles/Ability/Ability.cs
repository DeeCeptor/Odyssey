using UnityEngine;
using System.Collections;

// An ability belong to a unit, and must be CAST/ACTIVATED by the user.
// Either changes the stats of the unit/units or targets a space containing a unit.
public class Ability
{
    public bool can_cast = true;
    public bool cast_once = true;   // If true, can_cast is set to false after this ability has been cast
    public bool takes_entire_turn = false;  // If true, sets the unit to inactive after casting.
    public int cost;    // How much god favour it costs to use this ability
    public Unit caster; // Unit whose ability menu we're using to cast this ability
    public string ability_name; // Human readable name that shows when the user mouses over
    public string ability_description;  // Human readable description that shows when the user mouses over
    public bool targetable = false;     // Whether this ability needs a target or not. No target means the effect goes off immediately after clicking the ability button.

    public bool effects_self = true;    // If set to true, then this ability will simply apply effect_of_ability to the caster of this ability.
    public Effect effect_of_ability;    // Many abilities simply add effects onto the current unit, or an AOE buff. Place those effects here.

    
    // Extend the constructor and a abilitys_effect if this ability simply adds an effect.
    public Ability(string name, string description, Unit owner, int favour_cost)
    {
        ability_name = name;
        ability_description = description;
        caster = owner;
        cost = favour_cost;
    }


    public bool CanCastAbility()
    {
        return (can_cast
            && caster.active
            && (GodsManager.gods_manager.favour_remaining >= cost)
            );
    }
    public void TryToCastAbility()
    {
        if (CanCastAbility())
        {
            Debug.Log("Casting " + ability_name + " for " + cost);

            // Disable ability if it can only be cast once
            if (cast_once)
                can_cast = false;

            // Adjust cost
            GodsManager.gods_manager.ModifyFavour(-cost);

            CastAbility();
        }
        
    }


    // Ability has actually been cast. Override this method to implement more complex abilities.
    public virtual void CastAbility()
    {
        // Simplest ability. Just adds an effect to change the untis stats
        if (effects_self)
            caster.AddEffectToUnit(effect_of_ability.Clone(caster));
    }
}
