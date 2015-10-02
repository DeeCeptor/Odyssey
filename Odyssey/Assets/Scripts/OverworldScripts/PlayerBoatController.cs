using UnityEngine;
using System.Collections;

public class PlayerBoatController : MonoBehaviour {
	public float speed = 1f;
	public float fastSpeed = 1.5f;
	public float slowSpeed = 0.5f;
	public float turnSpeed = 0.2f;
	private int moveRate = 1;
	public float encounterRange = 5;
	public ResourceManager resource;
	public GameObject islandParkedAt;

	
	public bool paused = false;
	
	// Use this for initialization
	void Start () {
	resource = GetComponent<ResourceManager>();
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
	    if(!paused)
	    {
	
	    	if(moveRate == 0)
	    	{
	    		gameObject.GetComponent<Rigidbody>().velocity = Input.GetAxis("Vertical")*slowSpeed*transform.forward*((resource.stamina + 50f)/100f)*(1+(resource.poseidonsFavour/500f));
	    		transform.Rotate(Input.GetAxis("Horizontal")*turnSpeed*transform.up);
	    	}
	
	    	if(moveRate == 1)
	    	{
	    		gameObject.GetComponent<Rigidbody>().velocity = Input.GetAxis("Vertical")*speed*transform.forward*((resource.stamina + 50f)/100f)*(1+(resource.poseidonsFavour/500f));
	    		transform.Rotate(Input.GetAxis("Horizontal")*turnSpeed*transform.up);
	    	}
	
	    	if(moveRate == 2)
	    	{
		    	gameObject.GetComponent<Rigidbody>().velocity = Input.GetAxis("Vertical")*fastSpeed*transform.forward*((resource.stamina + 50f)/100f)*(1+(resource.poseidonsFavour/500f));
		    	transform.Rotate(Input.GetAxis("Horizontal")*turnSpeed*transform.up);
		    }

          if(resource.currentWeather.Length > 0)
          {
                 for (int i = 0; i< resource.currentWeather.Length; i++)
                 {
                    gameObject.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity * resource.currentWeather[i].gameObject.GetComponent<WeatherScript>().weatherSpeedDown;
                    gameObject.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity + resource.currentWeather[i].gameObject.GetComponent<WeatherScript>().weatherDirection;
                }
         }
	}
	}
	
	public void SlowSpeed()
	{
		moveRate = 0;
	}
	
	public void MediumSpeed()
	{
		if(resource.stamina>0)
		{
		moveRate = 1;
		}
	}
	
	public void HighSpeed()
	{
		if(resource.stamina>0)
		{
			moveRate = 2;
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
	
	//used for anchoring
	public void togglePause()
	{
		if(paused)
		{
			paused = false;
		}
		
		else if(!paused)
		{
			paused = true;
			Collider[] objectsNear = Physics.OverlapSphere(transform.position, encounterRange);
			for(int i = 0; i< objectsNear.Length;i++)
			{
				if (objectsNear[i].gameObject.tag.Equals("Island"))
				{
				islandParkedAt = objectsNear[i].gameObject;
				islandParkedAt.GetComponent<IslandEventScript>().HaveEvent();
				}
			
			}
		}
	}
}
