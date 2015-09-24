using UnityEngine;
using System.Collections.Generic;
using System.Collections;

// Class that controls the factions, turns and units
public class BattleManager : MonoBehaviour
{
    [HideInInspector]
    public static BattleManager battle_manager;
    public bool pre_battle_deployment = false;
    public int round_number = 0;     // How long has this battle been going
    public bool human_turn = false;     // Is the human playing?

    public Faction current_player;     // Whose turn it currently is
    Queue<Faction> players_waiting_for_turn = new Queue<Faction>();     // FIFO queue showing what player's are waiting to do their turn this round
    List<Faction> factions = new List<Faction>();   // The sides that are fighting in this fight

    AIController AI;


    void Start()
    {
        battle_manager = this;
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize()
    {
        yield return new WaitForSeconds(0.5f);

    
        // Set units to inactive if this is a pre battle deployment
        if (pre_battle_deployment)
        {
            Faction player_team = new Faction("Player", true, 1);
            factions.Add(player_team);

            // Place units and add them to the faction unit list
            GameObject instance = Instantiate(Resources.Load("Battles/Units/Hoplite", typeof(GameObject))) as GameObject;
            Unit unit = instance.GetComponent<Unit>();
            unit.u_name = "P1";
            unit.owner = player_team;
            HexMap.hex_map.WarpUnitTo(unit, HexMap.hex_map.GetHex(0, 0));
            player_team.units.Add(unit);

            instance = Instantiate(Resources.Load("Battles/Units/Hoplite", typeof(GameObject))) as GameObject;
            unit = instance.GetComponent<Unit>();
            unit.u_name = "P2";
            unit.owner = player_team;
            HexMap.hex_map.WarpUnitTo(unit, HexMap.hex_map.GetHex(1, 0));
            player_team.units.Add(unit);

            instance = Instantiate(Resources.Load("Battles/Units/Hoplite", typeof(GameObject))) as GameObject;
            unit = instance.GetComponent<Unit>();
            unit.u_name = "P3";
            unit.owner = player_team;
            HexMap.hex_map.WarpUnitTo(unit, HexMap.hex_map.GetHex(1, 1));
            player_team.units.Add(unit);

            instance = Instantiate(Resources.Load("Battles/Units/Hoplite", typeof(GameObject))) as GameObject;
            unit = instance.GetComponent<Unit>();
            unit.u_name = "P4";
            unit.owner = player_team;
            HexMap.hex_map.WarpUnitTo(unit, HexMap.hex_map.GetHex(0, 1));
            player_team.units.Add(unit);

            // Make the units draggable
            foreach (Faction faction in factions)
            {
                foreach (Unit _unit in faction.units)
                {
                    _unit.active = false;
                    _unit.gameObject.AddComponent<UnitDragDrop>();
                }
            }
        }
        // Regular battle
        else
        {
            
            SpawnPlayerDeployedUnits(); // Also gets the faction




            Faction player_team = new Faction("Player", true, 1);
            factions.Add(player_team);

            // Place units and add them to the faction unit list
            GameObject instance = Instantiate(Resources.Load("Battles/Units/Hoplite", typeof(GameObject))) as GameObject;
            Unit unit = instance.GetComponent<Unit>();
            unit.u_name = "P1";
            unit.owner = player_team;
            HexMap.hex_map.WarpUnitTo(unit, HexMap.hex_map.GetHex(0, 0));
            player_team.units.Add(unit);

            instance = Instantiate(Resources.Load("Battles/Units/Hoplite", typeof(GameObject))) as GameObject;
            unit = instance.GetComponent<Unit>();
            unit.u_name = "P2";
            unit.owner = player_team;
            HexMap.hex_map.WarpUnitTo(unit, HexMap.hex_map.GetHex(1, 0));
            player_team.units.Add(unit);

            instance = Instantiate(Resources.Load("Battles/Units/Hoplite", typeof(GameObject))) as GameObject;
            unit = instance.GetComponent<Unit>();
            unit.u_name = "P3";
            unit.owner = player_team;
            HexMap.hex_map.WarpUnitTo(unit, HexMap.hex_map.GetHex(1, 1));
            player_team.units.Add(unit);

            instance = Instantiate(Resources.Load("Battles/Units/Hoplite", typeof(GameObject))) as GameObject;
            unit = instance.GetComponent<Unit>();
            unit.u_name = "P4";
            unit.owner = player_team;
            HexMap.hex_map.WarpUnitTo(unit, HexMap.hex_map.GetHex(0, 1));
            player_team.units.Add(unit);




            Faction enemy_team = new Faction("Enemies", false, 2);
            AI = new AIController(enemy_team);
            factions.Add(enemy_team);

             instance = Instantiate(Resources.Load("Battles/Units/Hoplite", typeof(GameObject))) as GameObject;
            Unit unit2 = instance.GetComponent<Unit>();
            unit2.owner = enemy_team;
            unit2.u_name = "E1";
            HexMap.hex_map.WarpUnitTo(unit2, HexMap.hex_map.GetHex(2, 2));
            enemy_team.units.Add(unit2);

            instance = Instantiate(Resources.Load("Battles/Units/Hoplite", typeof(GameObject))) as GameObject;
            Unit unit4 = instance.GetComponent<Unit>();
            unit4.u_name = "E2";
            unit4.owner = enemy_team;
            HexMap.hex_map.WarpUnitTo(unit4, HexMap.hex_map.GetHex(3, 2));
            enemy_team.units.Add(unit4);


            // Set enemies. Everyone is an enemy of everyone currently
            /*
            foreach (Faction faction_1 in factions)
            {
                foreach (Faction faction_2 in factions)
                {
                    if (faction_1 != faction_2)
                        faction_1.enemies.Add(faction_2);
                }
            }*/
            player_team.enemies.Add(enemy_team);
            enemy_team.enemies.Add(player_team);
            
            StartRound();
        }

        yield return null;
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
        SetUnitsMovableTiles();

        StartTurn();
    }


    public void StartTurn()
    {
        foreach (Unit unit in current_player.units)
        {
            unit.StartTurn();
        }

        human_turn = current_player.human_controlled;
        PlayerInterface.player_interface.turn_text.text = current_player.faction_name + ", Round " + round_number;

        if (!current_player.human_controlled)
        {
            // AI player. Let the AI play for them
            AI.Do_Turn();
        }
    }


    public void EndTurn()
    {
        if (PlayerInterface.player_interface.CanSelect())
        {
            foreach (Unit unit in current_player.units)
            {
                unit.EndTurn();
            }

            if (players_waiting_for_turn.Count > 0)
            {
                // Set the next player's turn
                current_player = players_waiting_for_turn.Dequeue();
                StartTurn();
            }
            else
            {
                // Everyone's had a turn. Start a new round
                CheckVictoryAndDefeat();
                StartRound();
            }
        }
    }


    // Called after any unit moves, resets what tiles each unit can move to
    public void SetUnitsMovableTiles()
    {
        HexMap.hex_map.ResetEdgeScores();

        foreach (Faction faction in factions)
        {
            foreach (Unit unit in faction.units)
            {
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


    // If a player has no more units, the fight's over
    public void CheckVictoryAndDefeat()
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
            }
        }
    }

    public void Defeat()
    {
        Debug.Log("Player defeated");
        PlayerInterface.player_interface.summary_screen_title.text = "Defeat";
        PlayerInterface.player_interface.ShowSummaryScreen();
    }
    public void Victory()
    {
        Debug.Log("Player victory");
        PlayerInterface.player_interface.summary_screen_title.text = "Victory";
        PlayerInterface.player_interface.ShowSummaryScreen();
    }


    public void EndPreBattleDeployment()
    {
        // Record the players unit positions and facings
        PlayerUnitPositions positions = GameObject.Find("PlayerUnitPositions").GetComponent<PlayerUnitPositions>();
        positions.player_deployed_units.Clear();
        positions.saved_factions.Clear();

        foreach(Faction faction in factions)
        {
            positions.saved_factions.Add(faction);

            foreach(Unit unit in faction.units)
            {
                positions.player_deployed_units.Add(unit);
            }
        }

        Application.LoadLevel("TacticalBattle");
    }
    public void SpawnPlayerDeployedUnits()
    {

    }


    void Update () 
	{
	
	}
}
