using UnityEngine;
using System.Collections;

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
    public int number_of_deployable_units;

    // POST BATTLE INFORMATION



	void Start ()
    {
        // If there's already a battle_settings, delete it. There should only be one battle settings
        if (battle_settings != null)
            Destroy(battle_settings.gameObject);

        battle_settings = this;
        DontDestroyOnLoad(this.gameObject);
	}
	

}
