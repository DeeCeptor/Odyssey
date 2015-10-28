using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PortEventFunctions : IslandEventFunctions{
    public GameObject PortMenu;
    public GameObject RaidConversation;
    public float foodReward = 50f;
    public float waterReward = 50f;
    public float goldReward = 100f;
    public bool raided = false;
    public Dictionary<string,int> units;
    public int rand;

	// Use this for initialization
	void Start () {
        eventHandler = GameObject.FindGameObjectWithTag("EventController").GetComponent<EventManagement>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
	
	}

    public void Port()
    {

        GameObject shopMenu = Instantiate(PortMenu);
        MenuSwitch2 shopScript = shopMenu.GetComponent<MenuSwitch2>();
        Destroy(gameObject);
    }

    public void Raid()
    {

    }
}
