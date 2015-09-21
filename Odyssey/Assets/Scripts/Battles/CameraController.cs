using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
    float cur_zoom = 5;
    private float camera_z = -10;

	void Start () 
	{

	}
	
	void Update () 
	{
        float x = Input.GetAxis("Horizontal");  // left-right
		float y = Input.GetAxis("Vertical");   // up-down
		float z = Input.GetAxis("Mouse ScrollWheel");    // zoom

        cur_zoom = Mathf.Clamp(cur_zoom - z, 3, 10);    // Limit how far and how close we can zoom

        if (Camera.current != null)
        {
            this.gameObject.transform.position = new Vector3(
				Mathf.Clamp(this.gameObject.transform.position.x + x, HexMap.hex_map.x_min_cam, HexMap.hex_map.x_max_cam),
				Mathf.Clamp(this.gameObject.transform.position.y + y, HexMap.hex_map.y_min_cam, HexMap.hex_map.y_max_cam),
                camera_z);  // Z of camera never changes
            Camera.current.orthographicSize = cur_zoom;     // Set zoom level of camera
        }
    }
}
