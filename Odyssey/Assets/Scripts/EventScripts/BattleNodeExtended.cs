using UnityEngine;
using System.Collections;

public class BattleNodeExtended : Node {

    public string battleToHave;
    public bool retreat;
    public bool mustUseHero;
    public int troopsToDeploy;
    public bool first;
    public int aiAggresion;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void Run_Node()
    {
        EventManagement.gameController.EndEvent();
        EventManagement.gameController.StartBattle(battleToHave, retreat, mustUseHero, troopsToDeploy,first,aiAggresion);
    }
}
