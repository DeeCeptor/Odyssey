using UnityEngine;
using System.Collections;

public class PirateEventFunctions : MonoBehaviour {

	public EventManagement eventController;
	public ResourceManager resourceManager;
	public GameObject defaultConversation;
	public ConversationManager conManager;
	public ChoicesManager choiceManager;
	public GameObject failureConversation;
	public GameObject bribeConversation;
    public string pathToBattle = "";
	// Use this for initialization
	void Start () {
		resourceManager = GameObject.FindGameObjectWithTag("Player").GetComponent<ResourceManager>();
		choiceManager = GameObject.Find("Choices").GetComponent<ChoicesManager>();
		conManager = defaultConversation.GetComponent<ConversationManager>();
		eventController = GameObject.FindGameObjectWithTag("EventController").GetComponent<EventManagement>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Fight()
	{
        eventController.EndEvent();
        eventController.StartBattle(pathToBattle,false,false,7);
	   
	}
	
	public void Bribe()
	{
		if(resourceManager.gold < 20)
		{
		//initiateBattle
		}
		else
		{
		resourceManager.gold = resourceManager.gold - 20;
		choiceManager.Change_Conversation(bribeConversation.GetComponent<ConversationManager>());
		}
	}
	
	public void Flee()
	{
		// random number from 1 to 100 for checking sucess
		int sucessCheck = Random.Range(1,101);
		if(sucessCheck > 70)
		{
			choiceManager.Continue_Conversation();
		}
		else if(sucessCheck > 0 && sucessCheck <=70)
		{
			choiceManager.Change_Conversation(failureConversation.GetComponent<ConversationManager>());
		}
	}
	
	public void End()
	{
		eventController.EndEvent();
	}
}
