using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

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

    // Effects alter the stats of a unit, and can persist for a while. Either number of turns, permanent or from terrain
    public List<Effect> effects_on_unit = new List<Effect>();

    // Abilities appear in the units menu when you click on them and it's your turn.
    // Abilities must be ACTIVATED by the user to be used. They are never passive. Passives are effects.
    public List<Ability> abilities = new List<Ability>();

    // UNIT STATS
    // Normal stats are the base stats of the unit.
    // The non-normal stats are the current stats of the unit
    public float maximum_health = 200;
    float health = 200;
    public float normal_defence = 0.3f;     // Defense is a percentage, from 0 to 1 of how much damage is blocked
    float defence = 0.3f;
    public float normal_ranged_defence = 0.3f;      // Specific 0 to 1 % blocked from ranged attacks
    float ranged_defence = 0.3f;
    public int normal_attack_range = 1;     // 1 is adjacent. 0 would mean unable to attack
    int attack_range = 1;
    public float normal_damage = 80;     // Damage is blocked by defence
    float damage = 80;
    public float normal_piercing_damage = 20;    // Piercing damage ignores defence, making this stat extremely valuable
    float piercing_damage = 20;
    public int normal_movement = 3;     // How far this unit can move in a turn.
    int movement = 3;
    public float normal_flanking_bonus = 0; // Bonus percentage damage we get from flanking a unit
    float flanking_bonus;
    public float normal_bonus_vs_melee = 0;     // Bonus percentage of overall damage we get against melee
    float bonus_vs_melee = 0;
    public float normal_bonus_vs_cavalry = 0;
    float bonus_vs_cavalry = 0;
    public float normal_bonus_vs_ranged = 0;
    float bonus_vs_ranged = 0;

    public Unit_Types unit_type;    // Melee, ranged or cavalry. All units of these categories
    public bool is_ranged_unit = false;
    public bool counter_attacks = true;     // Counterattacks if the enemy is within range and in the frontal facing arc
    public int counter_attack_radius = 60;  // The difference in facing counter attacks can be done from. 60 means the front 3 hexes
    public bool attacks_are_counterable = true;


    // AI scores used to determine how this unit should move
    public float offensive_AI_score = 1;  // If this is an AI unit, this value indicates how agressively we should advance towards the enemy
    public float flanking_factor = 1.5f;    // Damage multiplied by this factor when flanking, to show that we're not taking damage
    public float ally_grouping_score = 0.1f;

    // Which direction the unit is facing. Hexagons have 6 facings. 360/6 = 60. This value will be a multiple of 6
    //[HideInInspector]
    public int facing = 0;
    [HideInInspector]
    public bool rotating = false;
    private float tile_move_speed = 7f;
    private bool desired_rotation_set = false;

    public bool hero = false;
    public bool is_squad = true;        // Heroes and some myth units are not squads
    public int normal_squad_size = 5;   // Size of squads doesn't affect stats at all. Only used for recording casualties
    public int remaining_individuals = 5;   // Heroes and some myth units only ever have 1 guy

    public string u_name = ""; // Name at the top of the unit panel
    public string u_description = "";  // Short description of the unit
    public Texture portrait;    // Unit portrait
    public string prefab_name;  // Exact name needed to load the prefab
    [HideInInspector]
    public GameObject unit_menu;
    public GameObject unit_sprite;

    public Faction owner; 


	void Start ()
    {
        Initialize();

        remaining_individuals = normal_squad_size;

        //this.SetRotation(new Vector3(0, 0, 0));
        health = maximum_health;

        // Set aura so we can tell which faction this player belongs to
        this.transform.FindChild("PlayerAura").GetComponent<SpriteRenderer>().color = this.owner.faction_color;

        //ResetStats();
        AssignAbilities();
    }


    public void Initialize()
    {
        unit_menu = this.transform.FindChild("UnitMenu").gameObject;
        unit_sprite = this.transform.FindChild("UnitSprite").gameObject;
    }


    // Override this in the super class
    public virtual void AssignAbilities()
    {

    }


	void Update ()
    {
        if (desired_rotation_set)
        {
            unit_sprite.transform.eulerAngles = new Vector3(0, 0, facing);
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
            SetDesiredRotationTowards(this.transform.position, mousePos);
        }
    }


    public void SetDesiredRotationTowards(Vector3 from, Vector3 towards)
    {
        // Use euler angles so we're dealing with degrees 0-360
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, towards - from);
        Vector3 angles = rotation.eulerAngles;
        int angle = (int) angles.z;

        SetRotation(angle);
        desired_rotation_set = true;
    }
    public void SetRotation(int facing)
    {
        this.facing = GetHexagonalDirection(facing);
        desired_rotation_set = true;
    }
    public void SetImmediateRotation(int facing)
    {
        this.facing = GetHexagonalDirection(facing);
        unit_sprite.transform.eulerAngles = new Vector3(0, 0, facing);
    }
    // Snaps the given direction to one of 6 hexagonal facings
    public int GetHexagonalDirection(int facing)
    {
        int angle = 0;
        if (facing >= 0 && facing < 60)
            angle = 30;
        else if (facing >= 60 && facing < 120)
            angle = 90;
        else if (facing >= 120 && facing < 180)
            angle = 150;
        else if (facing >= 180 && facing < 240)
            angle = 210;
        else if (facing >= 240 && facing < 300)
            angle = 270;
        else if (facing >= 300 && facing < 360)
            angle = 330;
        return angle;
    }


    // Called when the current owner's turn starts
    public virtual void StartTurn()
    {
        active = true;
        has_attacked = false;
        has_moved = false;

        if (owner.human_controlled)
            ready_to_be_controlled = true;

        TurnStartEffects();
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
        return IsFacing(unit.location);
    }
    // Is the unit facing this hex? (Used for counter attacking)
    public bool IsFacing(Hex hex)
    {
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, hex.world_coordinates - this.location.world_coordinates);//this.transform.position);
        int angle_towards_unit = (int) rotation.eulerAngles.z;
        int angle_diff = (int)Mathf.Abs(((angle_towards_unit - this.facing + 180) % 360 - 180));    // Do math to get the difference in this units facing and the direction towards the enemy

        // Go from 0 and 360, so we wrap around
        return (Mathf.Abs(angle_diff) <= counter_attack_radius + 5
            || Mathf.Abs(angle_diff) >= 360 - counter_attack_radius - 5);
    }


    public virtual void Die()
    {
        // Add 1 favour since we killed an enemy
        if (this.owner.IsEnemy(BattleManager.battle_manager.player_faction))
        {
            GodsManager.gods_manager.ModifyFavour(1);
        }

        dead = true;

        // Change status to having retreated
        PersistentBattleSettings.battle_settings.units_lost[this.owner.faction_ID]++;

        RemoveUnit();

        // Check victory/defeat conditions
        BattleManager.battle_manager.CheckVictoryAndDefeat();
    }
    public virtual void RemoveUnit()
    {
        if (this.transform.Find("UnitMenuCanvas") != null)
        {
            Debug.Log("Removing parent");
            PlayerInterface.player_interface.unit_menu_canvas.transform.parent = null;  // Remove parent, so we don't destroy this game object
            PlayerInterface.player_interface.UnitDeselected();
        }

        // Remove from the unit lists
        this.owner.units.Remove(this);

        this.location.occupying_unit = null;

        BattleManager.battle_manager.SetUnitsMovableTiles();

        // Remove game object
        Destroy(this.gameObject);
    }
    public void RetreatUnit()
    {
        Debug.Log("Retreating unit " + u_name);

        // Change status to having retreated
        PersistentBattleSettings.battle_settings.units_retreated[this.owner.faction_ID]++;
        PersistentBattleSettings.battle_settings.individuals_retreated[this.owner.faction_ID] += remaining_individuals;

        // Remove from play
        RemoveUnit();

        BattleManager.battle_manager.CheckVictoryAndDefeat();
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
        PlayerInterface.player_interface.ShowTerrainStatsPanel(this.location);
        PlayerInterface.player_interface.ShowUnitStatsPanel(this);
    }
    void OnMouseExit()
    {
        if (PlayerInterface.player_interface.selected_unit != null)// && PlayerInterface.player_interface.selected_unit != this)
        {
            PlayerInterface.player_interface.ShowUnitStatsPanel(PlayerInterface.player_interface.selected_unit);
        }
        else
        {
            PlayerInterface.player_interface.HideUnitStatsPanel();
        }

        PlayerInterface.player_interface.HideEstimatedDamagePanel();
    }


    // Attack unit if we right clicked on it and we have another unit selected
    void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject()   // Make sure mouse is not over UI
            && PlayerInterface.player_interface.SelectedUnitAvailableToControl()
            && !PlayerInterface.player_interface.selected_unit.has_attacked
            && PlayerInterface.player_interface.selected_unit.owner.IsEnemy(this))
        {
            PlayerInterface.player_interface.ShowEstimatedDamagePanel(this);

            if (Input.GetMouseButtonDown(1))     // Right clicked on unit
            {
                Debug.Log("OnMouseOver attack");
                PlayerInterface.player_interface.selected_unit.HumanAttacked(this, attacks_are_counterable);
            }
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
            hex.HighlightMoveHex();
        }
    }


    public void HexClicked(Hex hex)
    {
        if (this.owner == BattleManager.battle_manager.current_player 
            && BattleManager.battle_manager.current_player.human_controlled
            && this.active )
        {
            // Check if that's a valid spot. Can't have more than one unit sit on the same spot, can't move to the spot we're already on
            if (hex.occupying_unit == null && hex != location && !this.has_moved)
            {
                PlayerInterface.player_interface.UnhighlightHexes();
                HumanMovedTo(hex);
            }
            else if (hex.occupying_unit != null && this.owner.IsEnemy(hex.occupying_unit) && !this.has_attacked)
            {
                Debug.Log("HexClicked attack");
                HumanAttacked(hex.occupying_unit, attacks_are_counterable);
            }
        }
    }

    public void HumanMovedTo(Hex to)
    {
        PathTo(to);
        PlayerInterface.player_interface.ReevaluateCastableAbilities(this);
    }
    public void PathTo(Hex to)
    {
        // Check if that's a valid spot. Can't have more than one unit sit on the same spot, can't move to the spot we're already on
        if (to.occupying_unit == null && to != this.location)
        {
            // Pathfind to the correct spot
            if (this.movement_path.Count == 0)
            {
                this.movement_path = HexMap.hex_map.AStarFindPath(this.location, to, movement, this.owner);
                if (movement_path.Count > 0)
                {
                    SetLocation(to);
                    has_moved = true;
                    is_moving = true;

                    BattleManager.battle_manager.SetUnitsMovableTiles();
                }
            }
        }
    } 


    public void SetLocation(Hex hex)
    {
        // Remove the effects from this 
        UnitMovedChangeEffects();
        if (this.location != null)
        {
            this.location.occupying_unit = null;
        }
        this.location = hex;
        hex.occupying_unit = this;
        this.location_coordinates = new Vector2(hex.coordinate.x, hex.coordinate.y);

        // Get the effects on this hex
        GetHexEffects(hex);
    }
    // Add the bonuses conferred by the hex to this unit
    public void GetHexEffects(Hex hex)
    {
        effects_on_unit.AddRange(hex.GetEffectsOnHex(this));
        EvaluateEffects();
    }


    public void AddEffectToUnit(Effect effect)
    {
        effects_on_unit.Add(effect);
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
        flanking_bonus = normal_flanking_bonus;
        bonus_vs_melee = normal_bonus_vs_melee;
        bonus_vs_ranged = normal_bonus_vs_ranged;
        bonus_vs_cavalry = normal_bonus_vs_cavalry;
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

        EvaluateEffects();
    }
    public void ResetAndApplyEffects()
    {
        EvaluateEffects();
    }


    public void HumanAttacked(Unit victim, bool attacks_are_counterable)
    {
        Attack(victim, attacks_are_counterable);
        PlayerInterface.player_interface.ReevaluateCastableAbilities(this);
        PlayerInterface.player_interface.HideEstimatedDamagePanel();
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
        int prev_remaining_individuals = remaining_individuals;

        int modified_damage = (int) CalculateDamage(attacker, attacker.location);
        health -= (int) modified_damage;
        Debug.Log(u_name + " took " + modified_damage + " damage, " + " Flanking: " + !IsFacing(attacker) + ", " + GetHealth() + " HP remaining from " + attacker.u_name);

        // Show floating damage text
        PlayerInterface.player_interface.CreateFloatingText(this.transform.position, "<i>" + modified_damage + "</i>", true, 3.0f);

        if (health <= 0)
        {
            // We died, removed all the individuals left
            PersistentBattleSettings.battle_settings.individuals_lost[this.owner.faction_ID] += remaining_individuals;

            // Record casualties in troop manager
            if (TroopManager.playerTroops != null && owner == BattleManager.battle_manager.player_faction)
            {
                TroopManager.playerTroops.healthy[prefab_name] -= remaining_individuals;
                Debug.Log("Remaining " + prefab_name + ": " + TroopManager.playerTroops.healthy[prefab_name]);
            }
            remaining_individuals = 0;

            Die();
        }
        else
        {
            if (is_squad)
            {
                // We didn't die, calculate how many individuals we lost
                int HP_per_individual = (int) GetMaxHealth() / normal_squad_size;
                remaining_individuals = ((int) GetHealth() / HP_per_individual) + 1;
                PersistentBattleSettings.battle_settings.individuals_lost[this.owner.faction_ID] += (prev_remaining_individuals - remaining_individuals);

                // Record casualties in troop manager
                if (TroopManager.playerTroops != null && owner == BattleManager.battle_manager.player_faction)
                {
                    TroopManager.playerTroops.healthy[prefab_name] -= (prev_remaining_individuals - remaining_individuals);
                    Debug.Log("Remaining " + prefab_name + ": " + TroopManager.playerTroops.healthy[prefab_name]);
                }
            }
        }

        // Check if we can counter attack
        if (!dead)
        {
            if (attack_is_counterable && counter_attacks && IsFacing(attacker) && HexMap.hex_map.InRange(this.location, attacker.location, this.GetRange()))
            {
                Debug.Log(u_name + " counterattacking " + attacker.u_name);
                CounterAttack(attacker);
            }

            PlayerInterface.player_interface.RefreshUnitStatsPanel();
        }

        return health;
    }

    // Returns how much damage the attacker would do to this unit
    public float CalculateDamage(Unit attacker, Hex from)
    {
        // Have the damage be the percentage of the health remaining of this unit. Weak units don't do as much damage
        float raw_normal_damage = attacker.GetDamage() * (attacker.GetHealth() / attacker.GetMaxHealth());
        float raw_piercing_damage = attacker.GetPiercingDamage() * (attacker.GetHealth() / attacker.GetMaxHealth());

        // Get the right type of defence to use. Ranged defence from ranged units
        float defence = 0;
        if (attacker.is_ranged_unit)
        {
            defence = this.GetRangedDefence();
        }
        else
            defence = this.GetDefence();

        // Modify the damage by the defence percentage. 10% defence means 90% of the damage is inflicted.
        float modified_normal_damage = raw_normal_damage - (raw_normal_damage * defence);

        // Piercing damage is not affected by the enemy's defense.
        float damage = modified_normal_damage + raw_piercing_damage;

        // Apply bonuses to damage that's been modified by the enemies' defence
        // Apply bonus against specific type of unit
        if (this.unit_type == Unit_Types.Melee)
            damage += damage * GetMeleeBonus();
        else if (this.unit_type == Unit_Types.Cavalry)
            damage += damage * GetMeleeBonus();
        else if (this.unit_type == Unit_Types.Ranged)
            damage += damage * GetMeleeBonus();

        // If the attacker is flanking, apply a flanking bonus
        if (!IsFacing(from))
        {
            damage += damage * GetFlankingBonus();
        }

        return damage;
    }




    // UNIT STATS AND CHANGING THEM
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
        damage += damage * percent;
        damage += constant_amount;
        damage = Mathf.Max(0, damage);  // Can't have negative damage
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
        piercing_damage += piercing_damage * percent;
        piercing_damage += constant_amount;
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

    // Percentage applied against overall damage against a target we are flanking.
    public virtual float GetFlankingBonus()
    {
        return flanking_bonus;
    }
    public virtual void AdjustFlankingBonus(float constant_amount)
    {
        flanking_bonus = Mathf.Max(0, flanking_bonus + constant_amount);
    }

    // Percentage applied against overall damage against a target that is of type melee
    public virtual float GetMeleeBonus()
    {
        return bonus_vs_melee;
    }
    public virtual void AdjustMeleeBonus(float constant_amount)
    {
        bonus_vs_melee = Mathf.Max(0, bonus_vs_melee + constant_amount);
    }

    // Percentage applied against overall damage against a target that is of type cavalry
    public virtual float GetCavalryBonus()
    {
        return bonus_vs_cavalry;
    }
    public virtual void AdjustCavalryBonus(float constant_amount)
    {
        bonus_vs_cavalry = Mathf.Max(0, bonus_vs_cavalry + constant_amount);
    }

    // Percentage applied against overall damage against a target that is of type ranged
    public virtual float GetRangedBonus()
    {
        return bonus_vs_ranged;
    }
    public virtual void AdjustRangedBonus(float constant_amount)
    {
        bonus_vs_ranged = Mathf.Max(0, bonus_vs_ranged + constant_amount);
    }
}
