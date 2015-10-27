using UnityEngine;
using System.Collections;

public class Wander : MonoBehaviour
{
    public float wander_radius = 20;
    public float movement_speed = 1;
    public bool dont_stray = true;  // If set to false, the birds will wander incredibly far in random directions
    Vector2 center;
    Vector2 destination;

	void Start ()
    {
        center = this.transform.position;
        SetDestination();
    }
	
	void Update ()
    {
        //transform.LookAt(destination);
        this.transform.position = Vector2.MoveTowards(transform.position, destination, Time.deltaTime * movement_speed);

        if (Vector2.Distance(transform.position, destination) <= 1)
        {
            SetDestination();
        }
    }

    void SetDestination()
    {
        if (!dont_stray)
            center = destination;

        destination = center + Random.insideUnitCircle * wander_radius;
    }
}
