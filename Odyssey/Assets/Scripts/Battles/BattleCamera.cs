using UnityEngine;
using System.Collections;

public class BattleCamera : MonoBehaviour
{
    private float camera_z = -10;
    Camera cur_camera;
    HexMap map;

    // Smooth zoom properties
    float desired_zoom = 5;
    float start_time;
    float starting_zoom;
    float duration = 0.3f;  // Speed of zooming
    float x_min, y_min, x_max, y_max;

    void Start()
    {
        cur_camera = this.GetComponent<Camera>();
        StartCoroutine(get_boundaries());
    }
    IEnumerator get_boundaries()
    {
        yield return new WaitForSeconds(0.5f);

        x_min = HexMap.hex_map.x_min_cam;
        y_min = HexMap.hex_map.y_min_cam;
        x_max = HexMap.hex_map.x_max_cam;
        y_max = HexMap.hex_map.y_max_cam;
    }


    public void ChangeZoom(float zoom)
    {
        start_time = Time.time;
        desired_zoom = Mathf.Clamp(zoom, 3, 10);
        starting_zoom = this.cur_camera.orthographicSize;
    }

    /*
    void Update()
    {
        float x = Input.GetAxis("Horizontal") * 10;  // left-right
        float y = Input.GetAxis("Vertical") * 10;   // up-down
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
        
        this.transform.position = new Vector3(
            Mathf.Clamp(this.transform.position.x + x * Time.deltaTime, HexMap.hex_map.x_min_cam, HexMap.hex_map.x_max_cam),
            Mathf.Clamp(this.transform.position.y + y * Time.deltaTime, HexMap.hex_map.y_min_cam, HexMap.hex_map.y_max_cam),
            camera_z);  // Z of camera never changes
        
            this.transform.position = new Vector3(
        Mathf.Clamp(this.transform.position.x + x * Time.deltaTime, x_min, x_max),
        Mathf.Clamp(this.transform.position.y + y * Time.deltaTime, y_min, y_max),
        camera_z);  // Z of camera never changes
    }*/

    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal") / 2;  // left-right
        float y = Input.GetAxis("Vertical") / 2;   // up-down
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

        this.transform.position = new Vector3(
            Mathf.Clamp(this.transform.position.x + x, x_min, x_max),
            Mathf.Clamp(this.transform.position.y + y, y_min, y_max),
            camera_z);  // Z of camera never changes
        }
}
