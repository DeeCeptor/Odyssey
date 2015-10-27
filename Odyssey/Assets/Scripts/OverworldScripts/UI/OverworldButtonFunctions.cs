using UnityEngine;
using System.Collections;

public class OverworldButtonFunctions : MonoBehaviour {
	public GameObject player;
	public ResourceManager resources;
	public PlayerBoatController playerControl;
	//need these functions so that the player doesn't need to be attached to the overworld UI
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		resources = player.GetComponent<ResourceManager>();
		playerControl = player.GetComponent<PlayerBoatController>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	if(player==null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            resources = player.GetComponent<ResourceManager>();
            playerControl = player.GetComponent<PlayerBoatController>();
        }
	}
	
	public void LowRations()
	{
		resources.LowRations();
	}
	
	public void HighRations()
	{
		resources.HighRations();
	}
	
	public void NormalRations()
	{
		resources.NormalRations();
	}
	
	public void SlowSpeed()
	{
		resources.SlowSpeed();
		playerControl.SlowSpeed();
	}
	
	public void MediumSpeed()
	{
		resources.MediumSpeed();
		playerControl.MediumSpeed();
	}
	
	public void FastSpeed()
	{
		resources.HighSpeed();
		playerControl.HighSpeed();
	}
	
	public void Anchor()
	{
	playerControl.toggleAnchor();
	resources.toggleAnchor();
	}
}
