using UnityEngine;
using System.Collections;

public class AbilityButton : MonoBehaviour 
{
	public Ability ability;

	public void MousedOver()
	{
		AbilityTooltips.ability_tooltips.ShowAbilityTooltip(
			ability.ability_name + ": " + ability.cost + " Favour"
			+ "\n" + ability.ability_description);
	}

	public void MouseLeft()
	{
		AbilityTooltips.ability_tooltips.HideAbilityTooltip();
	}
}
