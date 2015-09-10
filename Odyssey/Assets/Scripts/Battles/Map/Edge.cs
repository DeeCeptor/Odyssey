using UnityEngine;
using System.Collections;

public class Edge 
{
    public Hex destination;
    public Hex source;
    public int cost;

    public Edge(Hex destination_hex, Hex source_hex, int movementCost)
    {
        destination = destination_hex;
        source = source_hex;
        cost = movementCost;
    }
}
