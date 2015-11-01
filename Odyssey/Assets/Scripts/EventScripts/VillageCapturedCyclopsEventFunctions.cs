using UnityEngine;
using System.Collections;

public class VillageCapturedCyclopsEventFunctions : IslandEventFunctions {
    public ResourceManager resourceManager;
    public GameObject defaultConversation;
    public ConversationManager conManager;
    public ChoicesManager choiceManager;
    public string battle = "FreeCyclopsBattle.txt";
    public GameObject cyclopsVillageConversation;
    public int cost = 100;
    public GameObject cyclopsKillConversation;
    public GameObject cyclopsFreeConversation;
    public GameObject notEnoughGoldConversation;
    public string unitToGet = "Cyclops";

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

    public void Travel()
    {
        eventHandler.islandEventIsOn.GetComponent<IslandEventScript>().explored = true;
        choiceManager.Change_Conversation(cyclopsVillageConversation.GetComponent<ConversationManager>());
    }

    public void Free()
    {
        if (resourceManager.gold < cost)
        {
            choiceManager.Change_Conversation(notEnoughGoldConversation.GetComponent<ConversationManager>());
        }
        else
        {
            if (TroopManager.playerTroops.healthy.ContainsKey(unitToGet))
            {
                TroopManager.playerTroops.healthy[unitToGet] += 1;
            }
            else
            {
                TroopManager.playerTroops.healthy.Add(unitToGet, 1);
            }
            choiceManager.Change_Conversation(cyclopsFreeConversation.GetComponent<ConversationManager>());
        }

    }

    public void Attack()
    {
        eventHandler.unitsReward.Add(unitToGet,1);
        eventHandler.StartBattle(battle,true,false,5,true,0);
    }

    public void Kill()
    {
        choiceManager.Change_Conversation(cyclopsKillConversation.GetComponent<ConversationManager>());
    }

}
