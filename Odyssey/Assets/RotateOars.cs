using UnityEngine;
using System.Collections;

public class RotateOars : MonoBehaviour {
    public float angleRotated;
    public float maxAngle = 45;
    public bool left =  false;
    public bool forward = true;
    private Vector3 pivotPoint;
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!EventManagement.gameController.paused && transform.parent.parent.parent.GetComponent<Rigidbody2D>().velocity.magnitude > 0)
        {
            if (left)
            {
                //rotate forward or backward
                if (forward)
                {
                    angleRotated += (22.5f * Time.deltaTime * (ResourceManager.playerResources.moveRate + 1));
                    transform.RotateAround(new Vector3(transform.parent.position.x, transform.parent.position.y,transform.parent.position.z)
                        ,transform.forward,22.5f*Time.deltaTime*(ResourceManager.playerResources.moveRate + 1));
                }
                else
                {
                    angleRotated -= (22.5f * Time.deltaTime * (ResourceManager.playerResources.moveRate + 1));
                    transform.RotateAround(new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z)
                        , transform.forward, -22.5f * Time.deltaTime * (ResourceManager.playerResources.moveRate + 1));
                }

                //
                if(angleRotated > maxAngle)
                {
                    forward = false;
                }
                if(angleRotated < -maxAngle)
                {
                    forward = true;
                }
            }
            else
            {
                if (forward)
                {
                    angleRotated += (22.5f * Time.deltaTime * (ResourceManager.playerResources.moveRate + 1));
                    transform.RotateAround(new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z)
                        , transform.forward, 22.5f * Time.deltaTime * (ResourceManager.playerResources.moveRate + 1));
                }
                else
                {
                    angleRotated -= (22.5f * Time.deltaTime * (ResourceManager.playerResources.moveRate + 1));
                    transform.RotateAround(new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z)
                        , transform.forward, -22.5f * Time.deltaTime * (ResourceManager.playerResources.moveRate + 1));
                }

                //
                if (angleRotated > maxAngle)
                {
                    forward = false;
                }
                if (angleRotated < -maxAngle)
                {
                    forward = true;
                }
            }
        }
	}
}
