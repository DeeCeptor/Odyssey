using UnityEngine;
using System.Collections;

public class BadOmensFunctions : MonoBehaviour {
    public ResourceManager resourceManager;
    public GameObject defaultConversation;
    public ConversationManager conManager;
    public ChoicesManager choiceManager;
    public float moraleToLose;
    public GameObject failConversation;
	// Use this for initialization
	void Start () {
        resourceManager = GameObject.FindGameObjectWithTag("Player").GetComponent<ResourceManager>();
        choiceManager = GameObject.Find("Choices").GetComponent<ChoicesManager>();
        conManager = defaultConversation.GetComponent<ConversationManager>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Lead()
    {
        int sucessCheck = Random.Range(1, 101);
        if (sucessCheck < TroopManager.playerTroops.getLeadership() * 20)
        {
            
            choiceManager.Continue_Conversation();
        }
        else
        {
            resourceManager.morale = resourceManager.morale - moraleToLose;
            choiceManager.Change_Conversation(failConversation.GetComponent<ConversationManager>());
        }
    }

    public void Piety()
    {
        int sucessCheck = Random.Range(1, 101);
        if (sucessCheck < TroopManager.playerTroops.getPiety() * 20)
        {

            choiceManager.Continue_Conversation();
        }
        else
        {
            resourceManager.morale = resourceManager.morale - moraleToLose;
            choiceManager.Change_Conversation(failConversation.GetComponent<ConversationManager>());
        }
    }
}
