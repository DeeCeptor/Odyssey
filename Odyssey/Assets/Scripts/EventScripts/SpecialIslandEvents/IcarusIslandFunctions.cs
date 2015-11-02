using UnityEngine;
using System.Collections;

public class IcarusIslandFunctions : IslandEventFunctions {
    public ResourceManager resourceManager;
    public GameObject defaultConversation;
    public ConversationManager conManager;
    public ChoicesManager choiceManager;
    public GameObject CorpseConversation;
    public GameObject diplomacySuccessConversation;
    public GameObject diplomacyFailureConversation;
    public GameObject pietySuccessConversation;
    public GameObject pietyFailureConversation;
    public GameObject attackConversation;

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

    public void ExploreClearing()
    {
        complete = true;
        eventHandler.islandEventIsOn.GetComponent<IslandEventScript>().explored = true;
        choiceManager.Change_Conversation(CorpseConversation.GetComponent<ConversationManager>());

    }

    public void Diplomacy()
    {

        int sucessCheck = Random.Range(1, 101);
        if (sucessCheck < TroopManager.playerTroops.getDiplomacy()*20)
        {
            choiceManager.Change_Conversation(diplomacySuccessConversation.GetComponent<ConversationManager>());
        }
        else
        {
            choiceManager.Change_Conversation(diplomacyFailureConversation.GetComponent<ConversationManager>());
        }
    }

    public void Piety()
    {
        int sucessCheck = Random.Range(1, 101);
        if (sucessCheck < TroopManager.playerTroops.getPiety() * 20)
        {
            choiceManager.Change_Conversation(pietySuccessConversation.GetComponent<ConversationManager>());
        }
        else
        {
            choiceManager.Change_Conversation(pietyFailureConversation.GetComponent<ConversationManager>());
        }
    }

    public void Attack()
    {
        choiceManager.Continue_Conversation();
    }

}
