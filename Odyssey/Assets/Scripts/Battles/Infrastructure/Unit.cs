﻿using UnityEngine;
using System.Collections.Generic;

public enum Unit_Types { Melee, Cavalry, Ranged };

public class Unit : MonoBehaviour
{
    [HideInInspector]
    public Hex location;       // Where this unit currently is
    [HideInInspector]
    public Vector2 location_coordinates;
    [HideInInspector]
    public List<Hex> movement_path = new List<Hex>();      // When ordered to move, this path is populated with the hexes this unit will traverse
    [HideInInspector]
    public Unit attack_target;  // Used for AI units. Who they will attack when they've reached their destination
    [HideInInspector]
    public List<Hex> tiles_I_can_move_to;   // Tiles that will be highlighted when the user clicks on the unit

    // UNIT ACTIVATION
    [HideInInspector]
    public bool active = false;    // Set to true when it's this unit's turn
    [HideInInspector]
    public bool ready_to_be_controlled = false;     // True when it's the human players turn and this unit is ready to act
    [HideInInspector]
    public bool has_moved = false; // Can only move if this is set to false
    [HideInInspector]
    public bool is_moving = false;      // True when the unit is moving
    [HideInInspector]
    public bool has_attacked = false;  // Can only attack once per round
    [HideInInspector]
    public bool dead = false;


    protected List<Effect> effects_on_unit = new List<Effect>();
    protected List<Effect> remove_effects = new List<Effect>();     // Effects to be removed shortly


    // UNIT STATS
    // Normal stats are the base stats of the unit.
    // The non-normal stats are the current stats of the unit
    public float maximum_health = 20;
    float health = 20;
    public float normal_defence = 0.3f;     // Defense is a percentage, from 0 to 1 of how much damage is blocked
    float defence = 0.3f;
    public float normal_ranged_defence = 0.3f;      // Specific 0 to 1 % blocked from ranged attacks
    float ranged_defence = 0.3f;
    public int normal_attack_range = 1;     // 1 is adjacent. 0 would mean unable to attack
    int attack_range = 1;
    public float normal_damage = 8;     // Damage is blocked by defence
    float damage = 8;
    public float normal_piercing_damage = 2;    // Piercing damage ignores defence, making this stat extremely valuable
    float piercing_damage;
    public int normal_movement = 3;     // How far this unit can move in a turn.
    int movement = 3;

    public Unit_Types unit_type;
    public bool is_ranged_unit = false;
    public bool counter_attacks = true;     // Counterattacks if the enemy is within range and in the frontal facing arc
    public int counter_attack_radius = 60; // The difference in facing counter attacks can be done from. 60 means the front 3 hexes
    public bool attacks_are_counterable = true;


    // AI scores used to determine how this unit should move
    public float offensive_AI_score = 1;  // If this is an AI unit, this value indicates how agressively we should advance towards the enemy
    public float flanking_factor = 1.5f;    // Damage multiplied by this factor when flanking, to show that we're not taking damage
    public float ally_grouping_score = 0.1f;

    // Which direction the unit is facing. Hexagons have 6 facings. 360/6 = 60. This value will be a multiple of 6
    [HideInInspector]
    public int facing = 0;
    [HideInInspector]
    public bool rotating = false;
    private float tile_move_speed = 7f;
    private bool desired_rotation_set = false;

    public string u_name = ""; // Name at the top of the unit panel
    public string u_description = "";  // Short description of the unit
    public Texture portrait;    // Unit portrait
    [HideInInspector]
    public string prefab_name;  // Exact name needed to load the prefab
    [HideInInspector]
    public GameObject unit_menu;

    // List<Ability> abilities;
    public Faction owner;


	void Start ()
    {
        unit_menu = this.transform.FindChild("UnitMenu").gameObject;
        this.SetRotation(new Vector3(0, 0, 0));
        health = maximum_health;

        // Set aura so we can tell which faction this player belongs to
        this.transform.FindChild("PlayerAura").GetComponent<SpriteRenderer>().color = this.owner.faction_color;

        ResetStats();
    }


