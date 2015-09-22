using UnityEngine;
using System.Collections;

public class mapPanScript : MonoBehaviour {
	public float mapPanSpeed = 1;
	public GameObject ocean;
	public bool active = false;
	public float minSize = 200;
	public float maxSize = 1000;
	public EventManagement eventHandler;
	public Camera camera;
	public float oceanXWidth;
	public float oceanZWidth;
	
	// Use this for initialization
	void Start () {
	camera = GetComponent<Camera>();
	}
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.M))
		{
			ToggleActivate();
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(ocean == null || eventHandler == null)
		{
			ocean = GameObject.FindGameObjectWithTag("Ocean");
			eventHandler = GameObject.FindGameObjectWithTag("EventController").GetComponent<EventManagement>();
			oceanXWidth = ocean.GetComponent<Renderer>().bounds.extents.x;
			oceanZWidth = ocean.GetComponent<Renderer>().bounds.extents.z;
		}
	
		if(active)
		{
			gameObject.GetComponent<Rigidbody>().velocity = new Vector3(Input.GetAxis("Horizontal")*mapPanSpeed * ((float)camera.orthographicSize),0,Input.GetAxis("Vertical")*mapPanSpeed * ((float)camera.orthographicSize));
			if(camera.orthographicSize > 2 || Input.GetAxis("Mouse ScrollWheel") > 0)
			{
				camera.orthographicSize = camera.orthographicSize - (3*Input.GetAxis("Mouse ScrollWheel"));
				//Debug.Log("mouse scroll axis : " + Input.GetAxis("Mouse ScrollWheel"));
				//Debug.Log("equation : 3 * " + Input.GetAxis("Mouse ScrollWheel") + " + " + background.transform.localScale.x + ", 3 * " + Input.GetAxis("Mouse ScrollWheel") + " + " + background.transform.localScale.y);
				float height = camera.orthographicSize * 2;
				float width = height * Screen.width / Screen.height;
			}
			ResizeMap();
		}
		
		
	}
	
	public void ToggleActivate()
	{
		if(active)
		{
			active = false;
			camera.enabled = false;
			eventHandler.Unpause();
		}
		else if(!active)
		{
			active = true;
			camera.enabled = true;
			eventHandler.Pause();
		}
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
		if(transform.position.z < 0- oceanZWidth)
		{
			transform.position = new Vector3(transform.position.x,transform.position.y,0- oceanZWidth);
		}
		
		if(transform.position.x > 0+ oceanXWidth)
		{
			transform.position = new Vector3(0+ oceanXWidth,transform.position.y,transform.position.z);
		}
		if(transform.position.z > 0+ oceanZWidth)
		{
			transform.position = new Vector3(transform.position.x,transform.position.y,0+ oceanZWidth);
		}
	}
}
