using UnityEngine;
using System.Collections;

public class mapPanScript : MonoBehaviour {
	public float mapPanSpeed = 1;
	public GameObject ocean;
	public float minSize = 200;
	public float maxSize = 1000;
	public EventManagement eventHandler;
	public Camera camera;
	public float oceanXWidth;
	public float oceanYWidth;
    public GameObject player;
	
	// Use this for initialization
	void Start () {
	camera = GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }
	
	void Update()
	{
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(ocean == null || eventHandler == null)
		{
			ocean = GameObject.FindGameObjectWithTag("Ocean");
			eventHandler = GameObject.FindGameObjectWithTag("EventController").GetComponent<EventManagement>();
			oceanXWidth = ocean.GetComponent<Renderer>().bounds.extents.x;
			oceanYWidth = ocean.GetComponent<Renderer>().bounds.extents.y;
		}
	
		gameObject.GetComponent<Rigidbody>().velocity = new Vector3(-Input.GetAxis("Horizontal")*mapPanSpeed * ((float)camera.orthographicSize), Input.GetAxis("Vertical") * mapPanSpeed * ((float)camera.orthographicSize), 0);
		if(camera.orthographicSize > 2 || Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			camera.orthographicSize = camera.orthographicSize - (3*Input.GetAxis("Mouse ScrollWheel"));
			//Debug.Log("mouse scroll axis : " + Input.GetAxis("Mouse ScrollWheel"));
			//Debug.Log("equation : 3 * " + Input.GetAxis("Mouse ScrollWheel") + " + " + background.transform.localScale.x + ", 3 * " + Input.GetAxis("Mouse ScrollWheel") + " + " + background.transform.localScale.y);
			float height = camera.orthographicSize * 2;
			float width = height * Screen.width / Screen.height;
		}

        if(Input.GetKeyDown(KeyCode.M))
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        }
        
        ResizeMap();
		
		
	}
	
	//ensure map is still on the screen
	public void ResizeMap()
	{
		if(camera.orthographicSize < minSize)
		{
			camera.orthographicSize = minSize;
		}
		if(camera.orthographicSize > maxSize)
		{
			camera.orthographicSize = maxSize;
		}
		
		if(transform.position.x < 0- oceanXWidth)
		{
			transform.position = new Vector3(0- oceanXWidth,transform.position.y,transform.position.z);
		}
		if(transform.position.y < 0- oceanYWidth)
		{
			transform.position = new Vector3(transform.position.x, 0 - oceanYWidth, transform.position.z);
		}
		
		if(transform.position.x > 0+ oceanXWidth)
		{
			transform.position = new Vector3(0+ oceanXWidth,transform.position.y,transform.position.z);
		}
		if(transform.position.y > 0+ oceanYWidth)
		{
			transform.position = new Vector3(transform.position.x, 0 + oceanYWidth, transform.position.z);
		}
	}
}
