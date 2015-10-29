using UnityEngine;
using System.Collections;

public class CentaurCaptureEventFunctions : IslandEventFunctions {
    public ResourceManager resourceManager;
    public GameObject defaultConversation;
    public ConversationManager conManager;
    public ChoicesManager choiceManager;
    public GameObject exploreConversation;
    public string battle = "CentaurRescueVillageBattle.txt";
    public GameObject freeConversation;
    public GameObject hireConversation;
    public GameObject notEnoughGoldConversation;
    public int freeCost = 40;
    public int hireCost = 50;
    public int humansToGet = 4;
    public int centaursToGet = 2;
    public string humanUnitToGet = "Swordsman";
    public string humanUnit2ToGet = "Slinger";
    public string centaurWarriorToGet = "CentaurWarrior";
    public string centaurArcherToGet = "CentaurMarksman";

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

    public void ExploreForest()
    {
        eventHandler.islandEventIsOn.GetComponent<IslandEventScript>().explored = true;
        choiceManager.Change_Conversation(exploreConversation.GetComponent<ConversationManager>());
    }

    public void Attack()
    {
        
        eventHandler.unitsReward.Add(humanUnitToGet, humansToGet);
        eventHandler.unitsReward.Add(humanUnit2ToGet, humansToGet);

        eventHandler.StartBattle(battle,true,false,5);
    }

    public void Free()
    {
        if (resourceManager.gold < freeCost)
        {
            choiceManager.Change_Conversation(notEnoughGoldConversation.GetComponent<ConversationManager>());
        }
        else
        {
            if (TroopManager.playerTroops.healthy.ContainsKey(humanUnitToGet))
            {
                TroopManager.playerTroops.healthy[humanUnitToGet] += humansToGet;
            }
            else
            {
                TroopManager.playerTroops.healthy.Add(humanUnitToGet,humansToGet);
            }

            if (TroopManager.playerTroops.healthy.ContainsKey(humanUnit2ToGet))
            {
                TroopManager.playerTroops.healthy[humanUnit2ToGet] += humansToGet;
            }
            else
            {
                TroopManager.playerTroops.healthy.Add(humanUnit2ToGet, humansToGet);
            }

            resourceManager.gold = resourceManager.gold - hireCost;
            choiceManager.Change_Conversation(freeConversation.GetComponent<ConversationManager>());
        }
    }

    public void Hire()
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
