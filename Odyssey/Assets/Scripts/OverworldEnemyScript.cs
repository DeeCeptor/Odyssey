using UnityEngine;
using System.Collections;

public class OverworldEnemyScript : MonoBehaviour {
	public GameObject collisionEvent;
	public float speed = 1f;
	public float sightLine = 50;
	public GameObject player;
	float randomMovingCounter = 0;
	//how many frames before changing direction
	float randomMovingChange = 240;
	public float randomDegreesToTurn = 0;
	//0 for turning left 1 or 2 for straight 3 for turning right
	public int direction = 1;
	public bool paused = false;
	public EventManagement eventHandler;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		eventHandler = GameObject.FindGameObjectWithTag("EventController").GetComponent<EventManagement>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	if(!paused)
	{
		if((transform.position - player.transform.position).magnitude < sightLine)
		{
			transform.LookAt(player.transform.position,transform.up);
			
		}
		
		else
		{
			if(randomMovingCounter>randomMovingChange)
			{
				randomMovingCounter = 0;
				direction = Random.Range (0,4);
				randomDegreesToTurn = Random.Range (0,181);
			}
			if(direction == 0)
			{
				transform.Rotate((randomDegreesToTurn/randomMovingChange)*transform.up);
			}
			if(direction == 3)
			{
				transform.Rotate((-randomDegreesToTurn/randomMovingChange)*transform.up);
			}
			randomMovingCounter = randomMovingCounter + 1;
		}
		
		gameObject.GetComponent<Rigidbody>().velocity = speed*transform.forward;
	}
	}
	
	public void Pause()
	{
		paused = true;
		gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
	}
	
	public void Unpause()
	{
		paused = false;
	}
	
	void OnCollisionEnter(Collision collide)
	{
	if(collide.gameObject.tag.Equals("Player"))
	{
	eventHandler.HaveEvent(collisionEvent);
	Destroy (gameObject);
	}
	}
}
