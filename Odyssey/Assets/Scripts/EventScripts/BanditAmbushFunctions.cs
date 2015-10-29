using UnityEngine;
using System.Collections;

public class BanditAmbushFunctions :IslandEventFunctions{
    public ResourceManager resourceManager;
    public GameObject defaultConversation;
    public ConversationManager conManager;
    public ChoicesManager choiceManager;
    public GameObject bribeConversation;
    public int goldReward = 70;
    public string battle = "BanditBattle.txt";
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

    public void Attack()
    {
        eventHandler.islandEventIsOn.GetComponent<IslandEventScript>().explored = true;
        eventHandler.rewardGold = goldReward;
        eventHandler.StartBattle(battle,true,false,5);
    }

    public void bribe()
    {
        eventHandler.islandEventIsOn.GetComponent<IslandEventScript>().explored = true;
        resourceManager.gold = resourceManager.gold - 20;
        End();
    }
}
