using UnityEngine;
using System.Collections.Generic;
using System.Collections;

// Class that controls the factions, turns and units
public class BattleManager : MonoBehaviour
{
    public static BattleManager battle_manager;
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
        yield return new WaitForSeconds(1); 

        Faction player_team = new Faction("Player", true);
        factions.Add(player_team);

        // Place units and add them to the faction unit list
        GameObject instance = Instantiate(Resources.Load("Battles/Units/Hoplite", typeof(GameObject))) as GameObject;
        Unit unit = instance.GetComponent<Unit>();
        unit.u_name = "P1";
        unit.owner = player_team;
        HexMap.hex_map.WarpUnitTo(unit, HexMap.hex_map.GetHex(1,1));
        player_team.units.Add(unit);

        instance = Instantiate(Resources.Load("Battles/Units/Hoplite", typeof(GameObject))) as GameObject;
        Unit unit3 = instance.GetComponent<Unit>();
        unit3.u_name = "P2";
        unit3.owner = player_team;
        HexMap.hex_map.WarpUnitTo(unit3, HexMap.hex_map.GetHex(2, 5));
        player_team.units.Add(unit3);




        Faction enemy_team = new Faction("Enemies", false);
        AI = new AIController(enemy_team);
        factions.Add(enemy_team);

        instance = Instantiate(Resources.Load("Battles/Units/Hoplite", typeof(GameObject))) as GameObject;
        Unit unit2 = instance.GetComponent<Unit>();
        unit2.owner = enemy_team;
        unit2.u_name = "E1";
        HexMap.hex_map.WarpUnitTo(unit2, HexMap.hex_map.GetHex(5, 4));
        enemy_team.units.Add(unit2);

        instance = Instantiate(Resources.Load("Battles/Units/Hoplite", typeof(GameObject))) as GameObject;
        Unit unit4 = instance.GetComponent<Unit>();
        unit4.u_name = "E2";
        unit4.owner = enemy_team;
        HexMap.hex_map.WarpUnitTo(unit4, HexMap.hex_map.GetHex(6, 10));
        enemy_team.units.Add(unit4);


        // Set enemies
        player_team.enemies.Add(enemy_team);
        enemy_team.enemies.Add(player_team);


        StartRound();

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


    // Called after any unit moves, resets what tiles each unit can move to
    public void SetUnitsMovableTiles()
    {
        foreach (Faction faction in factions)
        {
            foreach (Unit unit in faction.units)
            {
                // Get all the hexes within range
                unit.tiles_I_can_move_to = HexMap.hex_map.GetMovableHexesWithinRange(unit.location, unit.GetMovement());
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
            }
        }
    }

	
	void Update () 
	{
	
	}
}
