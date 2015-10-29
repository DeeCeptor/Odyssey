using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PortEventFunctions : IslandEventFunctions{
    public ResourceManager resourceManager;
    public GameObject defaultConversation;
    public ConversationManager conManager;
    public ChoicesManager choiceManager;
    public GameObject PortMenu;
    public GameObject RaidConversation;
    public GameObject portConversation;
    public GameObject raidedConversation;
    public bool raided = false;
    public float foodReward = 50f;
    public float waterReward = 50f;
    public int goldReward = 100;
    public string battleLevel = "PortRaidBattle.txt";
    public int favourToGain = 0;
    

    // Use this for initialization
    void Start () {
        eventHandler = GameObject.FindGameObjectWithTag("EventController").GetComponent<EventManagement>();
        resourceManager = GameObject.FindGameObjectWithTag("Player").GetComponent<ResourceManager>();
        choiceManager = GameObject.Find("Choices").GetComponent<ChoicesManager>();
        conManager = defaultConversation.GetComponent<ConversationManager>();
        raided = eventHandler.islandEventIsOn.GetComponent<PortIslandEventScript>().raided;
        
    }
	
	// Update is called once per frame
	void FixedUpdate () {
	
	}

    public void choice2()
    {
        if (!raided)
        {
            choiceManager.Change_Conversation(portConversation.GetComponent<ConversationManager>());
        }
        else
        {
            choiceManager.Change_Conversation(raidedConversation.GetComponent<ConversationManager>());
        }
    }

    public void Port()
    {

        GameObject shopMenu = Instantiate(PortMenu);
        MenuSwitch2 shopScript = shopMenu.GetComponent<MenuSwitch2>();
        Destroy(gameObject);
    }

    public void Raid()
    {
        eventHandler.rewardFood = foodReward;
        eventHandler.rewardWater = waterReward;
        eventHandler.rewardGold = goldReward;
        resourceManager.zeusFavour += favourToGain;
        eventHandler.islandEventIsOn.GetComponent<PortIslandEventScript>().raided = true;
        eventHandler.StartBattle(battleLevel,true,false,7,true,0);
        eventHandler.EndEvent();
    }
}
