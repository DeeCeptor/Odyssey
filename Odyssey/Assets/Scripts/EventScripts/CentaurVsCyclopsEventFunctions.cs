using UnityEngine;
using System.Collections;

public class CentaurVsCyclopsEventFunctions : IslandEventFunctions {
    public ResourceManager resourceManager;
    public GameObject defaultConversation;
    public ConversationManager conManager;
    public ChoicesManager choiceManager;
    public string centaurWarriorToGet = "CentaurWarrior";
    public string centaurArcherToGet = "CentaurMarksman";
    public int centaursToGet = 4;
    public string cyclopsToGet = "Cyclops";
    public int cyclopsNumToGet = 2;
    public string cyclopsFight = "CyclopsInCycVSCentBattle.txt";
    public string centaurFight = "CentaurInCycVSCentBattle.txt";
    public GameObject FightConversation;

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
        choiceManager.Change_Conversation(FightConversation.GetComponent<ConversationManager>());
    }

    public void FightCyclops()
    {
        eventHandler.unitsReward.Add(centaurWarriorToGet, centaursToGet);
        eventHandler.unitsReward.Add(centaurArcherToGet, centaursToGet);

        eventHandler.StartBattle(cyclopsFight, true, false, 5);
    }

    public void FightCentaur()
    {
        eventHandler.unitsReward.Add(cyclopsToGet, cyclopsNumToGet);
        eventHandler.StartBattle(centaurFight, true, false, 5);
    }
}
