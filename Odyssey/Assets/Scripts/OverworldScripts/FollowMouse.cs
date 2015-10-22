using UnityEngine;
using System.Collections;

public class FollowMouse : MonoBehaviour {
    public float speed = 10f;
    public float stopRange = 1;
	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        
        if (dir.magnitude > stopRange)
        {
           
            GetComponent<Rigidbody2D>().velocity = dir * speed;
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        }
	}
}
