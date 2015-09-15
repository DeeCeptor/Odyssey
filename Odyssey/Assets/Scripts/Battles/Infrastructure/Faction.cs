using UnityEngine;
using System.Collections.Generic;

public class Faction 
{
    public string faction_name;
    public List<Faction> enemies = new List<Faction>();
    public List<Unit> units = new List<Unit>();

    public bool human_controlled;  // Used by the human player?

    public Faction(string name, bool controlled_by_human)
    {
        human_controlled = controlled_by_human;
        faction_name = name;
    }


    public bool IsEnemy(Faction faction)
    {
        return enemies.Contains(faction);
    }
    public bool IsEnemy(Unit unit)
    {
        return enemies.Contains(unit.owner);
    }
}
