using UnityEngine;
using System.Collections;

public class PlayerInterface : MonoBehaviour 
{
    public static PlayerInterface player_interface;

    Unit selected_unit;


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
        selected_unit = unit;
    }


    public void TeleportUnit(Hex to)
    {
        if (selected_unit.location == null)
        {
            HexMap.hex_map.WarpUnitTo(selected_unit, to);
        }
        else if (selected_unit != null)
        {
            // Pathfind to the correct spot
            if (selected_unit.movement_path.Count == 0)
                selected_unit.movement_path = HexMap.hex_map.AStarFindPath(selected_unit.location, to, 5);
        }
    }
}
