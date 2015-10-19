﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;

// Class that controls the factions, turns and units
public class BattleManager : MonoBehaviour
{
    [HideInInspector]
    public static BattleManager battle_manager;
    public GameObject universal_battle_parent;

    public bool pre_battle_deployment = false;
    public int round_number = 0;     // How long has this battle been going
    public bool human_turn = false;     // Is the human playing?
    public Faction player_faction;

    public Faction current_player;     // Whose turn it currently is
    Queue<Faction> players_waiting_for_turn = new Queue<Faction>();     // FIFO queue showing what player's are waiting to do their turn this round
    public List<Faction> factions = new List<Faction>();   // The sides that are fighting in this fight

    void Awake()
    {
        battle_manager = this;
    }

    void Start()
    {
        StartCoroutine(InitializePreBattleDeployment());
    }


    // Called when the player needs to deploy units
    IEnumerator InitializePreBattleDeployment()
    {
        Debug.Log("Initializing prebattle deployment");

        yield return new WaitForSeconds(0.2f);
        pre_battle_deployment = true;

        // Set deployment interfaces
        PlayerInterface.player_interface.deployment_canvas.SetActive(true);
        PlayerInterface.player_interface.battle_specific_objects.SetActive(false);

        // Undarken hexes we can deploy on
        HexMap.hex_map.UndarkenDeploymentHexes();

        Faction player_team = new Faction("Player", true, 1, Color.green);
        factions.Add(player_team);
        PreBattleDeployment.pre_battle_deployment.player_faction = player_team;
        player_faction = player_team;
    }

    IEnumerator InitializeBattle()
    {
        Debug.Log("Initializing battle");

        // Hide the deployment canvas
        yield return new WaitForSeconds(0.01f);
        pre_battle_deployment = false;

        // Change interfaces
        PlayerInterface.player_interface.deployment_canvas.SetActive(false);
        PlayerInterface.player_interface.battle_specific_objects.SetActive(true);

        // Destroy the pre battle deployment objects
        Destroy(PreBattleDeployment.pre_battle_deployment.gameObject);

        // Undarken all hexes
        HexMap.hex_map.UndarkenAllHexes();

        // Set up the enemy AI faction
        Faction enemy_team = new Faction("Enemies", false, 2, Color.red);
        factions.Add(enemy_team);

        // Spawn enemies placed on the map designated in the text file
        SpawnUnitsPlacedOnMap();

        // Set enemies. Everyone is an enemy of everyone currently
        foreach (Faction faction_1 in factions)
        {
            foreach (Faction faction_2 in factions)
            {
                if (faction_1 != faction_2)
                    faction_1.enemies.Add(faction_2);
            }
        }

        // All factions created, populate battle statistics
        PersistentBattleSettings.battle_settings.PopulateBattleStatistics();

        // Start the game
        StartRound();
    }


    // Place all the units specified in the battle text file onto the map
    public void SpawnUnitsPlacedOnMap()
    {
        foreach(PotentialUnit unit in HexMap.hex_map.parser.units_to_be_spawned)
        {
            Debug.Log("Spawning unit at " + unit.position);
            Hex pos = HexMap.hex_map.GetHexFromTopDownCoordinates(new Vector2(unit.position.x, (int)unit.position.y));
            GameObject new_unit = SpawnUnit(GetFaction(unit.faction_name), "Battles/Units/" + unit.unit_name, (int)pos.coordinate.x, (int)pos.coordinate.y, false);
            new_unit.GetComponent<Unit>().Initialize();
            new_unit.GetComponent<Unit>().SetImmediateRotation(90);
        }
    }


    public void StartRound()
    {
        // Set the FIFO queue for the turn order of players this round
        foreach (Faction faction in factions)
        {
            players_waiting_for_turn.Enqueue(faction);
        }

        current_player = players_waiting_for_turn.Dequeue();
        round_number++;

        StartTurn();
    }


    public void StartTurn()
    {
        foreach (Unit unit in current_player.units)
        {
            unit.StartTurn();
        }

        SetUnitsMovableTiles();
        human_turn = (current_player.human_controlled && !current_player.use_ai);
        PlayerInterface.player_interface.turn_text.text = current_player.faction_name + ", Round " + round_number;

        if (!current_player.human_controlled || current_player.use_ai)
        {
            Do_AI_Turn();
        }
        else
        {
            PlayerInterface.player_interface.end_turn_button.interactable = true;
            PlayerInterface.player_interface.AI_turn_button.interactable = true;
        }
    }
    // AI's turn, regardless if this is player or enemy AI
    public void Do_AI_Turn()
    {
        // AI player. Let the AI play for them
        PlayerInterface.player_interface.UnhighlightHexes();
        PlayerInterface.player_interface.UnitDeselected();
        PlayerInterface.player_interface.end_turn_button.interactable = false;
        PlayerInterface.player_interface.AI_turn_button.interactable = false;
        PlayerInterface.player_interface.HideEstimatedDamagePanel();
        StartCoroutine(Wait_For_AI_Turn_To_End());
    }
    IEnumerator Wait_For_AI_Turn_To_End()
    {
        Debug.ClearDeveloperConsole();
        current_player.GetAI().Do_Turn();

        // Wait for the AI turn to finish
        while (!current_player.GetAI().done_AI_turn)
            yield return new WaitForSeconds(0.3f);

        // AI turn is over, end it
        EndTurn();
    }
    public void Do_Player_AI_Turn()
    {
        current_player.use_ai = true;
        Do_AI_Turn();
    }


