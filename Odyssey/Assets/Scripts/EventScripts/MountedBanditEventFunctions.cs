using UnityEngine;
using System.Collections;

public class MountedBanditEventFunctions : IslandEventFunctions {
    public ResourceManager resourceManager;
    public GameObject defaultConversation;
    public ConversationManager conManager;
    public ChoicesManager choiceManager;
    public string AttackBattle = "MountedBanditBattle.txt";
    public GameObject banditConversation;
    public GameObject bribeConversation;
    public int bribe = 40;
    public int goldReward = 100;
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
        choiceManager.Change_Conversation(banditConversation.GetComponent<ConversationManager>());
    }

    public void Fight()
    {
        eventHandler.rewardGold = goldReward;
        eventHandler.StartBattle(AttackBattle,true,false,4,false,1);
    }

    public void TryBribe()
    {
        int sucessCheck = Random.Range(1, 101);
        if (sucessCheck < TroopManager.playerTroops.getHaggling() * 20 && resourceManager.gold > bribe)
        {
            resourceManager.gold = resourceManager.gold - bribe;
            choiceManager.Change_Conversation(bribeConversation.GetComponent<ConversationManager>());
        }
        else
        {
            eventHandler.rewardGold = goldReward;
            choiceManager.Continue_Conversation();
        }
    }

}
