using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ActorManager : MonoBehaviour 
{
	public enum Actor_Positions { LEFT, MIDDLE, RIGHT };


	public static ActorManager actor_manager;

	private static List<Actor> actors_on_scene = new List<Actor>();		// List of all instantiated actors

	[HideInInspector]
	public static Actor left_actor;
	[HideInInspector]
	public static Actor middle_actor;
	[HideInInspector]
	public static Actor right_actor;


	public static bool Is_Actor_On_Scene(string actor_name)
	{
		// Search for our actor on the scene
		Actor item = actors_on_scene.Find(obj => obj.name==actor_name);
		return (item != null);
			/*
		// Check all positions to see if the actor is present
		if (Name_Match(actor_name, left_actor))
			return true;
		else if (Name_Match(actor_name, middle_actor))
			return true;
		else if (Name_Match(actor_name, right_actor))
			return true;

		return false;*/
	}


	// Finds the actor if it on the scene.
	// Returns null if the actor is not present
	public static Actor Get_Actor(string actor_name)
	{
		// Search for our actor on the scene
		Actor item = actors_on_scene.Find(obj => obj.name==actor_name);
		return item;
		/*
		if (Is_Actor_On_Scene(actor_name))
		{
			if (Name_Match(actor_name, left_actor))
				return left_actor;
			else if (Name_Match(actor_name, middle_actor))
				return middle_actor;
			else if (Name_Match(actor_name, right_actor))
				return right_actor;
			else
				return null;
		}
		else
			return null;*/
	}


	private static bool Name_Match(string actor_name, Actor current_actor)
	{
		return (current_actor != null && actor_name == current_actor.name);
	}


	// Returns the RectTransform positions of an actor position
	public static RectTransform Get_Position(Actor_Positions position)
	{
		Debug.Log(Enum.GetName(typeof(Actor_Positions), position));
		return GameObject.Find(VNProperties.canvas_name + "/ActorPositions/" + Enum.GetName(typeof(Actor_Positions), position)).GetComponent<RectTransform>();
	}


	// Instantiates an actor from the Resources/Actors folder with the name actor_name.
	// It then sets the object as a child of the Actors object in the canvas
	public static Actor Instantiate_Actor(string actor_name, ActorManager.Actor_Positions destination)
	{
		GameObject actor = Instantiate(Resources.Load("Actors/" + actor_name, typeof(GameObject))) as GameObject;
		Actor actor_script = actor.GetComponent<Actor>();
		actor.transform.parent = GameObject.Find(VNProperties.canvas_name + "/Actors").transform;
		actor.transform.localScale = Vector3.one;
		actors_on_scene.Add(actor_script);	// Add to list of actors

		return actor_script;
	}


	// Called by dialogue node to darken all actors but the one that's talking
	public static void Darken_All_Actors_But(Actor speaking_actor)
	{
		foreach (Actor actor in actors_on_scene)
		{
			// Don't darken this actor
			if (actor.name != speaking_actor.name)
			{
				actor.Darken();
			}
		}
	}


	void Start () 
	{
		actor_manager = this;
	}
	
	void Update () 
	{
	
	}
}
