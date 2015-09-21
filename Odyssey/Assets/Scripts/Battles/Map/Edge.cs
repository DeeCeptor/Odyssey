using UnityEngine;
using System.Collections;

public class Edge 
{
    public Hex destination;
    public Hex source;
    private int[] cost = new int[3];    // The cost of traversing this edge. Some hexes are slower to move through (Swamps). 
                                        // Units don't allow enemy units to traverse next to the unit (area of control).

    public Edge(Hex destination_hex, Hex source_hex, int movementCost)
    {
        destination = destination_hex;
        source = source_hex;
        cost[0] = movementCost;
        cost[1] = movementCost;
        cost[2] = movementCost;
    }


    // Units don't allow enemies to traverse next to this unit
    public int GetCost(Faction faction)
    {
        return cost[faction.faction_ID]; 
    }


    // Unit moved to a hex adjacent to this edge. Increase the cost of this edge for enemy units to traverse
    public void SetZoneOfControl(Faction faction)
    {
        if (faction.faction_ID == 1)
            cost[2] = 5;
        else if (faction.faction_ID == 2)
            cost[1] = 5;
    }


    public void ResetCost()
    {
        cost[1] = cost[0];  // Cost[0] is the base cost that never changes. The other costs change based on other units around the hex exerting zone of control.
        cost[2] = cost[0];
    }
}
