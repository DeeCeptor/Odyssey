using UnityEngine;
using System.Collections.Generic;

// Script that is not destroyed between levels.
// Records the positions of the units the player deployed in the pre tactical battle deployment level
public class PlayerUnitPositions : MonoBehaviour
{
    public List<Unit> player_deployed_units = new List<Unit>();    // Units set in their positions by the player
    public List<Faction> saved_factions = new List<Faction>();

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start ()
    {
	
	}
}