	void Update ()
    {
        if (desired_rotation_set)
        {
            this.transform.eulerAngles = new Vector3(0, 0, facing);
            desired_rotation_set = false;
        }
        // Check if we should be moving
        else if (movement_path != null && movement_path.Count > 0)
        {
            //transform.LookAt(movement_path[0].transform.position);
            //transform.position = Vector3.MoveTowards(transform.position, movement_path[0].transform.position, Time.deltaTime * 3f);
            Vector3 pos = Vector2.MoveTowards(transform.position, movement_path[0].transform.position, Time.deltaTime * tile_move_speed);
            //pos.z = 0;
            transform.position = pos;
            //transform.position = new Vector3(transform.position.x, transform.position.y, 0);

            if ((Vector2) transform.position == (Vector2) movement_path[0].transform.position)
            {
                if (movement_path.Count == 1)
                {
                    location = movement_path[0];

                    if (attack_target == null)  // is_moving is a flag used to know when the AI unit is finished its turn
                        is_moving = false;
                }

                movement_path.RemoveAt(0);
            }
        }
        else if (attack_target != null)
        {
            // Initiate an attack on the target once we're done moving
            this.Attack(attack_target, attacks_are_counterable);
            attack_target = null;

            is_moving = false;
        }
        else if (rotating)
        {
            // Rotate towards mouse
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            SetRotationTowards(this.transform.position, mousePos);
        }
    }


    public void SetRotationTowards(Vector3 from, Vector3 towards)
    {
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, towards - from);

        // Use euler angles so we're dealing with degrees 0-360
        Vector3 angles = rotation.eulerAngles;
        angles.x = 0;
        angles.y = 0;

