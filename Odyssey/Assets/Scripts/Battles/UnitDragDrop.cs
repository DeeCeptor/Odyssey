using UnityEngine;
using System.Collections;

public class UnitDragDrop : MonoBehaviour
{
    Unit unit;


    void Start () {
        unit = this.gameObject.GetComponent<Unit>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void OnMouseDrag()
    {
        // Move the unit so the currently highlighted hex if we can
        if (PlayerInterface.player_interface.highlighted_hex != null
            && PlayerInterface.player_interface.highlighted_hex != unit.location 
            && PlayerInterface.player_interface.highlighted_hex.deployment_zone
            && PlayerInterface.player_interface.highlighted_hex.occupying_unit == null
            && !PlayerInterface.player_interface.highlighted_hex.impassable)
        {
            HexMap.hex_map.WarpUnitTo(unit, PlayerInterface.player_interface.highlighted_hex);
        }
    }
}
