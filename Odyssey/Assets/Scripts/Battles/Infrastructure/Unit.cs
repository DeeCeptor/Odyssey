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

    bool active = false;    // Set to true when it's this unit's turn
    [HideInInspector]
    public bool ready_to_be_controlled = false;     // True when it's the human players turn and this unit is ready to act
    bool has_moved = false; // Can only move if this is set to false
    bool has_attacked = false;  // Can only attack once per round

    float maximum_health;
    float health;
    float normal_defense;
    float defense;
    int normal_attack_range = 1;
    int attack_range = 1;
    float normal_damage;
    float damage;
    int normal_movement;
    int movement = 3;

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

        if (owner.human_controlled)
            ready_to_be_controlled = true;
        //else

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
    public virtual int GetRange()
    {
        return attack_range;
    }
    // Returns the defense of the unit
    public virtual float GetDefense()
    {
        return defense;
    }
    public virtual int GetMovement()
    {
        return movement;
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


    public bool IsControllable()
    {
        return (this.active
                && this.ready_to_be_controlled);
    }


    void OnMouseDown()
    {
        // Attack if we clicked on an enemy unit
        if (PlayerInterface.player_interface.SelectedUnitAvailableToControl()
            && PlayerInterface.player_interface.selected_unit != this
            && !PlayerInterface.player_interface.selected_unit.has_attacked
            && PlayerInterface.player_interface.selected_unit.owner.IsEnemy(this)
            && HexMap.hex_map.InRange(PlayerInterface.player_interface.selected_unit.location, this.location, attack_range))
        {
            PlayerInterface.player_interface.selected_unit.Attack(this);
        }
        else
        {
            // Deselect other unit
            PlayerInterface.player_interface.UnitDeselected();

            // Select the unit
            PlayerInterface.player_interface.UnitSelected(this);

            // If the unit hasn't moved, is owned by the player and is active, highlight where it can move to
            /*if (PlayerInterface.player_interface.selected_unit.active
                && !PlayerInterface.player_interface.selected_unit.has_moved)
            {
                // Highlight each hex
                foreach (Hex hex in tiles_I_can_move_to)
                {
                    hex.HighlightHex();
                }
            }*/
        }
    }


    public void HighlightHexesWeCanMoveTo()
    {
        foreach (Hex hex in this.tiles_I_can_move_to)
        {
            hex.HighlightHex();
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
                PlayerInterface.player_interface.UnhighlightHexes();
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
        if (to.occupying_unit == null && to != this.location)
        {
            // Pathfind to the correct spot
            if (this.movement_path.Count == 0)
            {
                this.movement_path = HexMap.hex_map.AStarFindPath(this.location, to, movement);
                if (movement_path.Count > 0)
                {
                    SetLocation(to);
                    has_moved = true;
                    BattleManager.battle_manager.SetUnitsMovableTiles();
                }
            }
        }
    } 


    public void SetLocation(Hex hex)
    {
        this.location.occupying_unit = null;
        this.location = hex;
        hex.occupying_unit = this;
    }


    public void Attack(Unit victim)
    {
        if (victim != this 
            && !this.has_attacked 
            && this.active 
            && this.owner.IsEnemy(victim)
            && HexMap.hex_map.InRange(this.location, victim.location, attack_range))
        {
            victim.TakeHit(this);
            has_attacked = true;
            active = false;
        }
    }
    public float TakeHit(Unit attacker)
    {
        float modified_damage = CalculateDamage(attacker);
        health -= modified_damage;
        Debug.Log("Took " + modified_damage + " damaged, " + health + " HP remaining");

        if (health <= 0)
            Die();

        return health;
    }
    public float CalculateDamage(Unit attacker)
    {
        // Modify the damage by the defense percentage. 10% defense means 90% of the damage is inflicted.
        // Always do a minimum of 1 damage.
        return Mathf.Max(1, attacker.GetDamage() - (attacker.GetDamage() * this.GetDefense()));
    }
}