        SetRotation(angles);
    }
    public void SetRotation(Vector3 angles)
    {
        // Snap to one of the hexagon directions
        if (angles.z >= 0 && angles.z < 60)
            angles.z = 30;
        else if (angles.z >= 60 && angles.z < 120)
            angles.z = 90;
        else if (angles.z >= 120 && angles.z < 180)
            angles.z = 150;
        else if (angles.z >= 180 && angles.z < 240)
            angles.z = 210;
        else if (angles.z >= 240 && angles.z < 300)
            angles.z = 270;
        else if (angles.z >= 300 && angles.z < 360)
            angles.z = 330;

        this.transform.eulerAngles = angles;

        // Set facing
        this.facing = (int)angles.z;
    }
    public void SetDesiredRotationTowards(Vector3 from, Vector3 towards)
    {
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, towards - from);

        // Use euler angles so we're dealing with degrees 0-360
        Vector3 angles = rotation.eulerAngles;
        angles.x = 0;
        angles.y = 0;

        SetDesiredRotation(angles);
    }
    public void SetDesiredRotation(Vector3 angles)
    {
        // Snap to one of the hexagon directions
        if (angles.z >= 0 && angles.z < 60)
            angles.z = 30;
        else if (angles.z >= 60 && angles.z < 120)
            angles.z = 90;
        else if (angles.z >= 120 && angles.z < 180)
            angles.z = 150;
        else if (angles.z >= 180 && angles.z < 240)
            angles.z = 210;
        else if (angles.z >= 240 && angles.z < 300)
            angles.z = 270;
        else if (angles.z >= 300 && angles.z < 360)
            angles.z = 330;

        // Set facing
        this.facing = (int)angles.z;
        desired_rotation_set = true;
    }


    // Called when the current owner's turn starts
    public virtual void StartTurn()
    {
        active = true;
        has_attacked = false;
        has_moved = false;

        if (owner.human_controlled)
            ready_to_be_controlled = true;
    }
    public virtual void EndTurn()
    {
        if (owner.human_controlled)
            active = false;
        ready_to_be_controlled = false;
    }


    // Used for setting the facing of the unit
    public void StartRotating()
    {
        rotating = true;
    }
    public void StopRotating()
    {
        rotating = false;
    }
    // Used for deciding if this unit will counterattack the other unit
    public bool IsFacing(Unit unit)
    {
        // Check if the other unit is within our frontal arc
        /*
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, unit.transform.position - this.transform.position);
        float angle_towards_unit = rotation.eulerAngles.z;
        int angle_diff = (int) ((angle_towards_unit - this.facing + 180) % 360 - 180);    // Do math to get the difference in this units facing and the direction towards the enemy

        Debug.Log("Victim: " + facing + " towards enemy: " + angle_towards_unit + " diff: " + angle_diff);

        return (Mathf.Abs(angle_diff) <= counter_attack_radius);
        */
        return IsFacing(unit.location);
    }
    // Is the unit facing this hex? (Used for counter attacking)
    public bool IsFacing(Hex hex)
    {
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, hex.world_coordinates - this.location.world_coordinates);//this.transform.position);
        int angle_towards_unit = (int) rotation.eulerAngles.z;
        int angle_diff = (int)Mathf.Abs(((angle_towards_unit - this.facing + 180) % 360 - 180));    // Do math to get the difference in this units facing and the direction towards the enemy

        return (Mathf.Abs(angle_diff) <= counter_attack_radius + 5);
    }


    // Returns the health of the unit
    public virtual float GetHealth()
    {
        return health;
    }
    public virtual float GetMaxHealth()
    {
        return maximum_health;
    }

    // Returns the attack damage of the unit
    public virtual float GetDamage()
    {
        return damage;
    }
    // Adds to the units damage.
    // Percent is 0-1. 0.10 means add 10% of its normal damage
    public virtual void AdjustDamage(float constant_amount, float percent)
    {
        damage += constant_amount;
        damage += normal_damage * percent;
    }

    // Returns the piercing damage of the unit. Piercing damage ignores the defense of the unit
    public virtual float GetPiercingDamage()
    {
        return piercing_damage;
    }
    // Adds to the units piercing damage.
    // Percent is 0-1. 0.10 means add 10% of its normal damage
    public virtual void AdjustPiercingDamage(float constant_amount, float percent)
    {
        piercing_damage += constant_amount;
        piercing_damage += piercing_damage * percent;
    }

    public virtual int GetRange()
    {
        return attack_range;
    }
    public virtual void AdjustRange(int amount)
    {
        attack_range = Mathf.Max(1, attack_range + amount);
    }

    // Returns the defence of the unit
    public virtual float GetDefence()
    {
        return defence;
    }
    // Adds to the units damage.
    // Percent is 0-1. 0.10 means add 10% of its normal defence
    public virtual void AdjustDefence(float constant_amount)
    {
        defence = Mathf.Min(0.95f, Mathf.Max(0, defence + constant_amount));
    }

    // Returns the defence of the unit
    public virtual float GetRangedDefence()
    {
        return ranged_defence;
    }
    // Adds to the units damage.
    // Percent is 0-1. 0.10 means add 10% of its normal defence
    public virtual void AdjustRangedDefence(float constant_amount)
    {
        ranged_defence = Mathf.Min(0.95f, Mathf.Max(0, ranged_defence + constant_amount));
    }

    public virtual int GetMovement()
    {
        return movement;
    }
    public virtual void AdjustMovement(int amount)
    {
        movement = Mathf.Max(0, movement + amount);
    }


    // Returns true if the other unit is within the frontal 3 arc of this units facing
    public bool FacingTowards(Unit unit)
    {
        //Vector3 direction = unit.transform.position - this.transform.position;
        //direction.Normalize();

        //float angle = Vector2.Angle(unit.transform.position, this.transform.position);

        float angle = 180;
        return (Vector3.Angle(unit.transform.forward, transform.position - this.transform.position) <= angle);
    }


    public virtual void Die()
    {
        // Remove from the unit lists
        this.owner.units.Remove(this);
        dead = true;

        // Check victory/defeat conditions
        BattleManager.battle_manager.CheckVictoryAndDefeat();

        this.location.occupying_unit = null;

        BattleManager.battle_manager.SetUnitsMovableTiles();

        // Remove game object
        Destroy(this.gameObject);
    }


    public bool IsControllable()
    {
        return (this.active
                && this.ready_to_be_controlled
                && !this.is_moving);
    }


    // Show the stats of this unit when the user mouses over
    void OnMouseEnter()
    {
        PlayerInterface.player_interface.ShowUnitStatsPanel(this);
    }
    void OnMouseExit()
    {
        if (PlayerInterface.player_interface.selected_unit != null)// && PlayerInterface.player_interface.selected_unit != this)
            PlayerInterface.player_interface.ShowUnitStatsPanel(PlayerInterface.player_interface.selected_unit);
        else
            PlayerInterface.player_interface.HideUnitStatsPanel();
    }


    // Attack unit if we right clicked on it and we have another unit selected
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1)     // Right clicked on unit
            && PlayerInterface.player_interface.SelectedUnitAvailableToControl()
            && !PlayerInterface.player_interface.selected_unit.has_attacked
            && PlayerInterface.player_interface.selected_unit.owner.IsEnemy(this))
        {
            Debug.Log("OnMouseOver attack");
            PlayerInterface.player_interface.selected_unit.Attack(this, attacks_are_counterable);
        }
    }
    void OnMouseDown()      // Left clicked on unit
    {
        // Deselect other unit
        PlayerInterface.player_interface.UnitDeselected();

        // Select the unit
        PlayerInterface.player_interface.UnitSelected(this);
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
        Debug.Log("HexClicked");

        if (this.owner == BattleManager.battle_manager.current_player 
            && BattleManager.battle_manager.current_player.human_controlled
            && this.active )
        {
            // Check if that's a valid spot. Can't have more than one unit sit on the same spot, can't move to the spot we're already on
            if (hex.occupying_unit == null && hex != location && !this.has_moved)
            {
                PlayerInterface.player_interface.UnhighlightHexes();
                PathTo(hex);
            }
            else if (hex.occupying_unit != null && this.owner.IsEnemy(hex.occupying_unit) && !this.has_attacked)
            {
                Debug.Log("HexClicked attack");
                Attack(hex.occupying_unit, attacks_are_counterable);
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
                Debug.Log("FF");
                this.movement_path = HexMap.hex_map.AStarFindPath(this.location, to, movement, this.owner);
                Debug.Log("path length " + movement_path.Count);
                if (movement_path.Count > 0)
                {
                    Debug.Log("before SetLocation");
                    SetLocation(to);
                    has_moved = true;
                    is_moving = true;
                    Debug.Log("before SetUnitsMovableTiles");

                    BattleManager.battle_manager.SetUnitsMovableTiles();
                    Debug.Log("after SetUnitsMovableTiles");
                }
                else
                    Debug.Log("CC");
            }
            else
                Debug.Log("BB");
        }
        else
            Debug.Log("AA");
    } 


    public void SetLocation(Hex hex)
    {
        Debug.Log("before UnitMovedChangeEffects");
        // Remove the effects from this 
        UnitMovedChangeEffects();
        Debug.Log("after UnitMovedChangeEffects");
        if (this.location != null)
        {
           // hex.ResetZoneOfControl();
            this.location.occupying_unit = null;
        }
        this.location = hex;
        hex.occupying_unit = this;
        this.location_coordinates = new Vector2(hex.coordinate.x, hex.coordinate.y);
        // hex.SetZoneOfControl(this);

        Debug.Log("before GetHexEffects");
        // Get the effects on this hex
        GetHexEffects(hex);
        Debug.Log("after GetHexEffects");
    }
    // Add the bonuses conferred by the hex to this unit
    public void GetHexEffects(Hex hex)
    {
        effects_on_unit.AddRange(hex.GetEffectsOnHex(this));
        EvaluateEffects();
    }


    // Resets units to the 'normal' stats. Called before appyling the effects on a unit
    public void ResetStats()
    {
        movement = normal_movement;
        defence = normal_defence;
        ranged_defence = normal_ranged_defence;
        damage = normal_damage;
        piercing_damage = normal_piercing_damage;
        attack_range = normal_attack_range;
    }
    public void EvaluateEffects()
    {
        ResetStats();

        foreach (Effect effect in effects_on_unit)
        {
            effect.ApplyEffect();
        }
    }


    // Called when the unit has moved to remove the old hex's effect
    public void UnitMovedChangeEffects()
    {
        // Iterate through list backwards, so elements we remove don't change our ordering
        for (int i = effects_on_unit.Count - 1; i >= 0; i--)
        {
            if (effects_on_unit[i].UnitMoved())
            {
                effects_on_unit[i].RemoveThisEffect();
                effects_on_unit.RemoveAt(i);
            }
        }
        /*
        foreach (Effect effect in effects_on_unit)
        {
            if (effect.UnitMoved())
            {
                remove_effects.Add(effect);
            }
        }
        RemoveEffects();*/
    }
    // Called at the start of the turn to remove time sensitive effects
    public void TurnStartEffects()
    {
        // Iterate through list backwards, so elements we remove don't change our ordering
        for (int i = effects_on_unit.Count - 1; i >= 0; i--)
        {
            if (effects_on_unit[i].TurnStart())
            {
                effects_on_unit[i].RemoveThisEffect();
                effects_on_unit.RemoveAt(i);
            }
        }
        /*
        foreach (Effect effect in effects_on_unit)
        {
            if (effect.TurnStart())
            {
                remove_effects.Add(effect);
            }
        }
        RemoveEffects();*/
    }
    // Removes effects in the remove_effects list
    public void RemoveEffects()
    {
        while (remove_effects.Count > 0)
        {
            Effect effect = remove_effects[0];
            effects_on_unit.Remove(effect);
        }

        remove_effects.Clear();
    }


    public void Attack(Unit victim, bool attack_is_counterable)
    {
        if (victim != this 
            && !this.has_attacked 
            && this.active 
            && this.owner.IsEnemy(victim)
            && HexMap.hex_map.InRange(this.location, victim.location, attack_range))
        {
            victim.TakeHit(this, attack_is_counterable);
            has_attacked = true;
            active = false;
        }
    }
    public void CounterAttack(Unit victim)
    {
        if (victim != this
            && this.owner.IsEnemy(victim)
            && HexMap.hex_map.InRange(this.location, victim.location, attack_range))
        {
            victim.TakeHit(this, false);
        }
    }
    public float TakeHit(Unit attacker, bool attack_is_counterable)
    {
        int modified_damage = (int) CalculateDamage(attacker);
        health -= (int) modified_damage;
        Debug.Log(u_name + " took " + modified_damage + " damaged, " + health + " HP remaining from " + attacker.u_name);

        PlayerInterface.player_interface.CreateFloatingText(this.transform.position, modified_damage + "", false, 3.0f);

        if (health <= 0)
            Die();

        // Check if we can counter attack
        if (attack_is_counterable && counter_attacks && IsFacing(attacker) && HexMap.hex_map.InRange(this.location, attacker.location, this.GetRange()))
        {
            Debug.Log(u_name + " counterattacking " + attacker.u_name);
            CounterAttack(attacker);
        }

        PlayerInterface.player_interface.RefreshUnitStatsPanel();

        return health;
    }

    // Returns how much damage the attacker would do to this unit
    public float CalculateDamage(Unit attacker)
    {
        // Have the damage be the percentage of the health remaining of this unit. Weak units don't do as much damage
        float raw_normal_damage = attacker.GetDamage() * (attacker.GetHealth() / attacker.GetMaxHealth());
        float raw_piercing_damage = attacker.GetPiercingDamage() * (attacker.GetHealth() / attacker.GetMaxHealth());
        
        // Modify the damage by the defence percentage. 10% defence means 90% of the damage is inflicted.
        float modified_normal_damage = raw_normal_damage - (raw_normal_damage * this.GetDefence());

        // Piercing damage is not affected by the enemy's defense.
        // Every attack does a minimum of 1 damage.
        float final_damage = Mathf.Max(1, modified_normal_damage + raw_piercing_damage);
        return final_damage;
    }
}
