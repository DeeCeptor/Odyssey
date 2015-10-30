using UnityEngine;
using System.Collections;

public class DisputeEventFunctions : MonoBehaviour {
    public GameObject fightConversation;
    public GameObject diplomacyPass;
    public GameObject diplomacyFail;
    public GameObject convinedConversation;
    public ResourceManager resourceManager;
    public GameObject defaultConversation;
    public ConversationManager conManager;
    public ChoicesManager choiceManager;
    public float moralePenalty = 5;
    public float menToLoseMax = 2;

    // Use this for initialization
    void Start () {
        resourceManager = GameObject.FindGameObjectWithTag("Player").GetComponent<ResourceManager>();
        choiceManager = GameObject.Find("Choices").GetComponent<ChoicesManager>();
        conManager = defaultConversation.GetComponent<ConversationManager>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
