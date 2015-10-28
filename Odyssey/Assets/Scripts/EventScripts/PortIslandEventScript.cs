using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PortIslandEventScript : IslandEventScript {
    public Dictionary<string, int> units;
    public Dictionary<string, int> maxes;
    public int maxUnits = 10;
    public int maxAdjust = 5;
    public int numberToAdd = 1;
    public int rand;
    public int resupplyTime = 800;
    private int resupplyTimer = 0;

	// Use this for initialization
	void Start () {
        eventHandler = GameObject.FindGameObjectWithTag("EventController").GetComponent<EventManagement>();
        units = new Dictionary<string, int>();
        maxes = new Dictionary<string, int>();
        populatePort();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    if(!eventHandler.paused)
        {
            if(resupplyTimer > resupplyTime)
            {
                foreach (KeyValuePair<string, int> unitNum in units) if (units[unitNum.Key] < maxes[unitNum.Key])  units[unitNum.Key] = unitNum.Value + numberToAdd;
            }
        }
	}

    public void populatePort()
    {
        if(Random.Range(0,2) == 0 && !units.ContainsKey("Hoplite"))
        {
            units.Add("Hoplite", maxUnits - Random.Range(0,maxAdjust));
            maxes.Add("Hoplite", maxUnits + Random.Range(0, maxAdjust));
        }

        if (Random.Range(0, 2) == 0 && !units.ContainsKey("Swordsman"))
        {
            units.Add("Swordsman", maxUnits - Random.Range(0, maxAdjust));
            maxes.Add("Swordsman", maxUnits + Random.Range(0, maxAdjust));
        }

        if (Random.Range(0, 2) == 0 && !units.ContainsKey("Cavalry"))
        {
            units.Add("Cavalry", maxUnits - Random.Range(0, maxAdjust));
            maxes.Add("Cavalry", maxUnits + Random.Range(0, maxAdjust));
        }

        if (Random.Range(0, 2) == 0 && !units.ContainsKey("Slinger"))
        {
            units.Add("Slinger", maxUnits - Random.Range(0, maxAdjust));
            maxes.Add("Slinger", maxUnits + Random.Range(0, maxAdjust));
        }

        if (Random.Range(0, 2) == 0 && !units.ContainsKey("Archer"))
        {
            units.Add("Archer", maxUnits - Random.Range(0, maxAdjust));
            maxes.Add("Archer", maxUnits + Random.Range(0, maxAdjust));
        }

        if (Random.Range(0, 2) == 0 && !units.ContainsKey("Peltast"))
        {
            units.Add("Peltast", maxUnits - Random.Range(0, maxAdjust));
            maxes.Add("Peltast", maxUnits + Random.Range(0, maxAdjust));
        }

        if(units.Count < 3)
        {
            populatePort();
        }
        Debug.Log(units.Count);
    }
}
