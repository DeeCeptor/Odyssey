using UnityEngine;
using System.Collections;

public class IslandEventFunctions : MonoBehaviour {

	public GameObject eventToCall;
	public EventManagement eventHandler;
	// Use this for initialization
	void Start () {
	eventHandler = GameObject.FindGameObjectWithTag("EventController").GetComponent<EventManagement>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Explore()
	{
	
	}
	
	public void Upgrade()
	{
		
	}
	
	public void Gather()
	{
	eventHandler.resourceController.GatherToggle(eventHandler.islandEventIsOn);
	eventHandler.EndEvent();
	}
	
	public void End()
	{
		eventHandler.EndEvent();
	}
}
