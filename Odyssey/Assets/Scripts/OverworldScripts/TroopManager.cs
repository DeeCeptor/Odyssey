using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TroopManager : MonoBehaviour {
    public int totalTroops = 100;
    public int startingHoplite = 20;
    public int startingSwordsman = 20;
    public int startingArcher = 10;
    public int startingCavalry = 10;
    public int startingSlinger = 10;
    public Dictionary<string, int> healthy = new Dictionary<string, int>();
    public Dictionary<string, int> wounded = new Dictionary<string, int>();
    public GameObject[] heroes;
    public static TroopManager playerTroops;
    public int[] troopNums;
    // Use this for initialization
    void Start () {
        playerTroops = this;
        healthy.Add("Hoplite",startingHoplite);
        healthy.Add("Archer", startingArcher);
        healthy.Add("Swordsman", startingSwordsman);
        healthy.Add("Cavalry", startingCavalry);
        healthy.Add("Slinger", startingSlinger);

        wounded.Add("Hoplite", 0);
        wounded.Add("Archer", 0);
        wounded.Add("Swordsman", 0);
        wounded.Add("Cavalry", 0);
        wounded.Add("Slinger", 0);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public int getTroopNum()
    {
        int troops = 0;
        Dictionary<string,int>.ValueCollection healthyUnits = healthy.Values;
        healthyUnits.CopyTo(troopNums,0);
        Dictionary<string, int>.ValueCollection woundedUnits = wounded.Values;
        woundedUnits.CopyTo(troopNums, troopNums.Length);
        for(int i = 0; i < troopNums.Length;i++)
        {
            troops = troops + troopNums[i];
        }
        return troops;
    }

    public int getWoundedTroopNum()
    {
        int troops = 0;
        Dictionary<string, int>.ValueCollection woundedUnits = wounded.Values;
        woundedUnits.CopyTo(troopNums, 0);
        for (int i = 0; i < troopNums.Length; i++)
        {
            troops = troops + troopNums[i];
        }
        return troops;
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

    public void InjureRandom(int numToInjure)
    {
        int troops = 0;
        int[] healthyTroopNums;
        int[] woundedTroopNums;
        string[] keyArray;
        string curKey;
        Dictionary<string, int>.ValueCollection healthyUnits = healthy.Values;
        healthyTroopNums = new int[healthyUnits.Count];
        healthyUnits.CopyTo(healthyTroopNums, 0);
        Dictionary<string, int>.ValueCollection woundedUnits = wounded.Values;
        woundedTroopNums = new int[woundedUnits.Count];
        woundedUnits.CopyTo(woundedTroopNums, troopNums.Length);
        Dictionary<string, int>.KeyCollection keys = healthy.Keys;
        keyArray = new string[keys.Count];
        keys.CopyTo(keyArray, troopNums.Length);
        troops = getTroopNum();
        //if there are troops to injure

        //for each injury
        for (int i = 0; i < numToInjure; i++)
        {
            troops = getTroopNum();
            if (troops > 0)
            {
                
                //find a random number corresponding to an individual troop
                int randInt = Random.Range(0, troops);
                //find which unit that number belongs to
                int numsSearched = 0;
                for (int x = 0; x < healthyTroopNums.Length + woundedTroopNums.Length; x++)
                {
                    if (x < healthyTroopNums.Length)
                    {
                        numsSearched = numsSearched + healthyTroopNums[x];
                        if (randInt < numsSearched)
                        {
                            healthyTroopNums[x] = healthyTroopNums[x] - 1;
                            woundedTroopNums[x] = woundedTroopNums[x] + 1;
                            curKey = keyArray[x];
                            healthy[curKey] = healthy[curKey] - 1;
                            wounded[curKey] = wounded[curKey] + 1;
                        }
                    }
                    else
                    {
                        //if it wounds a wounded troop it kills it
                        numsSearched = numsSearched + woundedTroopNums[x];
                        if (randInt < numsSearched)
                        {
                            woundedTroopNums[x] = woundedTroopNums[x] - 1;
                            curKey = keyArray[x];
                            wounded[curKey] = wounded[curKey] - 1;
                        }
                    }
                }
            }
        }
    }

    public void healRandom(int numToHeal)
    {
        int troops = 0;
        int[] woundedTroopNums = new int[] { };
        string[] keyArray = new string[] { };
        string curKey;
        Dictionary<string, int>.ValueCollection woundedUnits = wounded.Values;
        woundedUnits.CopyTo(woundedTroopNums, troopNums.Length);
        Dictionary<string, int>.KeyCollection keys = healthy.Keys;
        keys.CopyTo(keyArray, 0);
        troops = getWoundedTroopNum();
        for (int i = 0; i < numToHeal; i++)
        {
            troops = getWoundedTroopNum();
            if (troops > 0)
            {
                int randInt = Random.Range(0, troops);
                //find which unit that number belongs to
                int numsSearched = 0;
                for (int x = 0; x < woundedTroopNums.Length; x++)
                {
                    numsSearched = numsSearched + woundedTroopNums[x];
                    if (randInt < numsSearched)
                    {
                        woundedTroopNums[x] = woundedTroopNums[x] - 1;
                        curKey = keyArray[x];
                        wounded[curKey] = wounded[curKey] - 1;
                    }
                }
            }
        }
    }
}
