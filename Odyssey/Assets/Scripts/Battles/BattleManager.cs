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

            for (int x = -1; x <= 2; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    SpawnUnit(player_team, "Battles/Units/BlueHoplite", x, y);
                }
            }
            /*
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
            */
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



            /*
            Faction player_team = new Faction("Player", true, 1);
            factions.Add(player_team);
            */

            Faction enemy_team = new Faction("Enemies", false, 2);
            AI = new AIController(enemy_team);
            factions.Add(enemy_team);

            for (int x = -4; x < 5; x++)
            {
                SpawnUnit(enemy_team, "Battles/Units/Hoplite", x, 6);
            }
            for (int x = -4; x < 5; x++)
            {
                SpawnUnit(enemy_team, "Battles/Units/Hoplite", x, -6);
            }


            // Set enemies. Everyone is an enemy of everyone currently

            foreach (Faction faction_1 in factions)
            {
                foreach (Faction faction_2 in factions)
                {
                    if (faction_1 != faction_2)
                        faction_1.enemies.Add(faction_2);
                }
            }
            //player_team.enemies.Add(enemy_team);
            //enemy_team.enemies.Add(player_team);
            
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

        StartTurn();
    }


    public void StartTurn()
    {
        foreach (Unit unit in current_player.units)
        {
            unit.StartTurn();
        }

        SetUnitsMovableTiles();
        human_turn = current_player.human_controlled;
        PlayerInterface.player_interface.turn_text.text = current_player.faction_name + ", Round " + round_number;

        if (!current_player.human_controlled)
        {
            // AI player. Let the AI play for them
            StartCoroutine(Wait_For_AI_Turn_To_End());
        }
    }
    IEnumerator Wait_For_AI_Turn_To_End()
    {
        AI.Do_Turn();

        // Wait for the AI turn to finish
        while (!AI.done_AI_turn)
            yield return new WaitForSeconds(0.3f);

        // AI turn is over, end it
        EndTurn();
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

        if (!current_player.human_controlled)
        {
            // AI player only needs to update their own units movable hexes
            foreach(Unit unit in current_player.units)
            {
                unit.tiles_I_can_move_to = HexMap.hex_map.GetMovableHexesWithinRange(unit.location, unit.GetMovement(), unit);
            }
        }
        else
        {
            // This is a human player, so update every units movable hexes
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
    }


    public void SpawnUnit(Faction owning_faction, string unit_prefab, int x, int y)
    {
        GameObject instance = Instantiate(Resources.Load(unit_prefab, typeof(GameObject))) as GameObject;
        Unit unit2 = instance.GetComponent<Unit>();
        unit2.owner = owning_faction;
        unit2.u_name = "E1";
        HexMap.hex_map.WarpUnitTo(unit2, HexMap.hex_map.GetHex(x, y));
        owning_faction.units.Add(unit2);
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
                DontDestroyOnLoad(unit.gameObject);
                Debug.Log("Saving " + unit.u_name);
            }
        }

        Application.LoadLevel("TacticalBattle");
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
