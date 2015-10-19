using UnityEngine;
using System.Collections;
public class PoseidonIslandEventScript : IslandEventScript {

	public bool templeDestroyed = false;
	public GameObject destroyedEvent;
	
	void OnTriggerEnter(Collider other)
	{
	if(!templeDestroyed)
	{
		eventHandler.HaveEvent(eventToCall);
	}
	else if(templeDestroyed)
	{
			eventHandler.HaveEvent(destroyedEvent);
	}
	}
}
