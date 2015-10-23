using UnityEngine;
using System.Collections;

public class InstantlyFollowMouse : MonoBehaviour
{
    public Vector2 offset;
    public int z_position;

	void Start () {
	
	}
	
	void Update ()
    {
        // Move to the mouse's position
        this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, z_position) + (Vector3) offset;
	}
}
