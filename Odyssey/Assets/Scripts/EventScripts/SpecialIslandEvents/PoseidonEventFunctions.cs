using UnityEngine;
using System.Collections;

public class PoseidonEventFunctions : IslandEventFunctions {

	// Use this for initialization
	void Start () {
		eventHandler = GameObject.FindGameObjectWithTag("EventController").GetComponent<EventManagement>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void investigateTemple()
	{
	
	}
	
	public void pray()
	{
	}
	
	public void Donate()
	{
	
	}
	
	public void RaidTemple()
	{
		
	}
	
}
