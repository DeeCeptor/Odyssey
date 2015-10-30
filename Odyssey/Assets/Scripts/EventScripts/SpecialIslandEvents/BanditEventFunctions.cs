using UnityEngine;
using System.Collections;

public class BanditEventFunctions : IslandEventFunctions {
    public ResourceManager resourceManager;
    public GameObject defaultConversation;
    public ConversationManager conManager;
    public ChoicesManager choiceManager;
    public GameObject banditConversation;
    public GameObject battleConversation;
    public GameObject tipConversation;
    public GameObject leaveConversation;
    public int tipOffReward = 20;

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
        //eventHandler.islandEventIsOn.GetComponent<IslandEventScript>().explored = true;
        choiceManager.Change_Conversation(banditConversation.GetComponent<ConversationManager>());
    }

    public void TipOff()
    {
        resourceManager.gold = resourceManager.gold + tipOffReward;
        choiceManager.Continue_Conversation();
    }

    public void Leave()
    {
        choiceManager.Change_Conversation(leaveConversation.GetComponent<ConversationManager>());
    }

    public void Rob()
    {
        choiceManager.Change_Conversation(battleConversation.GetComponent<ConversationManager>());
    }
}
