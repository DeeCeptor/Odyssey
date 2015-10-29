using UnityEngine;
using System.Collections;

public class CentaurMercenaryEventFunctions : IslandEventFunctions {
    public ResourceManager resourceManager;
    public string centaurWarriorToGet = "CentaurWarrior";
    public string centaurArcherToGet = "CentaurMarksman";
    public int centaursToGet = 4;
    public int hireCost = 100;
    public GameObject defaultConversation;
    public ConversationManager conManager;
    public ChoicesManager choiceManager;
    public GameObject notEnoughGoldConversation;
    public GameObject hireConversation;
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

    public void hire()
    {
        if (resourceManager.gold < hireCost)
        {
            choiceManager.Change_Conversation(notEnoughGoldConversation.GetComponent<ConversationManager>());
        }
        else
        {
            if (TroopManager.playerTroops.healthy.ContainsKey(centaurWarriorToGet))
            {
                TroopManager.playerTroops.healthy[centaurWarriorToGet] += centaursToGet;
            }
            else
            {
                TroopManager.playerTroops.healthy.Add(centaurWarriorToGet, centaursToGet);
            }

            if (TroopManager.playerTroops.healthy.ContainsKey(centaurArcherToGet))
            {
                TroopManager.playerTroops.healthy[centaurArcherToGet] += centaursToGet;
            }
            else
            {
                TroopManager.playerTroops.healthy.Add(centaurArcherToGet, centaursToGet);
            }

            resourceManager.gold = resourceManager.gold - hireCost;
            choiceManager.Change_Conversation(hireConversation.GetComponent<ConversationManager>());
        }
    }
}
