using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBoatController : MonoBehaviour {
	public float speed = 1f;
	public float fastSpeed = 1.5f;
	public float slowSpeed = 0.5f;
	public float turnSpeed = 0.2f;
	private int moveRate = 1;
    public float minDistToPoint;
	public float encounterRange = 15;
	public ResourceManager resource;
	public GameObject islandParkedAt;
    public bool anchored;
    // vertices in a line the boat must follow
    public Queue<Vector3> vertices;
    public int vertexIndex = 0;
    //distance to vertex for it to be counted as explored
    public float vertexDist = 0.1f;
    public Vector2 dir;
   

	
	public bool paused = false;
	
	// Use this for initialization
	void Start () {
	    resource = GetComponent<ResourceManager>();
        vertices = new Queue<Vector3>();
    }
	
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Mouse0))
      //  {
      //      numberOfPoints = 0;
      //      lineRender.SetVertexCount(0);
     //       EventManagement.gameController.Pause();
  //      }
   //     else if (Input.GetKey(KeyCode.Mouse0))
  //      {
  //          numberOfPoints++;
 //           lineRender.SetVertexCount(numberOfPoints);
            
    //        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
 //           worldPos.z = 10;
  //          lineRender.SetPosition(numberOfPoints - 1, worldPos);
 //       }
 //       else
  //      {
  //          EventManagement.gameController.Unpause();
  //      }
    }
	// Update is called once per frame
	void FixedUpdate () {
	    if(!paused && !anchored)
	    {
            if (0 < vertices.Count)
            {
                dir = (vertices.Peek() - transform.position);
                if (moveRate == 0)
                {
                    gameObject.GetComponent<Rigidbody2D>().velocity = dir.normalized*slowSpeed * ((resource.stamina + 50f) / 100f) * (1 + (resource.poseidonsFavour / 500f));
                }

                if (moveRate == 1)
                {
                    gameObject.GetComponent<Rigidbody2D>().velocity = speed * dir.normalized* ((resource.stamina + 50f) / 100f) * (1 + (resource.poseidonsFavour / 500f));
                }

                if (moveRate == 2)
                {
                    gameObject.GetComponent<Rigidbody2D>().velocity = fastSpeed * dir.normalized * ((resource.stamina + 50f) / 100f) * (1 + (resource.poseidonsFavour / 500f));
                }
                if(dir.magnitude<vertexDist)
                {
                    vertices.Dequeue();
                    Debug.Log(vertices.Count.ToString());
                }
            }
            

            LayerMask mask = 1 << 10;
            Collider[] currentWeather = Physics.OverlapSphere(transform.position, 0.1f, mask);

            for (int i = 0; i< currentWeather.Length; i++)
                 {
                    gameObject.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity * currentWeather[i].gameObject.GetComponent<WeatherScript>().weatherSpeedDown;
                   // gameObject.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity + currentWeather[i].gameObject.GetComponent<WeatherScript>().weatherDirection;
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
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
		
	}
	
	public void Unpause()
	{
		paused = false;
	}
	
	//used for anchoring
	public void toggleAnchor()
	{
		if(anchored)
		{
			anchored = false;
		}
		
		else if(!anchored)
		{
			anchored = true;
			Collider2D[] objectsNear = Physics2D.OverlapCircleAll(transform.position, encounterRange);
			for(int i = 0; i< objectsNear.Length;i++)
			{
				if (objectsNear[i].gameObject.tag.Equals("Island"))
				{
				islandParkedAt = objectsNear[i].gameObject;
				islandParkedAt.GetComponent<IslandEventScript>().HaveEvent();
                    Debug.Log(objectsNear[i].gameObject.name);
                }
			
			}
            Debug.Log(objectsNear.Length.ToString());
		}
	}
}
