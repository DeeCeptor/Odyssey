using UnityEngine;
using System.Collections;

public class DisputeEventFunctions : MonoBehaviour {
    public GameObject fightConversation;
    public GameObject diplomacyPass;
    public GameObject diplomacyFail;
    public GameObject confineConversation;
    public ResourceManager resourceManager;
    public GameObject defaultConversation;
    public ConversationManager conManager;
    public ChoicesManager choiceManager;
    public float moralePenalty = 5;
    public int menToLoseMax = 2;


    // Use this for initialization
    void Start () {
        resourceManager = GameObject.FindGameObjectWithTag("Player").GetComponent<ResourceManager>();
        choiceManager = GameObject.Find("Choices").GetComponent<ChoicesManager>();
        conManager = defaultConversation.GetComponent<ConversationManager>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Confine()
    {
        if (resourceManager.morale > moralePenalty)
        {
            resourceManager.morale = resourceManager.morale - moralePenalty;
        }
        choiceManager.Change_Conversation(confineConversation.GetComponent<ConversationManager>());
    }

    public void Diplomacy()
    {
        int sucessCheck = Random.Range(1, 101);
        if (sucessCheck < TroopManager.playerTroops.getDiplomacy() * 20)
        {

            choiceManager.Change_Conversation(diplomacyPass.GetComponent<ConversationManager>());
        }
        else
        {
            resourceManager.sailors = resourceManager.sailors - Random.Range(1, menToLoseMax + 1);
            resourceManager.morale = resourceManager.morale - moralePenalty;
            choiceManager.Change_Conversation(diplomacyFail.GetComponent<ConversationManager>());
        }
    }

    public void Fight()
    {
        if (resourceManager.sailors > 2)
        {
            resourceManager.sailors = resourceManager.sailors - Random.Range(1, menToLoseMax + 1);
        }
        choiceManager.Continue_Conversation();
    }
}
