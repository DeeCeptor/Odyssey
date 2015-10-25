using UnityEngine;
using System.Collections;

public class Phalanx : Ability
{
    public static float defence_bonus = 0.10f;

    public Phalanx(Unit owner)
        : base("Phalanx", "Nearby squads form up as one, granting all adjacent friendly units +" + defence_bonus + "% defence.", owner, 1, false)
    {

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
                edge.destination.occupying_unit.AddEffectToUnit(new ChangeRangedDefence(edge.destination.occupying_unit, 0.1f, 1));
            }
        }
    }
}
