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

public bool paused = false;

//how frequently your sailors must eat
public int framesPerConsuption = 120;
private int frameIterator = 0;

//rates of food and water consumption
public float foodPerSailorPerConsumption = 0.005f;
public float lowRationFoodPerSailorPerConsumption = 0.0025f;
public float highRationFoodPerSailorPerConsumption = 0.0075f;
public float waterPerSailorPerConsumption = 0.01f;
public float lowRationWaterPerSailorPerConsumption = 0.005f;
public float highRationWaterPerSailorPerConsumption = 0.015f;

//for ration and movement rates the lowest number is the lowest amount of rations and the lowest move rate is the slowest movement rate
public int rationRate = 1;
public int moveRate = 1;

//rates of morale and stamina loss/gain
public float moraleLossFromLowRations = 0.5f;
public float moraleGainFromHighRations = 0.5f;
public float staminaLossFromHighSpeed = 0.5f;
public float staminaLossFromLowRations= 0.5f;
public float staminaGainFromLowSpeed = 0.5f;
public float staminaGainFromHighRations= 0.5f;

//amount of resources
public float food = 100f;
public float water = 100f;
public int sailors = 100;
public float stamina = 100f;
public float wood = 50f;
public float ore = 50f;
public float morale = 100f;



	// Use this for initialization
	void Start () {
	waterText.GetComponent<Text>().text = "water: " + water.ToString() + "L";
	foodText.GetComponent<Text>().text = "food: " + food.ToString() + "Kg";
	moraleText.GetComponent<Text>().text = "morale: " + morale.ToString() + "%";
	staminaText.GetComponent<Text>().text = "stamina: " + stamina.ToString() + "%";
	sailorText.GetComponent<Text>().text = "sailors: " + sailors.ToString();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	if(paused == false)
	{
		if(frameIterator>framesPerConsuption)
		{
			frameIterator = 0;
			Consume(1);
			
		}
		frameIterator = frameIterator + 1;
		waterText.GetComponent<Text>().text = "water: " + water.ToString() + "L";
		foodText.GetComponent<Text>().text = "food: " + food.ToString() + "Kg";
		moraleText.GetComponent<Text>().text = "morale: " + morale.ToString() + "%";
		staminaText.GetComponent<Text>().text = "stamina: " + stamina.ToString() + "%";
		sailorText.GetComponent<Text>().text = "sailors: " + sailors.ToString();
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
	for(int i = 0;i<Consumptions;i++)
	{
		if(rationRate == 0)
		{
			food = food - (sailors*lowRationFoodPerSailorPerConsumption);
			water = water - (sailors*lowRationWaterPerSailorPerConsumption);
			morale = morale - moraleLossFromLowRations;
			stamina = stamina - staminaLossFromLowRations;
		}
		
		if(rationRate == 1)
		{
			food = food - (sailors*foodPerSailorPerConsumption);
			water = water - (sailors*waterPerSailorPerConsumption);
		}
		
		if(rationRate == 2)
		{
			food = food - (sailors*highRationFoodPerSailorPerConsumption);
			water = water - (sailors*highRationWaterPerSailorPerConsumption);
			morale = morale + moraleGainFromHighRations;
			stamina = stamina + staminaGainFromHighRations;
		}
		
		if(moveRate == 0)
		{
			stamina = stamina + staminaGainFromLowSpeed;
		}
		
		if(moveRate == 2)
		{
			stamina = stamina - staminaLossFromHighSpeed;
		}	
	}
	
	}
	
	public void Pause()
	{
	paused = true;
	}
	
	public void Unpause()
	{
		paused = false;
	}
}
