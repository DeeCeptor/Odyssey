using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerInterface : MonoBehaviour 
{
    public static PlayerInterface player_interface;

    public GameObject pause_menu;
    public GameObject summary_screen;
    public Text summary_screen_title;

    public Text turn_text;

    public GameObject unit_panel;
    public Text unit_name;
    public Text unit_description;
    public Text health_text;
    public Slider health_bar;
    public Text defence_text;
    public Slider defence_bar;
    public Text ranged_defence_text;
    public Slider ranged_defence_bar;

    public GameObject terrain_panel;
    public Text terrain_name;
    public Text terrain_description;


    [HideInInspector]
    public Unit selected_unit;
    [HideInInspector]
    public Hex highlighted_hex;

    private bool can_select = true;
    private bool is_rotating_unit = false;

	void Start () 
	{
        player_interface = this;

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
        ShowUnitStatsPanel(unit);

        if (unit.owner == BattleManager.battle_manager.current_player)
        {
            selected_unit = unit;

            unit.unit_menu.SetActive(true);
        }

        UnhighlightHexes();

        unit.HighlightHexesWeCanMoveTo();
    }


    // Dehighlights any hexes that may have been highlighted
    public void UnitDeselected()
    {
        HideUnitStatsPanel();
        UnhighlightHexes();

        if (selected_unit != null)
            selected_unit.unit_menu.SetActive(false);
        selected_unit = null;
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
        defence_text.text = "Defence: " + unit.GetDefence();
        defence_bar.value = unit.GetDefence();
    }
    public void SetRangedDefenceText(Unit unit)
    {
        ranged_defence_text.text = "Ranged Defence: " + unit.GetDefence();
        ranged_defence_bar.value = unit.GetDefence();
    }

    public bool SelectedUnitAvailableToControl()
    {
        return (selected_unit != null
            && selected_unit.IsControllable());
    }


    public void UnhighlightHexes()
    {
        foreach (Hex hex in HexMap.hex_map.all_hexes)
        {
            hex.UnhighlightHex();
        }
    }


    public void MousedOverHex(Hex hex)
    {
        if (highlighted_hex != hex && !hex.IsHighlighted())
        {
            if (highlighted_hex != null)
                highlighted_hex.UnMouseHighlight();
            highlighted_hex = hex;
            highlighted_hex.MouseHighlight();
        }

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


    public void CreateFloatingText(Vector3 position, string text, bool random_velocity, float time_to_die)
    {
        GameObject instance = Instantiate(Resources.Load("FloatingText", typeof(GameObject))) as GameObject;
        position.z = -6;
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
        summary_screen.SetActive(true);
    }
}
