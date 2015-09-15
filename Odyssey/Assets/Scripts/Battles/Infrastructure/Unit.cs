using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{
    [HideInInspector]
    public Hex location;       // Where this unit currently is
    [HideInInspector]
    public List<Hex> movement_path = new List<Hex>();      // When ordered to move, this path is populated with the hexes this unit will traverse
    [HideInInspector]
    public List<Hex> tiles_I_can_move_to;   // Tiles that will be highlighted when the user clicks on the unit

    bool active = false;    // Set to true when it 
    bool has_moved = false; // Can only move if this is set to false
    bool has_attacked = false;  // Can only attack once per round

    float maximum_health;
    float health;
    float normal_defense;
    float defense;
    float normal_damage;
    float damage;
    int normal_movement;
    int movement;

    public string u_name = ""; // Name at the top of the unit panel
    public string u_description = "";  // Short description of the unit
    public Texture portrait;    // Unit portrait

    // List<Ability> abilities;
    // List<Effects> current_effects;
    public Faction owner;


	void Start ()
    {
	
	}


	void Update ()
    {
        // Check if we should be moving
	    if (movement_path != null && movement_path.Count > 0)
        {
            //transform.LookAt(movement_path[0].transform.position);
            transform.position = Vector3.MoveTowards(transform.position, movement_path[0].transform.position, Time.deltaTime * 3f);

            if (transform.position == movement_path[0].transform.position)
            {
                if (movement_path.Count == 1)
                    location = movement_path[0];

                movement_path.RemoveAt(0);
            }
        }
	}


    // Called when the current owner's turn starts
    public virtual void StartTurn()
    {
        active = true;
        has_attacked = false;
        has_moved = false;

        // Get the effects on this hex
        //location.GetEffectsOnHex();
    }


    // Returns the health of the unit
    public virtual float GetHealth()
    {
        return health;
    }
    // Returns the attack damage of the unit
    public virtual float GetDamage()
    {
        return damage;
    }
    // Returns the defense of the unit
    public virtual float GetDefense()
    {
        return defense;
    }

    public float TakeHit(float incoming_damage)
    {
        // Modify the damage by the defense percentage. 10% defense means 90% of the damage is inflicted.
        // Always do a minimum of 1 damage.
        float modified_damage = Mathf.Max(1, incoming_damage - (incoming_damage * defense));

        health -= modified_damage;
        Debug.Log("Took " + modified_damage + " damaged, " + health + " HP remaining");

        if (health <= 0)
            Die();

        return health;
    }


    public virtual void Die()
    {
        // Remove from the unit lists
        this.owner.units.Remove(this);

        // Check victory/defeat conditions
        BattleManager.battle_manager.CheckVictoryAndDefeat();

        // Remove game object
        Destroy(this.gameObject);
    }


    void OnMouseDown()
    {
        if (PlayerInterface.player_interface.selected_unit != null
            && PlayerInterface.player_interface.selected_unit != this
            && !PlayerInterface.player_interface.selected_unit.has_attacked
            && PlayerInterface.player_interface.selected_unit.active
            && PlayerInterface.player_interface.selected_unit.owner.IsEnemy(this))
        {
            PlayerInterface.player_interface.selected_unit.Attack(this);
        }
        else
        {
            PlayerInterface.player_interface.UnitSelected(this);
        }
    }


    public void HexClicked(Hex hex)
    {
        if (this.owner == BattleManager.battle_manager.current_player 
            && BattleManager.battle_manager.current_player.human_controlled
            && this.active
            && !this.has_moved)
        {
            // Check if that's a valid spot. Can't have more than one unit sit on the same spot, can't move to the spot we're already on
            if (hex.occupying_unit == null && hex != location)
            {
                PathTo(hex);
            }
            else if (hex.occupying_unit != null && this.owner.IsEnemy(hex.occupying_unit))
            {
                Attack(hex.occupying_unit);
            }
        }
    }


    public void PathTo(Hex to)
    {
        // Check if that's a valid spot. Can't have more than one unit sit on the same spot, can't move to the spot we're already on
        if (to.occupying_unit == null && to != location)
        {
            // Pathfind to the correct spot
            if (this.movement_path.Count == 0)
            {
                this.movement_path = HexMap.hex_map.AStarFindPath(this.location, to, movement);
                if (movement_path.Count > 0)
                {
                    has_moved = true;
                }
            }
        }
    } 


    public void Attack(Unit victim)
    {
        if (victim != this 
            && !this.has_attacked 
            && this.active 
            && this.owner.IsEnemy(victim))
        {
            victim.TakeHit(this.GetDamage());
            has_attacked = true;
            active = false;
        }
    }
}
