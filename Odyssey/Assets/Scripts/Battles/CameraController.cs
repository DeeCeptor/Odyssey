using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
    float cur_zoom;

	void Start () 
	{
        cur_zoom = this.transform.position.z;
	}
	
	void Update () 
	{
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        /*
        cur_zoom += Input.GetAxis("Mouse ScrollWheel") * 10;
        Vector3 cur_position = new Vector3(this.transform.position.x + x, cur_zoom, this.transform.position.y + y);
        this.transform.position = cur_position;*/
        
        if (Camera.current != null)
        {
            Camera.current.transform.Translate(new Vector3(
                x,
                y,
                0));
        }
    }
}
