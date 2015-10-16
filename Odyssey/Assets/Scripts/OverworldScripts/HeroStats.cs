using UnityEngine;
using System.Collections;

public enum Hero_Weapon_Skill_Types { Ranged, Melee_Defensive, Melee_Offensive };

public class HeroStats : MonoBehaviour
{
    // Defaults set for Odysseus, since he's the first hero
    public string hero_name = "Odysseus";    // Same name as the prefab to spawn
    public bool injured = false;
	public int hitpoints = 10;
	public float foodConsumption = 0.005f;
	public float waterConsumption = 0.01f;
	public GameObject weapon;
	public GameObject armour;
	public GameObject accessory;
	public int leadership = 0;
	public int engineering = 0;
	public int scavenging = 0;
	public int seamanship = 0;
	public int diplomacy = 0;
	public int piety = 0;
	public int cunning = 0;
	public int medicene = 0;
	public int strength = 0;    // Used in tests and adds HP per level
	public int haggling = 0;
	public int animalHandling = 0;
	public int spear = 0;
	public int sword = 0;
	public int bow = 0;
    public Hero_Weapon_Skill_Types weaponType = Hero_Weapon_Skill_Types.Ranged;     // For the demo at least, each hero is limited to 1 type of wapon they used, and gain abilities based on that category
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
