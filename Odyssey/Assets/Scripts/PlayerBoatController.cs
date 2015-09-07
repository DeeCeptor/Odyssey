using UnityEngine;
using System.Collections;

public class PlayerBoatController : MonoBehaviour {
	public float speed = 1f;
	public float fastSpeed = 1.5f;
	public float slowSpeed = 0.5f;
	public float turnSpeed = 0.2f;
	private int moveRate = 1;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		if(moveRate == 0)
		{
			gameObject.GetComponent<Rigidbody>().AddForce(Input.GetAxis("Vertical")*slowSpeed*transform.forward);
			transform.Rotate(Input.GetAxis("Horizontal")*turnSpeed*transform.up);
		}
	
		if(moveRate == 1)
		{
			gameObject.GetComponent<Rigidbody>().AddForce(Input.GetAxis("Vertical")*speed*transform.forward);
			transform.Rotate(Input.GetAxis("Horizontal")*turnSpeed*transform.up);
		}
	
		if(moveRate == 2)
		{
			gameObject.GetComponent<Rigidbody>().AddForce(Input.GetAxis("Vertical")*fastSpeed*transform.forward);
			transform.Rotate(Input.GetAxis("Horizontal")*turnSpeed*transform.up);
		}
	}
	
	public void SlowSpeed()
	{
		moveRate = 0;
	}
	
	public void MediumSpeed()
	{
		moveRate = 1;
	}
	
	public void HighSpeed()
	{
		moveRate = 2;
	}
}
