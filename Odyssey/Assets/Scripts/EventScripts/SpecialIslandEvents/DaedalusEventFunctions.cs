using UnityEngine;
using System.Collections;

public class DaedalusEventFunctions : PortEventFunctions {
    public bool icarus = false;
    public GameObject getDaedalusConversation;
    public GameObject learnDaedalusConversation;
    public GameObject Daedalus;
    public GameObject daedalusPortConversation;
    public bool DaedalusGot;

    // Use this for initialization
    void Start () {
        eventHandler = GameObject.FindGameObjectWithTag("EventController").GetComponent<EventManagement>();
        resourceManager = GameObject.FindGameObjectWithTag("Player").GetComponent<ResourceManager>();
        choiceManager = GameObject.Find("Choices").GetComponent<ChoicesManager>();
        conManager = defaultConversation.GetComponent<ConversationManager>();
        raided = eventHandler.islandEventIsOn.GetComponent<PortIslandEventScript>().raided;
        icarus = GameObject.Find("IcarusIsland").GetComponent<IslandEventScript>().explored;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void WorriedMan()
    {
        if(icarus)
        {
            if (!TroopManager.playerTroops.FindHero("Daedalus"))
            {
                TroopManager.playerTroops.AddHero(Daedalus);
            }
            choiceManager.Change_Conversation(getDaedalusConversation.GetComponent<ConversationManager>());
            DaedalusGot = true;
        }
        else
        {
            choiceManager.Change_Conversation(learnDaedalusConversation.GetComponent<ConversationManager>());
        }
    }

    public void choice2()
    {
        if (!raided)
        {
            if(DaedalusGot)
            {
                choiceManager.Change_Conversation(portConversation.GetComponent<ConversationManager>());
            }
            else
            {
                choiceManager.Change_Conversation(daedalusPortConversation.GetComponent<ConversationManager>());
            }
        }
        else
        {
            choiceManager.Change_Conversation(raidedConversation.GetComponent<ConversationManager>());
        }
    }
}
