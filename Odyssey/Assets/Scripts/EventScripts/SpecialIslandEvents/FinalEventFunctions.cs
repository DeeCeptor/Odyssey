using UnityEngine;
using System.Collections;



public class FinalEventFunctions : IslandEventFunctions {
    public GameObject scyllaConversation;
    public GameObject charybdisConversation;
    public GameObject charybdisFailConversation;
    public GameObject toGreeceConversation;
    public GameObject initialSuitorConversation;
    public GameObject beggarConversation;
    public GameObject boldConversation;
    public GameObject suitorBattleConversation;
    public GameObject bowPassConversation;
    public GameObject bowFailConversation;
    public GameObject sneakPassConversation;
    public GameObject sneakFailConversation;
    public GameObject diplomacyPassConversation;
    public GameObject diplomacyFailConversation;
    public GameObject disgraceConversation;
    public string battleToHave;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Charybdis()
    {

    }

    public void Scylla()
    {

    }

    public void Disguise()
    {

    }

    public void Battle()
    {
        eventHandler.StartBattle(battleToHave,false,false,12);
    }

    public void Announce()
    {

    }
}
