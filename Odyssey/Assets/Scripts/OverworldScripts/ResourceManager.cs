using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class ResourceManager : MonoBehaviour {
//display texts for present amount
public GameObject waterText;
public GameObject foodText;
public GameObject moraleText;
public GameObject staminaText;
public GameObject sailorText;
public GameObject healthText;
public GameObject hullText;
public GameObject goldText;

public TroopManager troopManager;

public bool paused = false;
public bool anchored = false;
public bool gathering = false;

//how frequently your sailors must eat
public int framesPerConsuption = 120;
private int frameIterator = 0;

//rates of food and water consumption
public float foodPerSailorPerConsumption = 0.005f;
public float lowRationRate = 0.5f;
public float highRationRate = 1.5f;
public float waterPerSailorPerConsumption = 0.01f;

//for ration and movement rates the lowest number is the lowest amount of rations and the lowest move rate is the slowest movement rate
public int rationRate = 1;
public int moveRate = 1;

//rates of morale and stamina loss/gain
public float moraleLossFromLowRations = 0.5f;
public float moraleGainFromHighRations = 0.5f;
public float staminaLossFromHighSpeed = 1f;
public float staminaLossFromNormalSpeed = 1f;
public float staminaLossFromLowRations= 0.5f;
public float staminaGainFromLowSpeed = 0.5f;
public float staminaGainFromHighRations= 0.5f;
public float staminaGainFromAnchor = 1f;
public float staminaGainFromCamping = 2f;

//rates of health loss
public float healthLossFromStarvation = 1f;
public float healthLossFromDehydration = 3f;
public float healthLossFromLowRations = 1f;
public float healthLossFromExhaustion = 0.02f; //health loss is per each percent stamina below 50
public float healthGainNaturally = 0.5f;
public float healthGainFromSlowSpeed = 0.5f;
public float healthGainFromHighRations = 0.5f;
public float healthGainFromAnchoring = 1f;
public float healthGainFromCamping = 1f;

//amount of resources
public float food = 100f;
public float water = 100f;
public int sailors = 50;
public float stamina = 100f;
public float morale = 100f;
public float health = 100f;
public int gold = 50;
public int shipHealth = 100;
public float weight = 200f;
public float maxWeight = 500;
public CargoStats[] cargo;

//god's favours
public float poseidonsFavour = 0f;
public float zeusFavour = 0f;
public float hadesFavour = 0f;
public float athenasFavour = 0f;
public float aresFavour = 0f;

//gathering variables
//how many frames before resources
public float defaultGatheringFrames = 240;
private float gatheringCounter = 0;
public float waterToGain = 10f;
public float foodToGain = 5f;
//percent chance to get water when gathering instead of food;
public int waterPercent = 66;
//gameObject to spawn to indicate gain of water/food
public GameObject waterAquiredSymbol;
public GameObject foodAquiredSymbol;
public GameObject islandToPlunder;
public Collider[] currentWeather;
public WeatherScript weatherAtIndex;

	// Use this for initialization
	void Start () {
	waterText = GameObject.Find("WaterMonitor");
	foodText = GameObject.Find("FoodMonitor");
	moraleText = GameObject.Find("MoraleMonitor");
	sailorText = GameObject.Find("SailorsMonitor");
	staminaText = GameObject.Find("StaminaMonitor");
	healthText = GameObject.Find("HealthMonitor");
	hullText = GameObject.Find("HullMonitor");
	goldText = GameObject.Find("GoldMonitor");
	
	waterText.GetComponent<Text>().text = "water: " + water.ToString("F1") + "L";
	foodText.GetComponent<Text>().text = "food: " + food.ToString("F1") + "Kg";
	moraleText.GetComponent<Text>().text = "morale: " + morale.ToString() + "%";
	staminaText.GetComponent<Text>().text = "stamina: " + stamina.ToString() + "%";
	sailorText.GetComponent<Text>().text = "sailors: " + sailors.ToString();
	healthText.GetComponent<Text>().text = "health: " + health.ToString() + "%";
	hullText.GetComponent<Text>().text = "hull: " + shipHealth.ToString() + "%";
	goldText.GetComponent<Text>().text = "gold: " + gold.ToString();
	troopManager = GameObject.FindGameObjectWithTag("TroopManager").GetComponent<TroopManager>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	if(paused == false)
	{
		if(frameIterator>framesPerConsuption)
		{
			frameIterator = 0;
			Consume(1);
			CalcWeight();
            Weather();
		}
		if(!anchored)
		{
			gathering = false;
		}
		if(gathering)
		{
			Gather();
		}
		frameIterator = frameIterator + 1;
		waterText.GetComponent<Text>().text = "water: " + water.ToString("F1") + "L";
		foodText.GetComponent<Text>().text = "food: " + food.ToString("F1") + "Kg";
		moraleText.GetComponent<Text>().text = "morale: " + morale.ToString() + "%";
		staminaText.GetComponent<Text>().text = "stamina: " + stamina.ToString() + "%";
		sailorText.GetComponent<Text>().text = "sailors: " + sailors.ToString();
		healthText.GetComponent<Text>().text = "health: " + health.ToString() + "%";
		hullText.GetComponent<Text>().text = "hull: " + shipHealth.ToString() + "%";
		goldText.GetComponent<Text>().text = "gold: " + gold.ToString();
	}
	}

    public void Weather()
    {
        LayerMask mask = 1 << 10;
        currentWeather = Physics.OverlapSphere(transform.position, 0.1f,mask);
        for (int i = 0; i < currentWeather.Length;i++)
        {
            weatherAtIndex = currentWeather[i].gameObject.GetComponent<WeatherScript>();
            if (!anchored)
            {
                shipHealth = shipHealth - weatherAtIndex.weatherDamage;
                morale = morale - weatherAtIndex.weatherMoraleDrain;
                stamina = stamina - weatherAtIndex.weatherStaminaDrain;
            }
        }

    }
	
	public void LowRations()
	{
	    rationRate = 0;
	}
	
	public void NormalRations()
	{
		rationRate = 1;
	}
	
	public void HighRations()
	{
		rationRate = 2;
	}
	
	public void SlowSpeed()
	{
		moveRate = 0;
	}
	
	public void MediumSpeed()
	{
		moveRate = 1;
	}
	
	public void HighSpeed()
	{
		moveRate = 2;
	}
	
	public void Consume(int Consumptions)
	{
	
	if(food == 0|| water == 0)
	{
		rationRate = 0;
	}
	
	for(int i = 0;i<Consumptions;i++)
	{
	
		if(rationRate == 0)
		{
			food = food - (sailors*foodPerSailorPerConsumption*lowRationRate);
			food = food - (troopManager.getFoodConsumption()*lowRationRate);
			water = water - (sailors*waterPerSailorPerConsumption*lowRationRate);
			water = water - (troopManager.getWaterConsumption()*lowRationRate);
			morale = morale - moraleLossFromLowRations;
			stamina = stamina - staminaLossFromLowRations;
		}
		
		if(rationRate == 1)
		{
			food = food - (sailors*foodPerSailorPerConsumption);
			food = food - (troopManager.getFoodConsumption());
			water = water - (sailors*waterPerSailorPerConsumption);
			water = water - (troopManager.getWaterConsumption());
		}
		
		if(rationRate == 2)
		{
			food = food - (sailors*foodPerSailorPerConsumption*highRationRate);
			food = food - (troopManager.getFoodConsumption()*highRationRate);
			water = water - (sailors*waterPerSailorPerConsumption*highRationRate);
			water = water - (troopManager.getWaterConsumption()*highRationRate);
			morale = morale + moraleGainFromHighRations;
			stamina = stamina + staminaGainFromHighRations;
		}
		
		if(!anchored)
		{
		if(moveRate == 0)
		{
			stamina = stamina + staminaGainFromLowSpeed;
		}
		
		if(moveRate == 1)
		{
			stamina = stamina - staminaLossFromNormalSpeed;
		}
		
		if(moveRate == 2)
		{
			stamina = stamina - staminaLossFromHighSpeed;
		}
		}
		
		if(anchored)
		{
		stamina = stamina + staminaGainFromAnchor;
		}
		
		//increment health
		IncrementHealth();
		
		//if anything is less than zero adjust to zero, same if any percent is over 100
		FollowLimits();
		
	}
	
	}
	
	
	
	public void IncrementHealth()
	{
		if(food<=0)
		{
		health = health - healthLossFromStarvation;
		}
		
		if(water<=0)
		{
			health = health - healthLossFromDehydration;
		}
		
		if(stamina<50)
		{
			health = health - (healthLossFromExhaustion*50-stamina);
		}
		
		if(rationRate == 0)
		{
			health = health - healthLossFromLowRations;
		}
		
		if(food > 0 && water > 0 && stamina > 50)
		{
		health = health + healthGainNaturally;
		}
		
		if(rationRate == 2)
		{
		health = health + healthGainFromHighRations;
		}
		
		if(moveRate == 0 && !anchored)
		{
			health = health + healthGainFromSlowSpeed;
		}
		
		if(anchored)
		{
			health = health + healthGainFromAnchoring;
		}
		
	}
	
	public void FollowLimits()
	{
		if(food<0)
		{
			food = 0;
		}
		
		if(water<0)
		{
			water = 0;
		}
		
		if(stamina<0)
		{
			stamina = 0;
		}
		
		if(morale<0)
		{
			morale = 0;
		}
		
		if(health<0)
		{
			health = 0;
		}
		
		if(stamina > 100)
		{
			stamina = 100;
		}
		
		if(morale > 100)
		{
			morale = 100;
		}
		
		if(health > 100)
		{
			health = 100;
		}
	}
	
	public void CalcWeight()
	{
		weight = 0f;
		cargo = GetComponentsInChildren<CargoStats>();
		for(int i = 0; i< cargo.Length; i++)
		{
			weight = weight + cargo[i].weight;
		}
		weight = weight + food;
		weight = weight + water;
		weight = weight + gold;
	}
	
	public void AddCargo(GameObject cargoToAdd)
	{
		CargoStats newCargoStats = cargoToAdd.GetComponent<CargoStats>();
		cargoToAdd.transform.SetParent(transform);
		weight = weight + newCargoStats.weight;
	}
	
	public void DeleteCargo(GameObject cargoToDestroy)
	{
		CargoStats oldCargoStats = cargoToDestroy.GetComponent<CargoStats>();
		weight = weight - oldCargoStats.weight;
		Destroy(cargoToDestroy);
	}
	
	//used to see if a new item will fit in the cargo hold
	public bool checkIfOver(float newObjectWeight)
	{
		if(weight + newObjectWeight > maxWeight)
		{
		return true;
		}
		else return false;
	}
	
	public void Pause()
	{
		paused = true;
	}
	
	public void Unpause()
	{
		paused = false;
	}
	
	public void GatherToggle(GameObject island)
	{
		gathering = true;
		islandToPlunder = island;
	}
	
	public void Gather()
	{
		if(islandToPlunder.GetComponent<IslandGatherScript>().percentGathered == 100f)
		{
			gathering = false;
			//create message saying island is out of resources;
			islandToPlunder.GetComponent<IslandEventScript>().HaveEvent();
		}
		
		if(gatheringCounter > defaultGatheringFrames*(0.2+(islandToPlunder.GetComponent<IslandGatherScript>().percentGathered/50f)))
		{
			gatheringCounter =0;
			if(Random.Range (1,101)<waterPercent)
			{
				water = water + (waterToGain*(1f+(0.1f*troopManager.getScavenging())+(0.2f*islandToPlunder.GetComponent<IslandGatherScript>().amount))); 
				if(weight > maxWeight)
				{
					//open some kinda cargo menu
					water = water + (maxWeight-weight);
				}
			}
			else
			{
				food = food + (foodToGain*(1f+(0.1f*troopManager.getScavenging())+(0.2f*islandToPlunder.GetComponent<IslandGatherScript>().amount))); 
				if(weight > maxWeight)
				{
					//open some kinda cargo menu
					food = food + (maxWeight-weight);
				}
			}
			CalcWeight();
			
			islandToPlunder.GetComponent<IslandGatherScript>().percentGathered = islandToPlunder.GetComponent<IslandGatherScript>().percentGathered + 4;
		}
		gatheringCounter++;
	}
	
	public void Camp()
	{
	
	}
	
	public void toggleAnchor()
	{
		if(anchored)
		{
			anchored = false;
		}
		
		else if(!anchored)
		{
			anchored = true;
		}
	}
}
