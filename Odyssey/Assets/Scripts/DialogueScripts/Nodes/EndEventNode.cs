using UnityEngine;
using System.Collections;

public class EndEventNode : Node {
	public EventManagement eventController;
	
	// Use this for initialization
	void Start () {
	eventController = GameObject.FindGameObjectWithTag("EventController").GetComponent<EventManagement>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void Run_Node()
	{
		eventController.EndEvent();
	}
}
