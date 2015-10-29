using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutomatonIslandScript : PortIslandEventScript {

	// Use this for initialization
	void Start () {
        eventHandler = GameObject.FindGameObjectWithTag("EventController").GetComponent<EventManagement>();
        units = new Dictionary<string, int>();
        maxes = new Dictionary<string, int>();
        PopulatePort();
    }
	

    public void PopulatePort()
    {
        units.Add("Automata", maxUnits - Random.Range(0, maxAdjust));
        maxes.Add("Automata", maxUnits + Random.Range(0, maxAdjust));
    }
}
