using UnityEngine;
using System.Collections;
[System.Serializable]
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
    public PlayerBoatController playerStats;
    public float dieDistance = 150;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		eventHandler = GameObject.FindGameObjectWithTag("EventController").GetComponent<EventManagement>();
        playerStats = player.GetComponent<PlayerBoatController>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	if(!paused)
	{
        if ((transform.position - player.transform.position).magnitude < playerStats.visionRange)
        {
             GetComponent<SpriteRenderer>().enabled = true;
        }

        else if((transform.position - player.transform.position).magnitude > dieDistance)
        {
            Destroy(gameObject);
        }

        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }

        if ((transform.position - player.transform.position).magnitude < sightLine)
		{
                Vector3 diff = player.transform.position - transform.position;
                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

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
				transform.Rotate((randomDegreesToTurn/randomMovingChange)*transform.forward);
			}
			if(direction == 3)
			{
				transform.Rotate((-randomDegreesToTurn/randomMovingChange)*transform.forward);
			}
			randomMovingCounter = randomMovingCounter + 1;
		}
		
		gameObject.GetComponent<Rigidbody2D>().velocity = speed*transform.up;
	}
	}
	
	public void Pause()
	{
		paused = true;
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
	}
	
	public void Unpause()
	{
		paused = false;
	}
	
	void OnCollisionEnter2D(Collision2D collide)
	{
	if(collide.gameObject.tag.Equals("Player"))
	{
	eventHandler.HaveEvent(collisionEvent);
	Destroy (gameObject);
	}
	}
}
