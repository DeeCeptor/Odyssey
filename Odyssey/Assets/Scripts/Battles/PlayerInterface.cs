using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerInterface : MonoBehaviour 
{
    [HideInInspector]
    public static PlayerInterface player_interface;

    public GameObject deployment_canvas;
    public GameObject unit_menu_canvas;
    public GameObject ability_panel;

    public GameObject battle_specific_objects;  // Objects that don't appear when in deployment stage
    public Button end_turn_button;
    public Button AI_turn_button;
    public GameObject pause_menu;
    public GameObject summary_screen;
    public Text summary_screen_title;
	public Text summary_screen_summary;

    // Battle prediction panel
    public GameObject battle_prediction_panel;
    public Slider enemy_hp_slider;
    public Slider estimated_damage_slider;

    public Text turn_text;
    public Text favour_remaining_text;

    public GameObject unit_panel;
    public Text unit_name;
    public Text unit_description;
    public Text health_text;
    public Slider health_bar;
    public Text damage_text;
    public Slider damage_bar;
    public Text piercing_damage_text;
    public Slider piercing_damage_bar;
    public Text defence_text;
    public Slider defence_bar;
    public Text ranged_defence_text;
    public Slider ranged_defence_bar;
	public Text attack_range_text;
	public Slider attack_range_bar;
    public Text movement_text;
    public Slider movement_bar;

    public GameObject terrain_panel;
    public Text terrain_name;
    public Text terrain_description;

    public Text PlayerLosses;
    public Text EnemyLosses;

    public Unit selected_unit;
    [HideInInspector]
    public Hex highlighted_hex;

	public Color highlight_hex_movement;
	public Color highlight_non_hex_movement;
	public Color highlight_hex_attack;
	public Color highlight_non_hex_attack;

	public Color mouse_highlight_color;

    private bool can_select = true;
    private bool is_rotating_unit = false;

	void Awake () 
	{
        player_interface = this;
	}
    void Start()
    {
        unit_panel.SetActive(false);
        terrain_panel.SetActive(false);
        HideEstimatedDamagePanel();
    }
	

	void Update () 
	{
        // Check if we tried to start or end rotating a unit
	    if (Input.GetKeyDown(KeyCode.F))
        {
            if (!is_rotating_unit)
            {
                StartRotatingUnit();
            }
            else
            {
                StopRotatingUnit();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
	}


    // Call to ensure we can click on things, like units to select them.
    // Unable to click things when rotating a unit's facing or selecting an ability.
    public bool CanSelect()
    {
        return can_select;
    }
    public void StartRotatingUnit()
    {
        if (SelectedUnitAvailableToControl() && !is_rotating_unit && CanSelect())
        {
            can_select = false;
            is_rotating_unit = true;
            selected_unit.StartRotating();
        }
    }
    public void StopRotatingUnit()
    {
        if (selected_unit != null)
            selected_unit.StopRotating();

        can_select = true;
        is_rotating_unit = false;
    }


    // Player left clicked on the unit
    public void UnitSelected(Unit unit)
    {
        //UnitDeselected();
		StopRotatingUnit();
        ShowUnitStatsPanel(unit);

        if (unit.owner == BattleManager.battle_manager.current_player)
        {
            selected_unit = unit;

            //unit.unit_menu.SetActive(true);

            PopulateAbilitiesMenu(unit);

            // Check all enemies and see if they are within attack range of the selected unit
            foreach (Unit enemy in unit.owner.GetAllEnemyUnits())
            {
                if (HexMap.hex_map.InRange(unit.location, enemy.location, unit.GetRange()))
                {
                    enemy.location.HighlightAttackableHex(!unit.has_attacked && unit.active);
                }
            }
        }

        UnhighlightHexes();

        unit.HighlightHexesWeCanMoveTo(unit.has_moved);

        HighlightAttacksFrom(unit, unit.location);
    }


    // Dehighlights any hexes that may have been highlighted
    public void UnitDeselected()
    {
        this.unit_menu_canvas.SetActive(false);

        HideUnitStatsPanel();
        UnhighlightHexes();

        if (selected_unit != null)
            selected_unit.unit_menu.SetActive(false);
        selected_unit = null;
    }


    public void PopulateAbilitiesMenu(Unit unit)
    {
        // Destroy all previous ability buttons
        int childs = ability_panel.transform.childCount;
        for (int i = childs - 1; i > 0; i--)
        {
            GameObject.Destroy(ability_panel.transform.GetChild(i).gameObject);
        }

        // Populate the unit menu panel with the abilities of this unit
        for (int x = 0; x < unit.abilities.Count; x++)
        {
            Ability ability = unit.abilities[x];
            // Create a button for the ability
            GameObject newButton = Instantiate(Resources.Load("Battles/AbilityButton", typeof(GameObject))) as GameObject;
            newButton.name = ability.ability_name + "Button";
            Button button = newButton.GetComponent<Button>();
			button.image.sprite = ability.icon;
            button.onClick.AddListener(() => ability.TryToCastAbility());
            Text text = newButton.GetComponentInChildren<Text>();
            text.text = ability.ability_name;
            newButton.transform.SetParent(ability_panel.transform);
            newButton.transform.localScale = new Vector3(1, 1, 1);
			AbilityButton a_button = newButton.GetComponent<AbilityButton>();
			a_button.ability = ability;

            if (!ability.CanCastAbility())
                button.interactable = false;
        }

        this.unit_menu_canvas.transform.position = unit.transform.position;
        this.unit_menu_canvas.transform.parent = unit.transform;
        this.unit_menu_canvas.SetActive(true);
    }
    public void ReevaluateCastableAbilities(Unit unit)
    {
        if (selected_unit == unit && unit_menu_canvas.active)
        {
            PopulateAbilitiesMenu(unit);
        }
    }

    // Refreshes the units stats panel if that unit is selected.
    public void RefreshStatsPanel(Unit unit)
    {
        if (selected_unit == unit)
        {
            ShowUnitStatsPanel(selected_unit);
        }
    }


    public void ShowEstimatedDamagePanel(Unit target)
    {
        battle_prediction_panel.SetActive(true);

        // Set enemy HP, out of their max HP
        enemy_hp_slider.value = target.GetHealth() / target.GetMaxHealth();

        // Estimate the amount of damage the selected unit will do to the target
        estimated_damage_slider.value = target.CalculateDamage(selected_unit, selected_unit.location) / target.GetMaxHealth();
    }
    public void HideEstimatedDamagePanel()
    {
        battle_prediction_panel.SetActive(false);
    }


    public void ShowUnitStatsPanel(Unit unit)
    {
        unit_panel.gameObject.SetActive(true);

        // Set the unit title, description and stats
        unit_name.text = unit.u_name;
        unit_description.text = "<i>" + unit.u_description + "</i>";
        SetHealthText(unit);
        SetDefenceText(unit);
        SetRangedDefenceText(unit);
        SetDamageText(unit);
        SetPiercingDamageText(unit);
		SetRangeText(unit);
        SetMovementText(unit);
    }
    public void HideUnitStatsPanel()
    {
        unit_panel.gameObject.SetActive(false);
    }
    public void RefreshUnitStatsPanel()
    {
        if (selected_unit != null && selected_unit.GetHealth() > 0)
        {
            ShowUnitStatsPanel(selected_unit);
        }
        else
            HideUnitStatsPanel();
    }


    public void SetHealthText(Unit unit)
    {
        health_text.text = "Health: " + unit.GetHealth();
        health_bar.value = unit.GetHealth() / unit.GetMaxHealth();
    }
    public void SetDefenceText(Unit unit)
    {
        defence_text.text = "Defence: " + (int)(unit.GetDefence() * 100) + "%";
        defence_bar.value = unit.GetDefence();
    }
    public void SetRangedDefenceText(Unit unit)
    {
        ranged_defence_text.text = "Ranged Defence: " + (int)(unit.GetRangedDefence() * 100) + "%";
        ranged_defence_bar.value = unit.GetRangedDefence();
    }
    public void SetDamageText(Unit unit)
    {
        damage_text.text = "Damage: " + unit.GetDamage();
        damage_bar.value = unit.GetDamage() / 200.0f;
    }
    public void SetPiercingDamageText(Unit unit)
    {
        piercing_damage_text.text = "Piercing Damage: " + unit.GetPiercingDamage();
        piercing_damage_bar.value = unit.GetPiercingDamage() / 200.0f;
    }
	public void SetRangeText(Unit unit)
	{
		attack_range_text.text = "Attack Range: " + unit.GetRange();
		attack_range_bar.value = (float) unit.GetRange() / 5.0f;
	}
    public void SetMovementText(Unit unit)
    {
        movement_text.text = "Movement Speed: " + unit.GetMovement();
        movement_bar.value = (float) unit.GetMovement() / 10.0f;
    }

    public bool SelectedUnitAvailableToControl()
    {
        return (selected_unit != null
            && selected_unit.IsControllable());
    }


    // Removes hex aura from all hexes
    public void UnhighlightHexes()
    {
        foreach (Hex hex in HexMap.hex_map.all_hexes)
        {
            hex.UnhighlightHex();
        }
    }
    public void UnhightlightEnemyHexes()
    {
        foreach (Unit enemy in BattleManager.battle_manager.player_faction.GetAllEnemyUnits())
        {
            enemy.location.UnhighlightHex();
        }
    }


    // If the unit can attack, highlight all enemies we can hit from this hex
    public void HighlightAttacksFrom(Unit unit, Hex hex)
    {
        if (!unit.has_attacked)
        {
            foreach (Unit enemy in unit.owner.GetAllEnemyUnits())
            {
                if (HexMap.hex_map.InRange(hex, enemy.location, unit.GetRange()))
                {
                    enemy.location.HighlightAttackableHex(!unit.has_attacked && unit.active);
                }
            }
        }
    }
    public void HighlightAttacksFromUnitLocationAndFrom(Unit unit, Hex hex)
    {
        if (!unit.has_attacked)
        {
            foreach (Unit enemy in selected_unit.owner.GetAllEnemyUnits())
            {
                if (HexMap.hex_map.InRange(hex, enemy.location, unit.GetRange())
                    || HexMap.hex_map.InRange(unit.location, enemy.location, unit.GetRange()))
                {
                    enemy.location.HighlightAttackableHex(!unit.has_attacked && unit.active);
                }
            }
        }
    }


    public void MousedOverHex(Hex hex)
    {
        if (highlighted_hex != hex)// && !hex.IsHighlighted())
        {
            if (highlighted_hex != null)
                highlighted_hex.UnMouseHighlight();
            highlighted_hex = hex;
            highlighted_hex.MouseHighlight();

            UnhightlightEnemyHexes();

            // Highlight all hexes the selected can attack from this hex
            if (SelectedUnitAvailableToControl())
            {
                if (hex.IsHighlighted() || hex.occupying_unit == selected_unit)
                    HighlightAttacksFromUnitLocationAndFrom(selected_unit, hex);
                else
                    HighlightAttacksFromUnitLocationAndFrom(selected_unit, selected_unit.location);
            }
        }
        highlighted_hex = hex;

        ShowTerrainStatsPanel(hex);
    }
    public void ShowTerrainStatsPanel(Hex hex)
    {
        terrain_panel.gameObject.SetActive(true);

        // Set the hex title, description
        terrain_name.text = hex.h_name;
        terrain_description.text = "<i>" + hex.h_description + hex.coordinate + "</i>";
    }
    public void HideTerrainStatsPanel()
    {
        terrain_panel.gameObject.SetActive(false);
    }


    public GameObject CreateAttackObject(Vector3 position, Vector3 text_offset, Hex target,float velocity, string text, float time_to_die)
    {
        GameObject instance = Instantiate(Resources.Load("Battles/AttackObject", typeof(GameObject))) as GameObject;
        instance.transform.parent = BattleManager.battle_manager.universal_battle_parent.transform;
        instance.transform.position = position;
        AttackObject obj = instance.GetComponent<AttackObject>();
        obj.offset = text_offset;
        obj.speed = velocity;
        obj.string_to_display = text;
        obj.text_duration = time_to_die;
        obj.hex_to_go_towards = target;
        return instance;
    }
    public void CreateFloatingText(Vector3 position, string text, bool random_velocity, float time_to_die)
    {
        GameObject instance = Instantiate(Resources.Load("FloatingText", typeof(GameObject))) as GameObject;
        instance.transform.parent = BattleManager.battle_manager.universal_battle_parent.transform;
        position.z = -8;
        instance.transform.position = position;
        instance.GetComponent<TextMesh>().text = text;
        instance.GetComponent<Rigidbody2D>().velocity = Vector2.up;
    }


    public void TeleportUnit(Hex to)
    {
        if (selected_unit.location == null)
        {
            HexMap.hex_map.WarpUnitTo(selected_unit, to);
        }
        else if (selected_unit != null)
        {

        }
    }


    public void TogglePauseMenu()
    {
        pause_menu.SetActive(!pause_menu.activeSelf);
    }
    public void ShowSummaryScreen()
    {
        // Set the text of the casualties
        // Show the player casualties
        string text = "";
        int total_casualties = 0;
        int total_wounded = 0;
        int total_killed = 0;
        foreach (KeyValuePair<string, Casualty> entry in PersistentBattleSettings.battle_settings.casualties[BattleManager.battle_manager.player_faction.faction_ID])
        {
            Casualty casualty = entry.Value;
            text += casualty.name + "\t" + casualty.num_wounded + " wounded, " + casualty.num_killed + " dead\n";
            total_casualties += casualty.num_wounded + casualty.num_killed;
            total_killed += casualty.num_killed;
            total_wounded += casualty.num_wounded;
        }
        text += "\nTotal wounded: " + total_wounded + ", Total dead: " + total_killed + "\n";
        text += "Total casualties: " + total_casualties;
        PlayerLosses.text = text;


        // Show the enemy casualties
        text = "";
        total_casualties = 0;
        total_wounded = 0;
        total_killed = 0;
        foreach (KeyValuePair<string, Casualty> entry in PersistentBattleSettings.battle_settings.casualties[BattleManager.battle_manager.enemy_faction.faction_ID])
        {
            Casualty casualty = entry.Value;
            text += casualty.name + "\t" + casualty.num_wounded + " wounded, " + casualty.num_killed + " dead\n";
            total_casualties += casualty.num_wounded + casualty.num_killed;
            total_killed += casualty.num_killed;
            total_wounded += casualty.num_wounded;
        }
        text += "\nTotal wounded: " + total_wounded + ", Total dead: " + total_killed + "\n";
        text += "Total casualties: " + total_casualties;
        EnemyLosses.text = text;

        summary_screen.SetActive(true);
    }
}
