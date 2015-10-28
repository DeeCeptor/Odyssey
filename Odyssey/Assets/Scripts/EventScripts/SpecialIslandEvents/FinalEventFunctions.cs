using UnityEngine;
using System.Collections;



public class FinalEventFunctions : IslandEventFunctions {
    public ResourceManager resourceManager;
    public GameObject defaultConversation;
    public ConversationManager conManager;
    public ChoicesManager choiceManager;
    public GameObject scyllaConversation;
    public GameObject charybdisConversation;
    public GameObject charybdisFailConversation;
    public GameObject toGreeceConversation;
    public GameObject initialSuitorConversation;
    public GameObject beggarConversation;
    public GameObject boldConversation;
    public GameObject suitorBattleConversation;
    public GameObject provePassConversation;
    public GameObject proveFailConversation;
    public GameObject diplomacyPassConversation;
    public GameObject diplomacyFailConversation;
    public GameObject disgraceConversation;
    public string battleToHave;
    public int SoldiersInjureFromCharybdis = 15;
    public int hullFromCharybdis = 70;

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

    public void Charybdis()
    {
        int sucessCheck = Random.Range(1, 101);
        if (sucessCheck < TroopManager.playerTroops.getSeamanship() * 20)
        {
            TroopManager.playerTroops.InjureRandom(SoldiersInjureFromCharybdis);
            resourceManager.shipHealth = resourceManager.shipHealth - hullFromCharybdis;
            choiceManager.Change_Conversation(charybdisConversation.GetComponent<ConversationManager>());
        }
        else
        {
            choiceManager.Change_Conversation(charybdisFailConversation.GetComponent<ConversationManager>());
        }
    }

    public void Scylla()
    {
        TroopManager.playerTroops.killRandom(SoldiersInjureFromCharybdis);
        choiceManager.Change_Conversation(scyllaConversation.GetComponent<ConversationManager>());
    }

    public void Disguise()
    {
        choiceManager.Change_Conversation(beggarConversation.GetComponent<ConversationManager>());
    }

    public void Battle()
    {
        choiceManager.Change_Conversation(suitorBattleConversation.GetComponent<ConversationManager>());
    }

    public void Announce()
    {
        choiceManager.Change_Conversation(boldConversation.GetComponent<ConversationManager>());
    }

    public void Prove()
    {
        int sucessCheck = Random.Range(1, 101);
        int otherCheck = Random.Range(1, 101);
        if (sucessCheck < TroopManager.playerTroops.getStrength() * 20 && otherCheck < TroopManager.playerTroops.getBow())
        {
            choiceManager.Change_Conversation(provePassConversation.GetComponent<ConversationManager>());
        }
        else
        {
            choiceManager.Change_Conversation(proveFailConversation.GetComponent<ConversationManager>());
        }
    }

    public void Diplomacy()
    {
        int sucessCheck = Random.Range(1, 101);
        int otherCheck = Random.Range(1, 101);
        if (sucessCheck < TroopManager.playerTroops.getDiplomacy() * 20 && otherCheck < TroopManager.playerTroops.getLeadership())
        {
            choiceManager.Change_Conversation(diplomacyPassConversation.GetComponent<ConversationManager>());
        }
        else
        {
            choiceManager.Change_Conversation(diplomacyFailConversation.GetComponent<ConversationManager>());
        }
    }
}
