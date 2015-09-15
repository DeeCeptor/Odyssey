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
        unit.owner = player_team;
        HexMap.hex_map.WarpUnitTo(unit, HexMap.hex_map.GetHex(1,1));
        player_team.units.Add(unit);

        instance = Instantiate(Resources.Load("Battles/Units/Hoplite", typeof(GameObject))) as GameObject;
        unit = instance.GetComponent<Unit>();
        unit.owner = player_team;
        HexMap.hex_map.WarpUnitTo(unit, HexMap.hex_map.GetHex(1, 2));
        player_team.units.Add(unit);




        Faction enemy_team = new Faction("Enemies", true);
        factions.Add(enemy_team);

        instance = Instantiate(Resources.Load("Battles/Units/Hoplite", typeof(GameObject))) as GameObject;
        unit = instance.GetComponent<Unit>();
        unit.owner = enemy_team;
        HexMap.hex_map.WarpUnitTo(unit, HexMap.hex_map.GetHex(5, 2));
        enemy_team.units.Add(unit);

        instance = Instantiate(Resources.Load("Battles/Units/Hoplite", typeof(GameObject))) as GameObject;
        unit = instance.GetComponent<Unit>();
        unit.owner = enemy_team;
        HexMap.hex_map.WarpUnitTo(unit, HexMap.hex_map.GetHex(5, 1));
        enemy_team.units.Add(unit);


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