    public void EndTurn()
    {
        if (PlayerInterface.player_interface.CanSelect())
        {
            // Only let the player use the AI one turn at a time
            if (current_player.human_controlled && current_player.use_ai)
                current_player.use_ai = false;

            PlayerInterface.player_interface.UnhighlightHexes();
            PlayerInterface.player_interface.UnitDeselected();

            foreach (Unit unit in current_player.units)
            {
                unit.EndTurn();
            }

            if (!CheckVictoryAndDefeat())
            {
                if (players_waiting_for_turn.Count > 0)
                {
                    // Set the next player's turn
                    current_player = players_waiting_for_turn.Dequeue();
                    StartTurn();
                }
                else
                {
                    // Everyone's had a turn. Start a new round
                    StartRound();
                }
            }
            else
                return;
        }
    }


    // Called after any unit moves, resets what tiles each unit can move to
    public void SetUnitsMovableTiles()
    {
        HexMap.hex_map.ResetEdgeScores();

        // Update every units movable hexes
        foreach (Faction faction in factions)
        {
            foreach (Unit unit in faction.units)
            {
                // Reset the effects of everything
                unit.EvaluateEffects();

                unit.location.SetZoneOfControl(unit);
            }
        }

        foreach (Faction faction in factions)
        {
            foreach (Unit unit in faction.units)
            {
                // Get all the hexes within range
                unit.tiles_I_can_move_to = HexMap.hex_map.GetMovableHexesWithinRange(unit.location, unit.GetMovement(), unit);
            }
        }
    }
    public void SetMovableTilesOfUnit(Unit unit)
    {
        unit.tiles_I_can_move_to = HexMap.hex_map.GetMovableHexesWithinRange(unit.location, unit.GetMovement(), unit);
    }


    // Drag and drop is for the player deployment so they can drag and drop units around the map
    public GameObject SpawnUnit(Faction owning_faction, string unit_prefab, Hex hex, bool add_drag_drop)
    {
        return SpawnUnit(owning_faction, unit_prefab, (int)hex.coordinate.x, (int)hex.coordinate.y, add_drag_drop);
    }
    public GameObject SpawnUnit(Faction owning_faction, string unit_prefab, int x, int y, bool add_drag_drop)
    {
        GameObject instance = Instantiate(Resources.Load(unit_prefab, typeof(GameObject))) as GameObject;
        Unit unit = instance.GetComponent<Unit>();
        unit.owner = owning_faction;
        owning_faction.units.Add(unit);
        instance.transform.parent = this.universal_battle_parent.transform;

        // Spawn the unit in a non impassasble and non occupied space
        HexMap.hex_map.WarpUnitTo(unit, HexMap.hex_map.Nearest_Unoccupied_Passable_Hex(HexMap.hex_map.GetHex(x, y)));

        if (add_drag_drop)
            instance.AddComponent<UnitDragDrop>();

        return instance;
    }


    public Faction GetFaction(string name)
    {
        foreach(Faction faction in factions)
        {
            if (faction.faction_name == name)
                return faction;
        }

        Debug.Log("Couldn't find faction " + name + ", returning first faction " + factions[0].faction_name);
        return factions[0];
    }


    // If a player has no more units, the fight's over.
    // Return true if the game is over, false otherwise
    public bool CheckVictoryAndDefeat()
    {
        if (!pre_battle_deployment)
        {
            foreach (Faction faction in factions)
            {
                if (faction.units.Count <= 0)
                {
                    Debug.Log(faction.faction_name + " has been defeated");
                    if (faction.human_controlled)
                        Defeat();
                    else
                        Victory();

                    // Common stuff that happens regardless of who won
                    PlayerInterface.player_interface.ShowSummaryScreen();

                    return true;
                }
            }
        }
        return false;
    }

    public void Defeat()
    {
        Debug.Log("Player defeated");
        PlayerInterface.player_interface.summary_screen_title.text = "Defeat";
        PersistentBattleSettings.battle_settings.victory = false;
    }
    public void Victory()
    {
        Debug.Log("Player victory");
        PlayerInterface.player_interface.summary_screen_title.text = "Victory";
        PersistentBattleSettings.battle_settings.victory = true;
    }
    public void LoadOverworld()
    {
        Debug.Log("Loading Overworld");
        StartCoroutine(LoadingScreenToWorld());

        
    }
    IEnumerator LoadingScreenToWorld()
    {
        MoveUI.transition_UI.TransitionOut();

        // Wait until graphic fully covers screen
        while (!MoveUI.transition_UI.finished)
            yield return new WaitForSeconds(0.1f);

        // Set overworld object as active
        EventManagement.gameController.EndBattle();
        MoveUI.transition_UI.TransitionIn();
        Debug.Log("Destroying battle scene");

        // Destroy the battle scene objects
        Destroy(universal_battle_parent);
    }

    public void EndPreBattleDeployment()
    {
        // Remove drag and drop script from all units
        foreach (Faction faction in factions)
        {
            foreach (Unit unit in faction.units)
            {
                Destroy(unit.GetComponent<UnitDragDrop>());
            }
        }

        StartCoroutine(InitializeBattle());
    }
    // Takes the saved player units from the pre battle deployment screen and deploys them
    public void SpawnPlayerDeployedUnits()
    {
        PlayerUnitPositions positions = GameObject.Find("PlayerUnitPositions").GetComponent<PlayerUnitPositions>();

        foreach (Faction faction in positions.saved_factions)
        {
            factions.Add(faction);
            Debug.Log("Loading faction ");
            foreach (Unit unit in positions.player_deployed_units)
            {
                HexMap.hex_map.WarpUnitTo(unit, HexMap.hex_map.GetHex((int)unit.location_coordinates.x, (int)unit.location_coordinates.y));
                Debug.Log("Loading " + unit.u_name);
            }
        }
    }


    void Update () 
	{
	
	}
}
