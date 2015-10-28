using UnityEngine;
using System.Collections;

public class CircesIslandEventFunctions : IslandEventFunctions {
    public ResourceManager resourceManager;
    public GameObject defaultConversation;
    public ConversationManager conManager;
    public ChoicesManager choiceManager;
    public GameObject cottageConversation;
    public GameObject battleConversation;
    public GameObject metBattleConversation;
    public GameObject raidBattleConversation;
    public GameObject feastConversation;
    public GameObject leaveConversation;
    public GameObject harvestConversation;
    public GameObject hermesFavourConversation;
   
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
        choiceManager.Change_Conversation(cottageConversation.GetComponent<ConversationManager>());
    }

    public void HarvestAnimals()
    {
        int sucessCheck = Random.Range(1, 101);
        if (sucessCheck < 50)
        {
            resourceManager.AddFood(20);
            choiceManager.Continue_Conversation();
        }
        else
        {
            choiceManager.Change_Conversation(raidBattleConversation.GetComponent<ConversationManager>());
        }
    }

    public void Attack()
    {
        choiceManager.Change_Conversation(raidBattleConversation.GetComponent<ConversationManager>());
    }

    public void Swined()
    {
        int sucessCheck = Random.Range(1, 101);
        if (sucessCheck < resourceManager.hermesFavour)
        {
            choiceManager.Change_Conversation(hermesFavourConversation.GetComponent<ConversationManager>());
        }
        else
        {
            choiceManager.Change_Conversation(battleConversation.GetComponent<ConversationManager>());
        }
    }

    public void CheckCottage()
    {
        choiceManager.Change_Conversation(feastConversation.GetComponent<ConversationManager>());
    }
}
