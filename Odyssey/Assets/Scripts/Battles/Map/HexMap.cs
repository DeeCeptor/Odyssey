using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class HexMap : MonoBehaviour
{
    public static HexMap hex_map;

    List<Hex> all_hexes;


	void Start ()
    {
        hex_map = this;
        InitializeMap();
    }
	

    void InitializeMap()
    {
        float x = 0;
        for (int y = 0; y < 10; y++)
        {
            // Get hex offset of rows
            if (y % 2 == 0)
                x = 0.5f;
            else
                x = 0;

            while (x < 10.6)
            {
                GameObject instance = Instantiate(Resources.Load("Battles/Hex", typeof(GameObject))) as GameObject;
                instance.transform.position = new Vector3(x, 0, y);
                Hex hex = instance.GetComponent<Hex>();
                hex.coordinate = new Vector2((int) x, (int) y);
                x++;
            }
        }
    }


	void Update ()
    {
	
	}


    // Finds the most efficient path between two hexes, using the cost of the edges between the edges as the evaluator
    public List<Hex> AStarFindPath(Hex start, Hex finish, int movementAllowed)
    {
        // Reset hex search scores

        List<Hex> closedSet = new List<Hex>();
        List<Hex> openSet = new List<Hex>();
        openSet.Add(start);

        start.came_from = null;
        start.g_score = 0;  // Cost of best known path
        start.f_score = start.g_score + estimatedCost(start.coordinate, finish.coordinate);  // Estimated cost of path from start to finish


        // Keep going until openset is empty
        while (openSet.Count > 0)
        {
            openSet.Sort();
            Hex current = openSet[0];

            // Check if we found the goal
            if (current == finish)
            {
                return constructPath(current, start);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Edge neighbourEdge in current.neighbours)
            {
                int tentative_g_score = current.g_score + neighbourEdge.cost;

                if (closedSet.Contains(neighbourEdge.destination) && tentative_g_score >= neighbourEdge.destination.g_score)
                {
                    continue;
                }

                if (!openSet.Contains(neighbourEdge.destination) || tentative_g_score < neighbourEdge.destination.g_score)
                {
                    neighbourEdge.destination.came_from = current;

                    neighbourEdge.destination.g_score = tentative_g_score;
                    neighbourEdge.destination.f_score = neighbourEdge.destination.g_score +
                        estimatedCost(neighbourEdge.destination.coordinate, finish.coordinate);

                    if (!openSet.Contains(neighbourEdge.destination))
                    {
                        openSet.Add(neighbourEdge.destination);
                        neighbourEdge.destination.came_from = current;
                    }
                }
            }
        }

        // Return failure
        Debug.Log("Failed to find path between " + start.coordinate + " and " + finish.coordinate);
        return null;
    }


    /**
    * Creates a path by going from the goal and retracing its steps.  Each cell recorded
    * its predecessor, so now we just inverse the order and return the list of steps.
    */
    private List<Hex> constructPath(Hex startCell, Hex endCell)
    {
        Hex curCell = startCell;
        List<Hex> path = new List<Hex>();
        path.Add(startCell);

        while (curCell != endCell)
        {
            curCell = curCell.came_from;
            path.Insert(0, curCell);
        }

        return path;
    }


    public int estimatedCost(Vector2 start, Vector2 finish)
    {
        return (int)(Mathf.Abs(finish.x - start.x) + Mathf.Abs(finish.y + finish.y));
    }


    // Use LINQ to set the value of each hex in the list
    public void resetCellSearchScores()
    {
        all_hexes.ToList().ForEach(c => { c.f_score = 0; c.g_score = 0; });
    }



    public void WarpUnitTo(Unit unit, Hex destination)
    {
        destination.occupying_unit = unit;
        unit.location = destination;

        unit.transform.position = destination.transform.position;
    }
}
