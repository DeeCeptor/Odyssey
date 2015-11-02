using UnityEngine;
using System.Collections;

public class BattleNode : Node {

    public string battleToHave;
    public bool retreat;
    public bool mustUseHero;
    public int troopsToDeploy;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public override void Run_Node()
    {
        EventManagement.gameController.StartBattle(battleToHave,retreat,mustUseHero,troopsToDeploy);
    }
}
