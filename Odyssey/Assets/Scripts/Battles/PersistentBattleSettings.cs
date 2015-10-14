using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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
    public int number_of_deployable_units;  // How many guys can we bring to this brawl? Leadership will affect this

    // POST BATTLE INFORMATION
    public bool victory;    // Did we win?
    public bool hero_went_down; // Did our hero die? Perhaps used to write different after-battle reports.

    // Overall statistics
    // Accessed by using the faction's ID in the arrays below. Player faction is always 1. Enemy faction is 2. 0 is unused.
    public int[] units_lost;   // Full squads who were killed who belonged to this faction
    public int[] individuals_lost; // Ex: Each squad has 5 individuals, say in the battle we killed 38 guys, from 8 squads. 38 individuals killed. 1 hero = 1 individual
    public int[] units_retreated;
    public int[] individuals_retreated;  
    
    // Specific units statistics
    public Dictionary<string, int>[] specific_individuals_lost;


    void Awake ()
    {
        // If there's already a battle_settings, delete it. There should only be one battle settings
        if (battle_settings != null)
            Destroy(battle_settings.gameObject);

        battle_settings = this;
    }


    void Start ()
    {
        DontDestroyOnLoad(this.gameObject);
	}


    // Called after all factions have been created
    public void PopulateBattleStatistics()
    {
        units_lost = new int[BattleManager.battle_manager.factions.Count + 1];
        individuals_lost = new int[BattleManager.battle_manager.factions.Count + 1];
        units_retreated = new int[BattleManager.battle_manager.factions.Count + 1];
        individuals_retreated = new int[BattleManager.battle_manager.factions.Count + 1];

        specific_individuals_lost = new Dictionary<string, int>[BattleManager.battle_manager.factions.Count + 1];
    }
}
