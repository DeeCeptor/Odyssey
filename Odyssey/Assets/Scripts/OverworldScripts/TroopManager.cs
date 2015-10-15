using UnityEngine;
using System.Collections;

public class TroopManager : MonoBehaviour {
    public int totalTroops = 100;
    public int healthyHoplites = 20;
    public int woundedHoplites = 0;
    public int healthySwordsmen = 20;
    public int woundedSwordsmen = 0;
    public int healthyArchers = 10;
    public int woundedArchers = 0;
    public int healthyCavalry = 10;
    public int woundedCavalry = 0;
    public GameObject[] heroes;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
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
		
		HeroStats curHero;
		
		for(int i = 0; i<heroes.Length; i++)
		{
			curHero = heroes[i].GetComponent<HeroStats>();
			consumption = consumption + (curHero.waterConsumption);
		}
		return consumption;
	}
	
	// get methods for getting highest skill level of all heroes
	public int getLeadership()
	{
		int max=0;
		HeroStats curHero;
		for(int i = 0; i<heroes.Length; i++)
		{
			curHero = heroes[i].GetComponent<HeroStats>();
			if(curHero.leadership > max)
			{
				max = curHero.leadership;
			}
		}
		return max;
	}
	
	public int getEngineering()
	{
		int max=0;
		HeroStats curHero;
		for(int i = 0; i<heroes.Length; i++)
		{
			curHero = heroes[i].GetComponent<HeroStats>();
			if(curHero.engineering > max)
			{
				max = curHero.engineering;
			}
		}
		return max;
	}
	
	public int getScavenging()
	{
		int max=0;
		HeroStats curHero;
		for(int i = 0; i<heroes.Length; i++)
		{
			curHero = heroes[i].GetComponent<HeroStats>();
			if(curHero.scavenging > max)
			{
				max = curHero.scavenging;
			}
		}
		return max;
	}
	
	public int getSeamanship()
	{
		int max=0;
		HeroStats curHero;
		for(int i = 0; i<heroes.Length; i++)
		{
			curHero = heroes[i].GetComponent<HeroStats>();
			if(curHero.seamanship > max)
			{
				max = curHero.seamanship;
			}
		}
		return max;
	}
	
	public int getDiplomacy()
	{
		int max=0;
		HeroStats curHero;
		for(int i = 0; i<heroes.Length; i++)
		{
			curHero = heroes[i].GetComponent<HeroStats>();
			if(curHero.diplomacy > max)
			{
				max = curHero.diplomacy;
			}
		}
		return max;
	}
	
	public int getPiety()
	{
		int max=0;
		HeroStats curHero;
		for(int i = 0; i<heroes.Length; i++)
		{
			curHero = heroes[i].GetComponent<HeroStats>();
			if(curHero.piety > max)
			{
				max = curHero.piety;
			}
		}
		return max;
	}
	
	public int getCunning()
	{
		int max=0;
		HeroStats curHero;
		for(int i = 0; i<heroes.Length; i++)
		{
			curHero = heroes[i].GetComponent<HeroStats>();
			if(curHero.cunning > max)
			{
				max = curHero.cunning;
			}
		}
		return max;
	}
	
	public int getMedicene()
	{
		int max=0;
		HeroStats curHero;
		for(int i = 0; i<heroes.Length; i++)
		{
			curHero = heroes[i].GetComponent<HeroStats>();
			if(curHero.medicene > max)
			{
				max = curHero.medicene;
			}
		}
		return max;
	}
	
	public int getStrength()
	{
		int max=0;
		HeroStats curHero;
		for(int i = 0; i<heroes.Length; i++)
		{
			curHero = heroes[i].GetComponent<HeroStats>();
			if(curHero.strength > max)
			{
				max = curHero.strength;
			}
		}
		return max;
	}
	
	public int getHaggling()
	{
		int max=0;
		HeroStats curHero;
		for(int i = 0; i<heroes.Length; i++)
		{
			curHero = heroes[i].GetComponent<HeroStats>();
			if(curHero.haggling > max)
			{
				max = curHero.haggling;
			}
		}
		return max;
	}
	
	public int getAnimalHandling()
	{
		int max=0;
		HeroStats curHero;
		for(int i = 0; i<heroes.Length; i++)
		{
			curHero = heroes[i].GetComponent<HeroStats>();
			if(curHero.animalHandling > max)
			{
				max = curHero.animalHandling;
			}
		}
		return max;
	}
	
	public int getSpear()
	{
		int max=0;
		HeroStats curHero;
		for(int i = 0; i<heroes.Length; i++)
		{
			curHero = heroes[i].GetComponent<HeroStats>();
			if(curHero.spear > max)
			{
				max = curHero.spear;
			}
		}
		return max;
	}
	
	public int getSword()
	{
		int max=0;
		HeroStats curHero;
		for(int i = 0; i<heroes.Length; i++)
		{
			curHero = heroes[i].GetComponent<HeroStats>();
			if(curHero.sword > max)
			{
				max = curHero.sword;
			}
		}
		return max;
	}
	
	public int getBow()
	{
		int max=0;
		HeroStats curHero;
		for(int i = 0; i<heroes.Length; i++)
		{
			curHero = heroes[i].GetComponent<HeroStats>();
			if(curHero.bow > max)
			{
				max = curHero.bow;
			}
		}
		return max;
	}

    public void InjureRandom()
    {
        
    }
	
	
	
	
	
	
	
	
	
	
	
	
	
	
}
