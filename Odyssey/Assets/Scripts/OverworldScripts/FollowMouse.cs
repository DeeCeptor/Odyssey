using UnityEngine;
using System.Collections;

public class FollowMouse : MonoBehaviour {
    public float speed = 10f;
    public float stopRange = 1;
    public GameObject ocean;
    public float oceanXWidth;
    public float oceanYWidth;
    // Use this for initialization
    void Start () {
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }
	
	// Update is called once per frame
	void Update () {
        if (ocean == null)
        {
            ocean = GameObject.FindGameObjectWithTag("Ocean");
            oceanXWidth = ocean.GetComponent<Renderer>().bounds.extents.x;
            oceanYWidth = ocean.GetComponent<Renderer>().bounds.extents.y;
        }
        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        
        if (dir.magnitude > stopRange)
        {
           
            GetComponent<Rigidbody2D>().velocity = dir * speed;
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        }

        //don't let off map
        if (transform.position.x < 0 - oceanXWidth)
        {
            transform.position = new Vector3(0 - oceanXWidth, transform.position.y, transform.position.z);
        }
        if (transform.position.y < 0 - oceanYWidth)
        {
            transform.position = new Vector3(transform.position.x, 0 - oceanYWidth, transform.position.z);
        }

        if (transform.position.x > 0 + oceanXWidth)
        {
            transform.position = new Vector3(0 + oceanXWidth, transform.position.y, transform.position.z);
        }
        if (transform.position.y > 0 + oceanYWidth)
        {
            transform.position = new Vector3(transform.position.x, 0 + oceanYWidth, transform.position.z);
        }
    }
}
