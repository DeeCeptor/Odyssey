using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;

public class Hex : MonoBehaviour, IComparable<Hex>
{
    public Vector2 coordinate;      // x,y Grid coordinate
    public Vector3 world_coordinates;   // Coordinate in real world space (unity game space)
    public Vector2 top_down_left_right_coordinate;  // Grid as if going from the normal reading angle

    public List<Effect> effects_on_hex = new List<Effect>();

    [HideInInspector]
    public List<Edge> neighbours = new List<Edge>();     // List of adjacent and movable-to hexes

    [HideInInspector]
    public Unit occupying_unit;     // Unit that's on this hex, if there is one

    private SpriteRenderer aura;
	private Color highlighted_move_color = Color.blue;
    private Color highlighted_attack_color = Color.red;
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

    // Properties
    public string h_name;
    public string h_description;
    public int cost_to_enter_hex = 1;
    public bool impassable = false;     // If set to true, no unit can ever traverse this hex
    public bool deployment_zone = false;     // Player can deploy units on this hex in the player unit deployment screen
    public bool retreat_zone = false;    // Player can safely retreat units from this hex

    // Used for AI
    public float hex_score;         // How 'favourable' this hex is for the AI. Based on nearby allies, and hurt enemies


    // AI terrain score. How highly does each genre of unit value this hex?
    public float ranged_score = 0;
    public float melee_score = 0;
    public float cavalry_score = 0;


    void Start ()
    {
        //regular_material = this.GetComponent<MeshRenderer>().material;
        //regular_color = this.GetComponent<SpriteRenderer>().color;
        world_coordinates = this.transform.position;
        aura = this.transform.FindChild("HexAura").GetComponent<SpriteRenderer>();
        aura.gameObject.SetActive(false);
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


    public void HighlightMoveHex()
    {
        aura.gameObject.SetActive(true);
		aura.color = highlighted_move_color;
    }
    public void HighlightAttackableHex()
    {
        aura.gameObject.SetActive(true);
        aura.color = highlighted_attack_color;
    }
    public void UnhighlightHex()
    {
        aura.gameObject.SetActive(false);
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
        return aura.gameObject.active;
    }


    public float HexTerrainScoreForUnit(Unit unit)
    {
        switch (unit.unit_type)
        {
            case Unit_Types.Melee:
                return this.melee_score;
            case Unit_Types.Ranged:
                return this.ranged_score;
            case Unit_Types.Cavalry:
                return this.cavalry_score;
        }
        return 0;
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
         
        // left click
        if (Input.GetMouseButtonDown(0)
            && !EventSystem.current.IsPointerOverGameObject()   // Make sure mouse is not over UI
            )    
        {
            if (this.occupying_unit != null)    // Select the unit on this hex
                PlayerInterface.player_interface.UnitSelected(this.occupying_unit);
            else
                PlayerInterface.player_interface.UnitDeselected();
            //Debug.Log("Left click on this object");

            //if (PlayerInterface.player_interface.SelectedUnitAvailableToControl())
            //    PlayerInterface.player_interface.selected_unit.HexClicked(this);
        }
        // right click
        if (Input.GetMouseButtonDown(1)
            && !EventSystem.current.IsPointerOverGameObject())
        {
            if (PlayerInterface.player_interface.SelectedUnitAvailableToControl())
                PlayerInterface.player_interface.selected_unit.HexClicked(this);
        }
        // middle click
        if (Input.GetMouseButtonDown(2)
            && !EventSystem.current.IsPointerOverGameObject())
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
