using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinotaurEventFunctions : IslandEventFunctions {
    public ResourceManager resourceManager;
    public GameObject defaultConversation;
    public ConversationManager conManager;
    public ChoicesManager choiceManager;
    public bool explored;
    public string battleSaveMinotaur;
    public string battleKillMinotaur;
    public GameObject Theseus;
    public GameObject MinotaurConversation;
	// Use this for initialization
	void Start () {
        resourceManager = GameObject.FindGameObjectWithTag("Player").GetComponent<ResourceManager>();
        choiceManager = GameObject.Find("Choices").GetComponent<ChoicesManager>();
        conManager = defaultConversation.GetComponent<ConversationManager>();
        eventHandler = GameObject.FindGameObjectWithTag("EventController").GetComponent<EventManagement>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Save()
    {
        eventHandler.unitsReward = new Dictionary<string,int>();
        eventHandler.unitsReward.Add("Minotaur",1);
        eventHandler.StartBattle(battleSaveMinotaur,true,false,4);
    }

    public void Fight()
    {
        if (TroopManager.playerTroops.FindHero("Theseus"))
        {
            eventHandler.StartBattle(battleKillMinotaur, true, false, 4);
        }
        else
        {
            eventHandler.heroReward = Theseus;
            eventHandler.StartBattle(battleKillMinotaur, true, false, 4);
        }
        
    }

    public void ExploreVillage()
    {
        eventHandler.islandEventIsOn.GetComponent<IslandEventScript>().explored = true;
        choiceManager.Change_Conversation(MinotaurConversation.GetComponent<ConversationManager>());
    }
}
