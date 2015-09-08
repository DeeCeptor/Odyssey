using UnityEngine;
using System.Collections;

public class FishEventFunctions : MonoBehaviour {

public EventManagement eventController;
public ResourceManager resourceManager;
public GameObject defaultConversation;
public ConversationManager conManager;
public ChoicesManager choiceManager;
public GameObject failureConversation;


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
	
	public void fish(){
	// random number from 1 to 100 for checking sucess
	int sucessCheck = Random.Range(1,101);
	if(sucessCheck > 50)
	{
		choiceManager.Continue_Conversation();
		resourceManager.food = resourceManager.food + 10 + Random.Range(0,21);
	}
	else if(sucessCheck > 0 && sucessCheck <=50)
	{
			choiceManager.Change_Conversation(failureConversation.GetComponent<ConversationManager>());
	}
	
	}
	
	public void End()
	{
	eventController.EndEvent();
	}
}
