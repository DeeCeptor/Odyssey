using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
public float turnSpeed = 3f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			if(Input.GetAxis("Mouse X") > 0)
			{
				transform.RotateAround(transform.parent.position,transform.parent.up,turnSpeed);
			}
			
			if(Input.GetAxis("Mouse X") < 0)
			{
				transform.RotateAround(transform.parent.position,transform.parent.up,-turnSpeed);
			}
			
			if(Input.GetAxis("Mouse Y") > 0)
			{
			//wasn't very pleasant
				//transform.RotateAround(transform.parent.position,transform.parent.forward,turnSpeed);
			}
			
			if(Input.GetAxis("Mouse Y") < 0)
			{
				//wasn't very pleasant
				//transform.RotateAround(transform.parent.position,transform.parent.forward,-turnSpeed);
			}
		}
	}
}
