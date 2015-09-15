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

    public Unit selected_unit;


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
    }


    public void MousedOverHex(Hex hex)
    {
        // Set the hex title, description
        terrain_name.text = hex.h_name;
        terrain_description.text = "<i>" + hex.h_description + "</i>";
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
