using UnityEngine;
using System.Collections;

// Rotates sideways, then fades out the unit and then destroys it
public class RotateSideways : MonoBehaviour
{
    SpriteRenderer renderer;
    float rotateSpeed = 300f;
    float fadeSpeed = 2f;
    bool rotating = true;
    Quaternion from_rotation;
    float start;

	void Start ()
    {
        renderer = this.GetComponentInChildren<SpriteRenderer>();
        from_rotation = transform.rotation;
        start = Time.time;
    }
	
	void Update ()
    {
        if (rotating)
        {
            // Rotate sideways
            //transform.Rotate(new Vector3(0, 0, 3f), Space.World);
            start += Time.deltaTime;
            //Quaternion to_rotation = Quaternion.Euler(new Vector3(0, 0, -90));
            //transform.rotation = Quaternion.Lerp(from_rotation, to_rotation, start);
            transform.Rotate(new Vector3(0, 0, -Time.deltaTime * rotateSpeed));
            //Debug.Log(transform.rotation);
            if (transform.rotation.eulerAngles.z <= 270 && transform.rotation.eulerAngles.z > 10)
            {
                rotating = false;
            }
        }
        else
        {
            // Fade to transparent
            Color color = renderer.color;
            color.a = Mathf.Lerp(color.a, 0, Time.deltaTime * fadeSpeed);
            renderer.color = color;

            // Destroy when completely transparent
            if (color.a <= 0)
                GameObject.Destroy(this.gameObject);
        }
    }
}
