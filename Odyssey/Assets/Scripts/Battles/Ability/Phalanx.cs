using UnityEngine;
using System.Collections;

public class Phalanx : Ability
{
    public static float defence_bonus = 0.10f;

    public Phalanx(Unit owner)
        : base("Phalanx", "Nearby squads form up with one, granting all friendly units with 1 hex +" + defence_bonus + "% defence. Takes the entire turn.", owner, 1)
    {
        effect_of_ability = new ChangeMovement(owner, 3, 1);
        effects_self = false;
        takes_entire_turn = true;
    }


    public override void CastAbility()
    {
        base.CastAbility();

        foreach (Edge edge in caster.location.neighbours)
        {
            if (edge.destination.occupying_unit != null
                && edge.destination.occupying_unit.owner.IsAlly(caster))
            {
                edge.destination.occupying_unit.AddEffectToUnit(new ChangeDefence(edge.destination.occupying_unit, 0.1f, 1));
            }
        }
    }
}
