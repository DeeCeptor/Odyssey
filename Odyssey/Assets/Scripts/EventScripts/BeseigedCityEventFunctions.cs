using UnityEngine;
using System.Collections;

public class BeseigedCityEventFunctions : IslandEventFunctions {

    public ResourceManager resourceManager;
    public GameObject defaultConversation;
    public ConversationManager conManager;
    public ChoicesManager choiceManager;
    public string AttackBattle;
    public string DefendBattle;
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
        eventHandler.islandEventIsOn.GetComponent<PortIslandEventScript>().raided = true;
        eventHandler.islandEventIsOn.GetComponent<PortIslandEventScript>().explored = true;
        //eventHandler.StartBattle(AttackBattle,true,false,4);
    }

    public void defend()
    {
        eventHandler.islandEventIsOn.GetComponent<PortIslandEventScript>().explored = true;
        //eventHandler.StartBattle(DefendBattle,true,false,4);
    }
}
