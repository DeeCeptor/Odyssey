using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PreBattleDeployment : MonoBehaviour
{
    [HideInInspector]
    public static PreBattleDeployment pre_battle_deployment;

    public Dictionary<string, int> deployable_units = new Dictionary<string, int>();
    public int cur_deployed_units = 0;
    public int maximum_deployable_units = 10;

    public Dictionary<string, HeroStats> deployable_heroes = new Dictionary<string, HeroStats>();   // Contains the stats of all deployable heroes

    public string unit_to_spawn;        // String that contains the location of the prefab to spawn
    public string unit_to_spawn_name;
    public GameObject deployable_sprite;    // Sprite that follows the mouse to show the unit we're going to spawn
    public GameObject deploy_unit_button;   // Button to populate our deploy units list
    public Transform deployable_panel;      // Panel that gets populated with buttons to deploy units
    public Text units_remaining_text;
    [HideInInspector]
    public Faction player_faction;


    void Start ()
    {
        pre_battle_deployment = this;

        // Look at the persistent battle settings for how many units we can deploy
        if (PersistentBattleSettings.battle_settings != null)
            maximum_deployable_units = PersistentBattleSettings.battle_settings.number_of_deployable_units;

        // Get available units from the troop manager
        if (TroopManager.playerTroops != null)
        {
            Debug.Log("Getting units from troop manager");

            // Add units we can deploy to the deployment panel
            foreach (KeyValuePair<string, int> pair in TroopManager.playerTroops.healthy)
            {
                if (pair.Value > 0)
                    deployable_units.Add(pair.Key, pair.Value / 10);
            }

            // Add available non-wounded heroes to be deployed
            foreach (GameObject hero_obj in TroopManager.playerTroops.heroes)
            {
                // Get the stats from the hero stats script
                HeroStats hero_stats = hero_obj.GetComponent<HeroStats>();
                if (!hero_stats.injured)
                {
                    Debug.Log("Adding hero to deployment list: " + hero_stats.hero_name);
                    deployable_units.Add(hero_stats.hero_name, 1);
                    deployable_heroes.Add(hero_stats.hero_name, hero_stats);
                }
            }
        }
        else
        {
            // Setting up debug available units
            Debug.Log("Setting available debug units");

            deployable_units.Add("Hoplite", 4);
            deployable_units.Add("Cavalry", 4);
            deployable_units.Add("Archer", 4);
            deployable_units.Add("Slinger", 4);
            deployable_units.Add("Swordsman", 4);
        }


        // Populate the buttons to spawn units
        foreach (KeyValuePair<string, int> pair in deployable_units)
        {
            GameObject newButton = Instantiate(deploy_unit_button) as GameObject;
            newButton.name = pair.Key;
            Button button = newButton.GetComponent<Button>();
            Debug.Log(pair.Key + " x " + pair.Value);
            string unit_name = pair.Key;
            button.onClick.AddListener(() => SelectUnitFromDeploymentMenu(unit_name));
            Text text = newButton.GetComponentInChildren<Text>();
            text.text = pair.Key + " x " + pair.Value;
            newButton.transform.SetParent(deployable_panel);
            newButton.transform.localScale = new Vector3(1, 1, 1);

            if (pair.Value <= 0)
                button.interactable = false;
        }

        SetUnitsRemainingText();
    }


    void Update ()
    {
	    if (unit_to_spawn != "")
        {
            // If we have a unit to spawn have the sprite follow the mouse
            deployable_sprite.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Left click to deploy the unit
            if (Input.GetMouseButtonDown(0)
                && !EventSystem.current.IsPointerOverGameObject()
                && PlayerInterface.player_interface.highlighted_hex != null 
                && PlayerInterface.player_interface.highlighted_hex.occupying_unit == null
                && PlayerInterface.player_interface.highlighted_hex.deployment_zone
                && cur_deployed_units < maximum_deployable_units
                && !PlayerInterface.player_interface.highlighted_hex.impassable)
            {
                DeployUnit();
            }
        }

        // Right clicking on a deployed unit un deploys it
        if (Input.GetMouseButtonDown(1)
            && !EventSystem.current.IsPointerOverGameObject()
            && PlayerInterface.player_interface.highlighted_hex != null
            && PlayerInterface.player_interface.highlighted_hex.occupying_unit != null)
        {
            UndeployUnit();
        }
    }
    public void DeployUnit()
    {
        int remaining_units = deployable_units[unit_to_spawn_name];
        if (remaining_units > 0)    // Spawn the unit if we have units available to spawn
        {
            Debug.Log("Spawning unit " + unit_to_spawn_name);
            deployable_units[unit_to_spawn_name] = deployable_units[unit_to_spawn_name] - 1;
            cur_deployed_units++;
            SetUnitsRemainingText();
            SetDeployButtonText(unit_to_spawn_name, deployable_units[unit_to_spawn_name]);
            GameObject unit = BattleManager.battle_manager.SpawnUnit(player_faction, unit_to_spawn, PlayerInterface.player_interface.highlighted_hex, true);
            Unit unit_script = unit.GetComponent<Unit>();
            unit_script.Initialize();

            // If there are viewable enemies on the field, face towards the nearest
            List<Unit> enemies = BattleManager.battle_manager.player_faction.GetAllEnemyUnits();
            if (enemies.Count > 0)
            {
                unit_script.SetImmediateRotation(
                    unit_script.GetAngleTowards(unit_script.location.world_coordinates,
                    BattleManager.battle_manager.player_faction.GetClosestEnemy(unit_script).location.world_coordinates));
            }
            else
                unit_script.SetImmediateRotation(270);

            // Check if this is a hero
            HeroStats hero_stats;
            if (deployable_heroes.TryGetValue(unit_to_spawn_name, out hero_stats))
            {
                Debug.Log("Adding stats and ability for hero " + hero_stats.hero_name);

                switch (hero_stats.weaponType)
                {
                    case Hero_Weapon_Skill_Types.Ranged:
                        AddRangedHeroAbilities(unit_script, hero_stats);
                        break;
                }

                AlterHeroStats(unit_script, hero_stats);

                unit_script.ResetStats();
            }

            // Disable the deployment of that unit if we're out of those units to deploy
            if (deployable_units[unit_to_spawn_name] <= 0)
            {
                deployable_panel.FindChild(unit_to_spawn_name).gameObject.GetComponent<Button>().interactable = false;
                unit_to_spawn = "";
                unit_to_spawn_name = "";
            }
        }
    }
    public void UndeployUnit()
    {
        cur_deployed_units--;
        SetUnitsRemainingText();
        string name = PlayerInterface.player_interface.highlighted_hex.occupying_unit.u_name;
        Debug.Log("Undeploying " + name);

        deployable_units[name] = deployable_units[name] + 1;
        SetDeployButtonText(name, deployable_units[name]);

        // Re enable deployment button of this unit we just deleted
        if (deployable_units[name] == 1)
        {
            deployable_panel.FindChild(name).gameObject.GetComponent<Button>().interactable = true;
        }

        PlayerInterface.player_interface.highlighted_hex.occupying_unit.RemoveUnit();
    }


    public void SetDeployButtonText(string unit_name, int number_available)
    {
        deployable_panel.FindChild(unit_name).GetComponentInChildren<Text>().text = unit_name + " x " + number_available;
    }


    public void SetUnitsRemainingText()
    {
        units_remaining_text.text = (maximum_deployable_units - cur_deployed_units) + " unit deployments remaining";
    }


    public void SelectUnitFromDeploymentMenu(string unit_name)
    {
        Debug.Log("Selected " + unit_name + " to deploy");
        unit_to_spawn = "Battles/Units/" + unit_name;
        unit_to_spawn_name = unit_name;

        // Activate and set deployable sprite
        deployable_sprite.SetActive(true);
        deployable_sprite.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    // Based on the heroes stats, alter the units stats
    public void AlterHeroStats(Unit hero, HeroStats hero_stats)
    {
        hero.maximum_health += hero_stats.strength * 30;    // 30 HP per strength
    }


    // Adds an ability per level of bow skill
    public void AddRangedHeroAbilities(Unit hero, HeroStats hero_stats)
    {
        for(int weapon_level = 1; weapon_level < hero_stats.bow; weapon_level++)
        {
            switch (weapon_level)
            {
                case 1:
                    hero.abilities.Add(new ShotOfLegends(hero));
                    break;
                case 2:
                    hero.normal_attack_range++;
                    break;
                case 3:
                    hero.counter_attacks = true;
                    break;
                case 4:
                    hero.abilities.Add(new PiercingShot(hero));
                    break;
                case 5:
                    // Speed shooting. Not implemented
                    break;
            }
        }
    }
}
