using UnityEngine;
using System.Collections.Generic;

public class Faction 
{
    public string faction_name;
    public List<Faction> enemies = new List<Faction>();
    public List<Unit> units = new List<Unit>();
    public int faction_ID;  // Used for pathing. Each team needs its own ID
    public bool human_controlled;  // Used by the human player?
    private AIController AI;
    public bool use_ai = false;
    public Color faction_color;

    public Faction(string name, bool controlled_by_human, int ID, Color color)
    {
        faction_ID = ID;
        human_controlled = controlled_by_human;
        faction_name = name;
        faction_color = color;
        AI = new AIController(this);
    }
    public void SetAI(AIController faction_AI)
    {
        AI = faction_AI;
    }
    public AIController GetAI()
    {
        return AI;
    }


    public bool IsEnemy(Faction faction)
    {
        return enemies.Contains(faction);
    }
    public bool IsEnemy(Unit unit)
    {
        return enemies.Contains(unit.owner);
    }
    public bool IsAlly(Faction faction)
    {
        return (faction == this);
    }
    public bool IsAlly(Unit unit)
    {
        return (unit.owner == this);
    }


    public List<Unit> GetAllEnemyUnits()
    {
        List<Unit> units = new List<Unit>();

        foreach (Faction enemy in enemies)
        {
            units.AddRange(enemy.units);
        }

        return units;
    }
    public List<Unit> GetAllAllyUnits()
    {
        return units;
    }
}
