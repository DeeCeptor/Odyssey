using UnityEngine;
using System.Collections;

public class TroopManager : MonoBehaviour {
public int totalTroops = 100;
public GameObject[] Units;
public GameObject[] Heroes;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void AddTroop(GameObject unit)
	{
	unit.transform.SetParent(transform);
	Units[Units.Length] = unit;
	}
	
	public void AddHero(GameObject hero)
	{
		hero.transform.SetParent(transform);
		Units[Units.Length] = hero;
	}
	
	public float getFoodConsumption()
	{
	return 0;
	}
	
	public float getWaterConsumption()
	{
	return 0;
	}
	
	
}
