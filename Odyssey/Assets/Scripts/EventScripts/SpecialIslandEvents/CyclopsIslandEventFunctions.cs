using UnityEngine;
using System.Collections;

public class CyclopsIslandEventFunctions : IslandEventFunctions {

    // Use this for initialization
    public ResourceManager resourceManager;
    public GameObject defaultConversation;
    public ConversationManager conManager;
    public ChoicesManager choiceManager;
    public GameObject caveConversation;
    public GameObject battleConversation;
    public GameObject feastConversation;
    public GameObject sucessfulBowConversation;
    public GameObject failureBowConversation;
    public GameObject sucessfulCunningConversation;
    public GameObject failureCunningConversation;

    public float foodToAdd = 50;
    public float waterToAdd = 30;

    void Start()
    {
        resourceManager = GameObject.FindGameObjectWithTag("Player").GetComponent<ResourceManager>();
        choiceManager = GameObject.Find("Choices").GetComponent<ChoicesManager>();
        conManager = defaultConversation.GetComponent<ConversationManager>();
        eventHandler = GameObject.FindGameObjectWithTag("EventController").GetComponent<EventManagement>();
    }
    // Update is called once per frame
    void Update () {
	
	}

    public void ExploreCave()
    {
        eventHandler.islandEventIsOn.GetComponent<IslandEventScript>().explored = true;
        choiceManager.Change_Conversation(caveConversation.GetComponent<ConversationManager>());
    }

    public void BattleConversation()
    {
        choiceManager.Change_Conversation(battleConversation.GetComponent<ConversationManager>());
    }

    public void Feast()
    {
        resourceManager.food = resourceManager.food + foodToAdd;
        resourceManager.water = resourceManager.water + waterToAdd;
        choiceManager.Change_Conversation(feastConversation.GetComponent<ConversationManager>());
    }

    public void Sacrifice()
    {
        if(resourceManager.sailors > 10)
        {
            resourceManager.sailors = resourceManager.sailors - 10;
        }
        else
        {
            resourceManager.sailors = 0;
        }
        TroopManager.playerTroops.InjureRandom(10);
        choiceManager.Continue_Conversation();

    }

    public void Scamper()
    {
        int sucessCheck = Random.Range(1, 101);
        if (sucessCheck < TroopManager.playerTroops.getScavenging() * 20)
        {
            resourceManager.food = resourceManager.food + foodToAdd/2;
            resourceManager.water = resourceManager.water + waterToAdd/2;
            choiceManager.Continue_Conversation();
        }
        else
        {
            choiceManager.Change_Conversation(feastConversation.GetComponent<ConversationManager>());
        }
    }

    public void Cunning()
    {
        int sucessCheck = Random.Range(1, 101);
        if (sucessCheck < TroopManager.playerTroops.getCunning() * 20)
        {
            choiceManager.Change_Conversation(sucessfulCunningConversation.GetComponent<ConversationManager>());
        }
        else
        {
            choiceManager.Change_Conversation(failureCunningConversation.GetComponent<ConversationManager>());
        }
    }

    public void Bow()
    {
        int sucessCheck = Random.Range(1, 101);
        if (sucessCheck < TroopManager.playerTroops.getBow()*20)
        {
            choiceManager.Change_Conversation(sucessfulBowConversation.GetComponent<ConversationManager>());
        }
        else
        {
            choiceManager.Change_Conversation(failureBowConversation.GetComponent<ConversationManager>());
        }
    }

    public void Leave()
    {
        choiceManager.Continue_Conversation();
    }
}
