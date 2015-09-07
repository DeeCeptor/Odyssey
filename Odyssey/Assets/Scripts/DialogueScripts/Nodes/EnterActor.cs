using UnityEngine;
using System.Collections;

public class EnterActor : Node 
{
	public string actor_name;
	public bool fade_in;
	public bool slide_in;
	public ActorManager.Actor_Positions destination;	// Which space the will occupy

	public override void Run_Node()
	{
		Actor actor_script;

		// Check if the actor is already present
		if (ActorManager.Is_Actor_On_Scene(actor_name))
		{
			// Actor is already on the scene
			actor_script = ActorManager.Get_Actor(actor_name).GetComponent<Actor>();
		}
		else
		{
			// Actor is not in the scene. Instantiate it
			actor_script= ActorManager.Instantiate_Actor(actor_name, destination);
		}

		if (slide_in)
		{
			// Have it slide in
			actor_script.Slide_In(2.0f);
		}
		else
		{
			// Not sliding in, simply place it at the correct position
			actor_script.Place_At_Position(destination);
		}

		if (fade_in)
		{
			// Have this actor fade in
			actor_script.Fade_In(2.0f);
		}

		Finish_Node();
	}
	
	
	public override void Button_Pressed()
	{
		Finish_Node();
	}
	
	
	public override void Finish_Node()
	{
		StopAllCoroutines();
		
		base.Finish_Node();
	}
}
