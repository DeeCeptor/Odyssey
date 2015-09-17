using UnityEngine;
using System.Collections;

public class TroopManager : MonoBehaviour {
public int totalTroops = 100;
public GameObject[] units;
public GameObject[] heroes;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void AddTroop(GameObject unit)
	{
		unit.transform.SetParent(transform);
		units[units.Length] = unit;
	}
	
	public void AddHero(GameObject hero)
	{
		hero.transform.SetParent(transform);
		heroes[heroes.Length] = hero;
	}
	
	public float getFoodConsumption()
	{
		float consumption = 0f;
		TroopStats curTroop;
	
		for(int i = 0; i<units.Length; i++)
		{
			curTroop = units[i].GetComponent<TroopStats>();
			consumption = consumption + (curTroop.foodConsumptionPerSoldier*curTroop.totalLiving);
		}
	
	
		HeroStats curHero;
		
		for(int i = 0; i<heroes.Length; i++)
		{
			curHero = heroes[i].GetComponent<HeroStats>();
			consumption = consumption + (curHero.foodConsumption);
		}
		return consumption;
	}
	
	public float getWaterConsumption()
	{
		float consumption = 0f;
		TroopStats curTroop;
		
		for(int i = 0; i<units.Length; i++)
		{
			curTroop = units[i].GetComponent<TroopStats>();
			consumption = consumption + (curTroop.waterConsumptionPerSoldier*curTroop.totalLiving);
		}
		
		
		HeroStats curHero;
		
		for(int i = 0; i<heroes.Length; i++)
		{
			curHero = heroes[i].GetComponent<HeroStats>();
			consumption = consumption + (curHero.waterConsumption);
		}
		return consumption;
	}
	
	
}
