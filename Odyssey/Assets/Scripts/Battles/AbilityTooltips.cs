using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AbilityTooltips : MonoBehaviour 
{
	public static AbilityTooltips ability_tooltips;
	public GameObject tooltip_window;

	void Awake()
	{
		ability_tooltips = this;
	}

	public void ShowAbilityTooltip(string ability_text)
	{
		tooltip_window.SetActive(true);

		tooltip_window.GetComponentInChildren<Text>().text = ability_text;
	}

	public void HideAbilityTooltip()
	{
		tooltip_window.SetActive(false);
	}
}
