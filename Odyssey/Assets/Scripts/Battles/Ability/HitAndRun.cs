using UnityEngine;
using System.Collections;

public class HitAndRun : Ability
{
    public HitAndRun(Unit owner)
        : base("Hit & Run", "Allows the unit to move once again after attacking. Lasts 1 turn.", owner, 1, true)
    {
        cast_after_attack = true;
        effects_self = false;
    }

    public override void CastAbility()
    {
        base.CastAbility();

        caster.active = true;
        caster.has_moved = false;

		BattleManager.battle_manager.SetMovableTilesOfUnit(caster);
		caster.HighlightHexesWeCanMoveTo(true);
    }
}
