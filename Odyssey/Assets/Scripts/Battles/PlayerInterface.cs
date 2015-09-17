using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerInterface : MonoBehaviour 
{
    public static PlayerInterface player_interface;

    public Text turn_text;

    public Text unit_name;
    public Text unit_description;

    public Text terrain_name;
    public Text terrain_description;


    [HideInInspector]
    public Unit selected_unit;
    [HideInInspector]
    public Hex highlighted_hex;

	void Start () 
	{
        player_interface = this;
	}
	

	void Update () 
	{
	
	}


    // Player left clicked on the unit
    public void UnitSelected(Unit unit)
    {
        // Set the unit title, description and stats
        unit_name.text = unit.u_name;
        unit_description.text = "<i>" + unit.u_description + "</i>";

        if (unit.owner == BattleManager.battle_manager.current_player)
        {
            selected_unit = unit;
        }

        UnhighlightHexes();

        unit.HighlightHexesWeCanMoveTo();
    }
    // Dehighlights any hexes that may have been highlighted
    public void UnitDeselected()
    {
        UnhighlightHexes();

        selected_unit = null;
    }


    public bool SelectedUnitAvailableToControl()
    {
        return (selected_unit != null
            && selected_unit.IsControllable());
    }


    public void UnhighlightHexes()
    {
        foreach (Hex hex in HexMap.hex_map.all_hexes)
        {
            hex.UnhighlightHex();
        }
    }


    public void MousedOverHex(Hex hex)
    {
        if (highlighted_hex != hex && !hex.IsHighlighted())
        {
            if (highlighted_hex != null)
                highlighted_hex.UnMouseHighlight();
            highlighted_hex = hex;
            highlighted_hex.MouseHighlight();
        }

        // Set the hex title, description
        terrain_name.text = hex.h_name;
        terrain_description.text = "<i>" + hex.h_description + hex.coordinate + "</i>";
    }


    public void TeleportUnit(Hex to)
    {
        if (selected_unit.location == null)
        {
            HexMap.hex_map.WarpUnitTo(selected_unit, to);
        }
        else if (selected_unit != null)
        {

        }
    }
}
