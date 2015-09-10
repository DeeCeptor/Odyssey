using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{
    public Hex location;       // Where this unit currently is
    public List<Hex> movement_path = new List<Hex>();      // When ordered to move, this path is populated with the hexes this unit will traverse

	void Start ()
    {
	
	}
	
	void Update ()
    {
        // Check if we should be moving
	    if (movement_path != null && movement_path.Count > 0)
        {
            transform.LookAt(movement_path[0].transform.position);
            transform.position = Vector3.MoveTowards(transform.position, movement_path[0].transform.position, Time.deltaTime * 2.0f);

            if (transform.position == movement_path[0].transform.position)
            {
                if (movement_path.Count == 1)
                    location = movement_path[0];

                movement_path.RemoveAt(0);
            }
        }
	}


    void OnMouseDown()
    {
        PlayerInterface.player_interface.UnitSelected(this);
    }
}
