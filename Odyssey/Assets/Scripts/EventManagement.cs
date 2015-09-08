using UnityEngine;
using System.Collections;

public class EventManagement : MonoBehaviour {
	
	public GameObject player;
	public ResourceManager resourceController;
	public PlayerBoatController playerController;
	public GameObject currentEvent;

	public GameObject[] SeaEventList;
	
	public int framesPerCheckRegular = 240;
	public int framesPerCheck = 240;
	private int frameIterator = 0;
	public float eventChance = 25f;
	public bool paused = false;
	
	// Use this for initialization
	void Start () {
	player = GameObject.FindGameObjectWithTag("Player");
	resourceController = player.GetComponent<ResourceManager>();
	playerController = player.GetComponent<PlayerBoatController>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(paused == false)
			{
			if(frameIterator>framesPerCheck)
				{
				frameIterator = 0;
				framesPerCheck = framesPerCheckRegular + Random.Range(0,121);
				if(Random.Range (0,100) <eventChance)
				{
				HaveEvent();
				}
				
				}
			frameIterator = frameIterator + 1;
			}
	}
	
	public void HaveEvent()
	{
		currentEvent = Instantiate(SeaEventList[0]);
		Pause();
	}
	
	public void EndEvent()
	{
	Unpause();
	Destroy(currentEvent);
	}

	public void Pause()
	{
		paused = true;
		resourceController.Pause();
		playerController.Pause();
	}
	
	public void Unpause()
	{
		paused = false;
		resourceController.Unpause();
		playerController.Unpause();
	}
	
}
