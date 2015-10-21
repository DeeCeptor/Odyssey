﻿using UnityEngine;
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

    public Color faction_color;	// Colour of the facing aura
	public Color unit_color;	// Colour tinting of the unit itself. White is default.

    public Faction(string name, bool controlled_by_human, int ID, Color aura_colour, Color unit_tint)
    {
        faction_ID = ID;
        human_controlled = controlled_by_human;
        faction_name = name;
		faction_color = aura_colour;
		unit_color = unit_tint;
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


    // Returns the closest enemy. Returns the given unit if there are no enemies
    public Unit GetClosestEnemy(Unit unit)
    {
        List<Unit> enemies = GetAllEnemyUnits();
        int closest_distance = 99;
        Unit closest_enemy = unit;
        foreach (Unit enemy in enemies)
        {
            int distance = HexMap.hex_map.DistanceBetweenHexes(unit.location.coordinate, enemy.location.coordinate);

            if (distance < closest_distance)
            {
                closest_enemy = enemy;
                closest_distance = distance;
            }
        }

        return closest_enemy;
    }
}
