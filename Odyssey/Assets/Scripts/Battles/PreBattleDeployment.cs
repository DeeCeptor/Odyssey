using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PreBattleDeployment : MonoBehaviour
{
    [HideInInspector]
    public static PreBattleDeployment pre_battle_deployment;

    public Dictionary<string, int> deployable_units = new Dictionary<string, int>();
    public int cur_deployed_units = 0;
    public int maximum_deployable_units = 10;

    // String that contains the location of the prefab to spawn
    public string unit_to_spawn;
    public GameObject deployable_sprite;    // Sprite that follows the mouse to show the unit we're going to spawn
    public GameObject deploy_unit_button;   // Button to populate our deploy units list
    public Transform deployable_panel; 
    [HideInInspector]
    public Faction player_faction;


    void Start ()
    {
        pre_battle_deployment = this;

        // Add units we can deploy to the deployment panel
        deployable_units.Add("Hoplite", 6);
        deployable_units.Add("Cavalry", 3);
        deployable_units.Add("Archer", 4);

        foreach (KeyValuePair<string, int> pair in deployable_units)
        {
            GameObject newButton = Instantiate(deploy_unit_button) as GameObject;
            Button button = newButton.GetComponent<Button>();
            Debug.Log(pair.Key + " x " + pair.Value);
            string unit_name = pair.Key;
            button.onClick.AddListener(() => SelectUnitFromDeploymentMenu(unit_name));
            Text text = newButton.GetComponentInChildren<Text>();
            text.text = pair.Key + " x " + pair.Value;
            newButton.transform.SetParent(deployable_panel);
        }
    }


    void Update ()
    {
	    if (unit_to_spawn != "")
        {
            // If we have a unit to spawn have the sprite follow the mouse
            deployable_sprite.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Left click to deploy the unit
            if (Input.GetMouseButtonDown(0) 
                && PlayerInterface.player_interface.highlighted_hex != null 
                && PlayerInterface.player_interface.highlighted_hex.occupying_unit == null
                && cur_deployed_units < maximum_deployable_units)
            {
                Debug.Log("Spawning unit");
                cur_deployed_units++;
                BattleManager.battle_manager.SpawnUnit(player_faction, unit_to_spawn, PlayerInterface.player_interface.highlighted_hex);
            }
        }

        // Right clicking on a deployed unit un deploys it
        if (Input.GetMouseButtonDown(1)
            && PlayerInterface.player_interface.highlighted_hex != null
            && PlayerInterface.player_interface.highlighted_hex.occupying_unit != null)
        {
            cur_deployed_units--;
            PlayerInterface.player_interface.highlighted_hex.occupying_unit.Die();
        }
    }


    public void SelectUnitFromDeploymentMenu(string unit_name)
    {
        Debug.Log("Selected " + unit_name + " to deploy");
        unit_to_spawn = "Battles/Units/" + unit_name;

        // Activate and set deployable sprite
        deployable_sprite.SetActive(true);
        deployable_sprite.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
