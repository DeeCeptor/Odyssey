using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
    float cur_zoom = 5;

	void Start () 
	{
        cur_zoom = this.transform.position.y;
	}
	
	void Update () 
	{
        float x = Input.GetAxis("Horizontal");  // left-right
        float y = Input.GetAxis("Mouse ScrollWheel");   // zoom
        float z = Input.GetAxis("Vertical");    // up-down
        //Debug.Log(y);
        /*
        cur_zoom += Input.GetAxis("Mouse ScrollWheel") * 10;
        Vector3 cur_position = new Vector3(this.transform.position.x + x, cur_zoom, this.transform.position.y + y);
        this.transform.position = cur_position;*/

        cur_zoom = Mathf.Clamp(cur_zoom - y, 1, 10);

        if (Camera.current != null)
        {
            this.gameObject.transform.position = new Vector3(Mathf.Clamp(gameObject.transform.position.x + x, 0, 10),
                cur_zoom,//Mathf.Clamp(gameObject.transform.position.y + y, 0, 10),
                Mathf.Clamp(gameObject.transform.position.z + z, -3, 8)    );
            //Camera.current.transform.Translate(new Vector3(
            /*this.gameObject.transform.Translate(new Vector3(
                x,
                y,
                0));*/
        }
    }
}
