using UnityEngine;
using System.Collections;

public class EventManagement : MonoBehaviour {

	public GameObject[] SeaEventList;
	
	public int framesPerCheckRegular = 240;
	public int framesPerCheck = 240;
	private int frameIterator = 0;
	public float eventChance = 25f;
	public bool paused = false;
	// Use this for initialization
	void Start () {
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
		Instantiate(SeaEventList[0]);
	}

	public void Pause()
	{
		paused = true;
	}
	
	public void Unpause()
	{
		paused = false;
	}
	
}
