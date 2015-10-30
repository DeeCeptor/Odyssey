using UnityEngine;
using System.Collections;

public class GoodOmensFunctions : MonoBehaviour {
    public ResourceManager resourceManager;
    public GameObject defaultConversation;
    public ConversationManager conManager;
    public ChoicesManager choiceManager;
    public float moraleToGain;
    public GameObject godConversation;
    public float poseidonFavourToGain;
    // Use this for initialization
    void Start () {
        resourceManager = GameObject.FindGameObjectWithTag("Player").GetComponent<ResourceManager>();
        choiceManager = GameObject.Find("Choices").GetComponent<ChoicesManager>();
        conManager = defaultConversation.GetComponent<ConversationManager>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Celebrate()
    {
        if (resourceManager.morale + moraleToGain < 100)
        {
            resourceManager.morale = resourceManager.morale + moraleToGain;
        }
        else
        {
            resourceManager.morale = 100;
        }

        choiceManager.Continue_Conversation();
    }

    public void Thank()
    {
        if (resourceManager.poseidonsFavour + poseidonFavourToGain < 100)
        {
            resourceManager.poseidonsFavour = resourceManager.poseidonsFavour + poseidonFavourToGain;
        }
        else
        {
            resourceManager.poseidonsFavour = 100;
        }

        choiceManager.Change_Conversation(godConversation.GetComponent<ConversationManager>());
    }
}
