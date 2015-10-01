using UnityEngine;
using System.Collections;

public class BattleCamera : MonoBehaviour
{
    private float camera_z = -10;
    Camera cur_camera;

    // Smooth zoom properties
    float desired_zoom = 5;
    float start_time;
    float starting_zoom;
    float duration = 0.3f;  // Speed of zooming


    void Start()
    {
        cur_camera = this.GetComponent<Camera>();
    }


    public void ChangeZoom(float zoom)
    {
        start_time = Time.time;
        desired_zoom = Mathf.Clamp(zoom, 3, 10);
        starting_zoom = this.cur_camera.orthographicSize;
    }


    void Update()
    {
        float x = Input.GetAxis("Horizontal");  // left-right
        float y = Input.GetAxis("Vertical");   // up-down
        float z = Input.GetAxis("Mouse ScrollWheel");    // zoom

        // Smoothly zoom in and out if the player scrolled the scroll wheel
        if (z != 0)
        {
            ChangeZoom(desired_zoom - z);
        }

        if (desired_zoom != this.cur_camera.orthographicSize)
        {
            float i = (Time.time - start_time) / duration;
            cur_camera.orthographicSize = Mathf.Lerp(starting_zoom, desired_zoom, i);
        }
        //desired_zoom = Mathf.Clamp(cur_zoom - z, 3, 10);    // Limit how far and how close we can zoom

        this.transform.position = new Vector3(
            Mathf.Clamp(this.transform.position.x + x, HexMap.hex_map.x_min_cam, HexMap.hex_map.x_max_cam),
            Mathf.Clamp(this.transform.position.y + y, HexMap.hex_map.y_min_cam, HexMap.hex_map.y_max_cam),
            camera_z);  // Z of camera never changes
        //cur_camera.orthographicSize = cur_zoom;     // Set zoom level of camera
    }
}
