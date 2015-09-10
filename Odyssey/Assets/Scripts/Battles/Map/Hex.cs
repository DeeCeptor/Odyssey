using UnityEngine;
using System.Collections.Generic;
using System;

public class Hex : MonoBehaviour, IComparable<Hex>
{
    public Vector2 coordinate;      // Grid coordinate

    public List<Edge> neighbours = new List<Edge>();     // List of adjacent and movable-to hexes

    public Unit occupying_unit;     // Unit that's on this hex, if there is one

    // Graph traversing cost functions
    public int f_score = 0;
    public int g_score = 0;
    public Hex came_from;


    void Start ()
    {

    }

	
	void Update ()
    {
	
	}


    void OnMouseDown()
    {
        PlayerInterface.player_interface.TeleportUnit(this);
    }


    /**
     * Method used for list.Sort() functionality in graph map. Sorts lowest to highest (ascending).
     * Returns the ordering when comparing two Hexes with their f_cost.
     */
    public int CompareTo(Hex other)
    {
        // Sort in order of lowest f_score
        if (this.f_score < other.f_score)
            return -1;
        if (this.f_score > other.f_score)
            return 1;
        // Else they're equal
        else
            return 0;
    }


    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Hex")// && !neighbours.Contains(collision.gameObject.GetComponent<Hex>()))
        {
            GameObject neighbour = collision.gameObject;
            //neighbours.Add(neighbour.GetComponent<Hex>());
            neighbours.Add(new Edge(neighbour.GetComponent<Hex>(), this, 1));
            Debug.DrawLine(this.transform.position, neighbour.transform.position, Color.red, 500);
        }
    }
}
