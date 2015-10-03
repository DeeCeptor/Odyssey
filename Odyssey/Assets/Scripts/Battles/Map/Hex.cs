using UnityEngine;
using System.Collections.Generic;
using System;

public class Hex : MonoBehaviour, IComparable<Hex>
{
    public Vector2 coordinate;      // Grid coordinate
    public Vector3 world_coordinates;

    public List<Effect> effects_on_hex = new List<Effect>();

    [HideInInspector]
    public List<Edge> neighbours = new List<Edge>();     // List of adjacent and movable-to hexes

    [HideInInspector]
    public Unit occupying_unit;     // Unit that's on this hex, if there is one

	private Color highlighted_color = Color.blue;
	private Color mouse_highlighted_color = Color.green;
	private Color previous_color;
	Color regular_color = Color.white;

    // Graph traversing cost functions
    [HideInInspector]
    public int f_score = 0;
    [HideInInspector]
    public int g_score = 0;
    [HideInInspector]
    public Hex came_from;

    public string h_name;
    public string h_description;
    public int cost_to_enter_hex = 1;
    public bool impassable = false;     // If set to true, no unit can ever traverse this hex
    //List<Effects> effects_on_hex;

    // Used for AI
    public float hex_score;         // How 'favourable' this hex is for the AI. Based on nearby allies, and hurt enemies
    public float defense_score;     // What bonuses this hex gives


    void Start ()
    {
        //regular_material = this.GetComponent<MeshRenderer>().material;
        //regular_color = this.GetComponent<SpriteRenderer>().color;
        world_coordinates = this.transform.position;
    }

	
	void Update ()
    {
	
	}


    // Clones the effects on this hex and returns a list of the effects to be applied to the unit.
    public List<Effect> GetEffectsOnHex(Unit unit_standing_on_hex)
    {
        List<Effect> effects = new List<Effect>();

        foreach (Effect effect in effects_on_hex)
        {
            effects.Add(effect.Clone(unit_standing_on_hex));
        }

        return effects;
    }


    // Set the cost of all edges from adjacent hexes leading to adjacent hexes and all edges leading to this hex to be high,
    // as this unit is exerting a zone of control.
    public void SetZoneOfControl(Unit occupying_unit)
    {
        foreach (Edge edge in neighbours)
        {
            Hex cur_hex = edge.destination;

            foreach (Edge cur_edge in cur_hex.neighbours)
            {
                if (cur_edge.destination == this || HexMap.hex_map.InRange(cur_edge.destination, this, 1))
                {
                    //Debug.Log("Control from : " + cur_edge.source.coordinate + " to: " + cur_edge.destination.coordinate);
                    cur_edge.SetZoneOfControl(occupying_unit.owner);
                }
            }
        }
    }
    public void ResetZoneOfControl()
    {
        foreach (Edge edge in neighbours)
        {
            edge.ResetCost();
            Hex cur_hex = edge.destination;

            foreach (Edge cur_edge in cur_hex.neighbours)
            {
                if (cur_edge.destination == this || HexMap.hex_map.InRange(cur_edge.destination, this, 1))
                    cur_edge.ResetCost();
            }
        }
    }


    public void HighlightHex()
    {
		this.GetComponent<SpriteRenderer>().color = highlighted_color;
        //this.GetComponent<MeshRenderer>().material = highlighted_material;
    }
    public void UnhighlightHex()
    {
		this.GetComponent<SpriteRenderer>().color = regular_color;
        //this.GetComponent<MeshRenderer>().material = regular_material;
    }
    public void MouseHighlight()
    {
		previous_color = this.GetComponent<SpriteRenderer>().color;
		this.GetComponent<SpriteRenderer>().color = mouse_highlighted_color;
        //previous_material = this.GetComponent<MeshRenderer>().material;
        //this.GetComponent<MeshRenderer>().material = mouse_highlighted_material;
    }
    public void UnMouseHighlight()
    {
		this.GetComponent<SpriteRenderer>().color = previous_color;
        //this.GetComponent<MeshRenderer>().material = previous_material;
    }
    public bool IsHighlighted()
    {
        //return this.GetComponent<MeshRenderer>().material == highlighted_material;
		return this.GetComponent<SpriteRenderer>().color == Color.blue;

    }


    void OnMouseDown()
    {
        
    }


    void OnMouseEnter()
    {
        
    }
    void OnMouseExit()
    {
        PlayerInterface.player_interface.HideTerrainStatsPanel();
    }


    void OnMouseOver()
    {
        PlayerInterface.player_interface.MousedOverHex(this);

        if (Input.GetMouseButtonDown(0))    // left click
        {
            if (this.occupying_unit != null)    // Select the unit on this hex
                PlayerInterface.player_interface.UnitSelected(this.occupying_unit);
            else
                PlayerInterface.player_interface.UnitDeselected();
            //Debug.Log("Left click on this object");

            //if (PlayerInterface.player_interface.SelectedUnitAvailableToControl())
            //    PlayerInterface.player_interface.selected_unit.HexClicked(this);
        }
        if (Input.GetMouseButtonDown(1))    // right click
        {
            if (PlayerInterface.player_interface.SelectedUnitAvailableToControl())
                PlayerInterface.player_interface.selected_unit.HexClicked(this);
        }
        if (Input.GetMouseButtonDown(2))    // middle click
        {
            if (PlayerInterface.player_interface.SelectedUnitAvailableToControl())
                PlayerInterface.player_interface.selected_unit.HexClicked(this);
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hex")// && !neighbours.Contains(collision.gameObject.GetComponent<Hex>()))
        {
            GameObject neighbour = collision.gameObject;

            // Only create an edge between these 2 hexes if they're both passable
            if (!impassable && !neighbour.GetComponent<Hex>().impassable)
            {
                Edge edge = new Edge(neighbour.GetComponent<Hex>(), this, neighbour.GetComponent<Hex>().cost_to_enter_hex);
                neighbours.Add(edge);
                HexMap.hex_map.all_edges.Add(edge);
                Debug.DrawLine(this.transform.position, neighbour.transform.position, Color.red, 500);
            }
        }
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
}
