using UnityEngine;
using System.Collections;
public class IslandEventScript : MonoBehaviour {

public bool explored = false;
public GameObject eventToCall;
public GameObject eventToCallExplored;
public EventManagement eventHandler;

	// Use this for initialization
	void Start () {
		eventHandler = GameObject.FindGameObjectWithTag("EventController").GetComponent<EventManagement>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void HaveEvent()
	{
		eventHandler.HaveIslandEvent(eventToCall,gameObject);
	}
}
