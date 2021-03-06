﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Casualty
{
    public string name;
    public int num_wounded = 0;
    public int num_killed = 0;
}

// Class that contains pre battle information, and will be populated with post battle information after it is done.
// Create this object before the battle, load the battle level, when the battle is finished we will load back the overworld and can analyze the battle results.
// This object persists between level loads. Destroy this object when done analyzing post battle info.
// Ensure only of these objects ever exist.
public class PersistentBattleSettings : MonoBehaviour
{
    public static PersistentBattleSettings battle_settings;

    // PRE BATTLE SETTINGS
    public string path_to_battle_file;  // Ex: /Resources/Battles/LevelFiles/Level1.txt
    public bool can_retreat = true;
    public bool must_include_main_hero = false;
    public bool player_goes_first = true;   // If the player is ambusdhing the enemy or has an advantage, check this so the player goes first and has an advantage
    public bool show_enemy_units_in_deployment = false;     // If the player is ambusdhing the enemy or has an advantage, check this so the player has maximum information when deploying
    public int number_of_deployable_units;  // How many guys can we bring to this brawl? Leadership will affect this
    public float enemy_agressiveness = 1;   // 1 means agressive. 0 means defensive. Set to whether or not the player is defending or attacking

    // POST BATTLE INFORMATION
    public bool victory;    // Did we win?
	public bool game_over = false;	// Did this battle result in all units being casualties while Odysseus was deployed?
	public bool hero_went_down = false; // Did our hero die? Perhaps used to write different after-battle reports.

    // Overall statistics
    // Accessed by using the faction's ID in the arrays below. Player faction is always 1. Enemy faction is 2. 0 is unused.
    public int[] units_lost;   // Full squads who were killed who belonged to this faction
    public int[] individuals_lost; // Ex: Each squad has 5 individuals, say in the battle we killed 38 guys, from 8 squads. 38 individuals killed. 1 hero = 1 individual
    public int[] units_retreated;
    public int[] individuals_retreated;  
    
    // Specific units statistics
    // Accessed by faction ID and u_name
    public Dictionary<string, Casualty>[] casualties;


    void Awake ()
    {
        // If there's already a battle_settings, delete it. There should only be one battle settings
        if (PersistentBattleSettings.battle_settings != null)
        {
            Debug.Log("PersistentBattleSettings already exist. Destroy this script");
            this.gameObject.SetActive(false);
            //DestroyImmediate(this.gameObject);
            return;
        }
        else
        {
            battle_settings = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }


    // Called after all factions have been created
    public void PopulateBattleStatistics()
    {
        units_lost = new int[BattleManager.battle_manager.factions.Count + 1];
        individuals_lost = new int[BattleManager.battle_manager.factions.Count + 1];
        units_retreated = new int[BattleManager.battle_manager.factions.Count + 1];
        individuals_retreated = new int[BattleManager.battle_manager.factions.Count + 1];

        casualties = new Dictionary<string, Casualty>[BattleManager.battle_manager.factions.Count + 1];

        for (int x = 0; x < casualties.Length; x++)
            casualties[x] = new Dictionary<string, Casualty>();
    }


    public string GetProperPathToFile()
    {
        return "/Resources/Battles/LevelFiles/" + path_to_battle_file;
    }
}
