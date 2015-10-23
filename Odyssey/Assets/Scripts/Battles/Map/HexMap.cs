using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class HexMap : MonoBehaviour
{
    public static HexMap hex_map;
    public LevelFileParser parser = new LevelFileParser();

    [HideInInspector]
    public List<Hex> all_hexes = new List<Hex>();
    Dictionary<String, Hex> hex_dictionary = new Dictionary<String, Hex>();
    [HideInInspector]
    public List<Edge> all_edges = new List<Edge>();

    public int x_size, y_size;  // How tall and wide the map is. Set this in th
    int x_max, y_max, x_min, y_min; // Min and max coordinates for the hexes
    [HideInInspector]
	public float x_max_cam, y_max_cam, x_min_cam, y_min_cam;    // Camera boundaries
    float x_offset = 1.88f;//3.29f;     // Offset used for placing hexes
    float y_offset = 1.622f;//1.89f;
    List<Hex> hexes_in_range = new List<Hex>();


    public Hex GetHex(int x, int y)
    {
        Hex val;
        hex_dictionary.TryGetValue(x + "," + y, out val);
        return val;
    }


    void Awake ()
    {
        hex_map = this;
    }
	void Start ()
    {
        InitializeMap();
    }


    System.Random rand = new System.Random();
    string[] hex_types = new string[] { "Battles/Hexes/Hex", "Battles/Hexes/ForestHex"
        , "Battles/Hexes/HillHex", "Battles/Hexes/WaterHex"
        , "Battles/Hexes/RuinsHex", "Battles/Hexes/SwampHex" };
    string GetRandomHexPrefab()
    {
        return hex_types[rand.Next(0, hex_types.Length)];
    }
    string GetHexPrefabFromName(string name)
    {
        return "Battles/Hexes/" + name;
    }

    void InitializeMap()
    {
        // Read in the file stored in persistent battle settings
        if (PersistentBattleSettings.battle_settings != null)
        {
            Debug.Log("PersistentBattleSettings path: " + PersistentBattleSettings.battle_settings.GetProperPathToFile());
            parser.ReadInLevel(Application.dataPath + PersistentBattleSettings.battle_settings.GetProperPathToFile());
        }
        // No file to read in set. Must be debug mode. Just use my test file
        else
        {
            Debug.Log("No path in PersistentBattleSettings set. Using test file");
            parser.ReadInLevel(Application.dataPath + "/Resources/Battles/LevelFiles/Level1.txt");
        }

        y_size = parser.y_dimension;
        x_size = parser.x_dimension;
        int actual_x = -1;
        int actual_y = -1;
        int cur_hex = 0;
        Debug.Log("Map size: " + x_size + " " + y_size + ", number of hexes: " + parser.hex_types.Count);

        // Coordinates set up like http://stackoverflow.com/questions/5084801/manhattan-distance-between-tiles-in-a-hexagonal-grid/5085274#5085274
        // Reads hexes from top to bottom, left to right
        for (int y = y_size / 2; y >= -y_size / 2; y--)
        {
            actual_y++;
            for (int x = -x_size / 2; x <= x_size / 2; x++)
            {
                actual_x++;
                int x_coord = x;

                // Correct the  coordinates so we can continue to use our tilted axis, but the map looks evenly layed out
                Vector2 coords = GetCorrectedCoordinates(x, y);
                x_coord = (int) coords.x;

                if (cur_hex >= parser.hex_types.Count)
                    break;

                PotentialHex p_hex = parser.hex_types[cur_hex];
                // Spawn the right hex prefab by looking at the file we loaded
                GameObject instance = Instantiate(Resources.Load(
                    GetHexPrefabFromName(p_hex.hex_type)
                    , typeof(GameObject))) as GameObject;

                float x_pos = x_coord * x_offset + y * x_offset / 2;
                float y_pos = y * y_offset;
                instance.transform.parent = BattleManager.battle_manager.universal_battle_parent.transform;
                instance.transform.position = new Vector3(x_pos, y_pos, 1);
                Hex hex = instance.GetComponent<Hex>();
                hex.deployment_zone = p_hex.deployment_zone;
                hex.retreat_zone = p_hex.retreat_zone;
                hex.coordinate = new Vector2((int)x_coord, (int)y);
                hex.top_down_left_right_coordinate = new Vector2(actual_x, actual_y);
                all_hexes.Add(hex);
                hex_dictionary.Add((int)x_coord + "," + (int)y, hex);

                cur_hex++;

                // Give hex its terrain abilities
                switch (hex.h_name)
                {
                    case "Ruins":
                        hex.effects_on_hex.Add(new Ruins(null));
                        break;
                    case "Forest":
                        hex.effects_on_hex.Add(new Forest(null));
                        break;
                    case "Hill":
                        hex.effects_on_hex.Add(new Hill(null));
                        break;
                }

                // Add retreat ability and retreat art to hex
                if (hex.retreat_zone)
                {
                    GameObject r_instance = Instantiate(Resources.Load("Battles/Hexes/RetreatHexArt" , typeof(GameObject))) as GameObject;
                    r_instance.transform.parent = hex.gameObject.transform;
                    r_instance.transform.localScale = new Vector3(1, 1, 1);
                    r_instance.transform.position = new Vector3(x_pos, y_pos, 0.5f);
                    hex.effects_on_hex.Add(new RetreatHex(null));
                }


                // Set boundaries and camera boundaries
                if (x_pos > x_max_cam)
                {
                    x_max = (int)x;
                    x_max_cam = x_pos;
                }
                if (y_pos > y_max_cam)
                {
                    y_max = (int)y;
                    y_max_cam = y_pos;
                }

                if (x_pos < x_min_cam)
                {
                    x_min = x;
                    x_min_cam = x_pos;
                }
                if (y_pos < y_min_cam)
                {
                    y_min = y;
                    y_min_cam = y_pos;
                }
            }
            actual_x = -1;
        }

        // Set the actual number of hexes in each row and column
        x_size = actual_x + 1;
        y_size = actual_y + 1;
    }


    // Changes coordinates so the board lines up instead of being diagonal
    public Vector2 GetCorrectedCoordinates(int x, int y)
    {
        int x_coord = x;

        if (y > 0)
            x_coord = x - (y / 2);
        else if (y < 0)
            x_coord = x + (Mathf.Abs(y - 1) / 2);

        return new Vector2(x_coord, y);
    }
    public Vector2 GetUncorrectedCoordinates(int x, int y)
    {
        int x_coord = x;

        if (y > 0)
            x_coord = x + (y / 2);
        else if (y < 0)
            x_coord = x - (Mathf.Abs(y - 1) / 2);

        return new Vector2(x_coord, y);
    }
    public Hex GetHexFromTopDownCoordinates(Vector2 coordinate)
    {
        foreach (Hex hex in all_hexes)
        {
            if (hex.top_down_left_right_coordinate == coordinate)
                return hex;
        }
        Debug.Log("GetHexFromTopDownCoordinates no hex from with coordinates " + coordinate);
        return all_hexes[0];
    }


    // Resets the cost of all the edges in the hex map
    public void ResetEdgeScores()
    {
        all_edges.ToList().ForEach(c => { c.ResetCost(); });
    }


    // Checks if the two hexes are within range of eachother. No blocking line of sight is calculated
    public bool InRange(Hex from, Hex to, int range)
    {
        return InRange(from.coordinate, to.coordinate, range);
    }
    public bool InRange(Vector2 from, Vector2 to, int range)
    {
        return range >= DistanceBetweenHexes(from, to);
    }

    // Returns the number of hexes needed to get from A to B
    public int DistanceBetweenHexes(Vector2 from, Vector2 to)
    {
        /* Close but not quite. Doesn't work from one direction
        int delta_x = (int)Mathf.Abs(from.x - to.x);
        int delta_y = (int)Mathf.Abs(from.y - to.y);
        int delta_diff = delta_x + delta_y;     // Difference between delta x and delta y.
        return Mathf.Max(delta_x, delta_y, delta_diff);
        */
        // Based on math from http://stackoverflow.com/questions/5084801/manhattan-distance-between-tiles-in-a-hexagonal-grid/5085274#5085274
        int delta_x = (int) to.x - (int) from.x;
        int delta_y = (int)to.y - (int)from.y;
        if (Mathf.Sign(delta_x) == Mathf.Sign(delta_y))
            return Mathf.Abs(delta_x + delta_y);
        else
            return Mathf.Max(Mathf.Abs(delta_x), Mathf.Abs(delta_y));
    }


	void Update ()
    {
	
	}


    // Finds the most efficient path between two hexes, using the cost of the edges between the edges as the evaluator
    public List<Hex> AStarFindPath(Hex start, Hex finish, int maximum_cost_allowed, Faction faction)
    {
        // Reset hex search scores
        resetCellSearchScores();

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
            if (current == finish && current.g_score <= maximum_cost_allowed)
            {
                return constructPath(current, start);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Edge neighbourEdge in current.neighbours)
            {
                int tentative_g_score = current.g_score + neighbourEdge.GetCost(faction);

                // Check if have exceeded our allowed movement
                if (current.g_score >= maximum_cost_allowed || (closedSet.Contains(neighbourEdge.destination) && tentative_g_score >= neighbourEdge.destination.g_score))
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
        return new List<Hex>();
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


    // Checks if we can move between two hexes. Records all hexes checked for this path
    public List<Hex> AStarPathableToRecordHexes(Hex start, Hex finish, int maximum_cost_allowed, Faction faction)
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
            if (current == finish && current.g_score <= maximum_cost_allowed)
            {
                if (current.occupying_unit == null)
                {
                    hexes_in_range.Add(current);
                }
                return null;
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Edge neighbourEdge in current.neighbours)
            {
                int tentative_g_score = current.g_score + neighbourEdge.GetCost(faction);

                // Check if have exceeded our allowed movement
                if (current.g_score >= maximum_cost_allowed || (closedSet.Contains(neighbourEdge.destination) && tentative_g_score >= neighbourEdge.destination.g_score))
                {
                    continue;
                }

                if (current.occupying_unit == null)
                {
                    hexes_in_range.Add(current);
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
        return null;
    }


    public List<Hex> GetMovableHexesWithinRange(Hex location, int range, Unit unit)
    {
        hexes_in_range = new List<Hex>();

        int x = (int)location.coordinate.x;
        int xMin = Mathf.Max(x_min, x - range);
        int xMax = Mathf.Min(x_max, x + range);

        int y = (int)location.coordinate.y;
        int yMin = Mathf.Max(y_min, y - range);
        int yMax = Mathf.Min(y_max, y + range);

        for (int cur_x = xMin; cur_x <= xMax; cur_x++)
        {
            for (int cur_y = yMin; cur_y <= yMax; cur_y++)
            {
                Hex cur_hex;
                hex_dictionary.TryGetValue(cur_x + "," + cur_y, out cur_hex);
                // Boundary calculations aren't correct with tilted axis
                if (cur_hex != null && cur_hex.occupying_unit == null && !cur_hex.impassable && !hexes_in_range.Contains(cur_hex))
                    AStarPathableToRecordHexes(location, cur_hex, range, unit.owner);
            }
        }
        // Remove duplicate hexes
        hexes_in_range = hexes_in_range.Distinct().ToList();

        return hexes_in_range;
    }


    public List<Hex> HexesWithinRange(Hex location, int range)
    {
        List<Hex> hexes_in_range = new List<Hex>();

        int x = (int)location.coordinate.x;
        int xMin = Mathf.Max(x_min, x - range);
        int xMax = Mathf.Min(x_max, x + range * 2);

        int y = (int)location.coordinate.y;
        int yMin = Mathf.Max(y_min, y - range);
        int yMax = Mathf.Min(y_max, y + range);

        for (int cur_x = xMin; cur_x <= xMax; cur_x++)
        {
            for (int cur_y = yMin; cur_y <= yMax; cur_y++)
            {
                Hex cur_hex;
                hex_dictionary.TryGetValue(cur_x + "," + cur_y, out cur_hex);
                hexes_in_range.Add(cur_hex);
            }
        }

        return hexes_in_range;
    }
    public List<Hex> HexesWithinRangeContainingEnemies(Hex location, int range, Faction faction)
    {
        List<Hex> hexes_in_range = new List<Hex>();

        foreach (Unit enemy in faction.GetAllEnemyUnits())
        {
            if (this.InRange(location, enemy.location, range))
            {
                hexes_in_range.Add(enemy.location);
            }
        }

        return hexes_in_range;
        /*
        int x = (int)location.coordinate.x;
        int xMin = Mathf.Max(0, x - range * 2);
        int xMax = Mathf.Min(x_max, x + range * 2);

        int y = (int)location.coordinate.y;
        int yMin = Mathf.Max(0, y - range * 2);
        int yMax = Mathf.Min(y_max, y + range * 2);

        for (int cur_x = xMin; cur_x <= xMax; cur_x++)
        {
            for (int cur_y = yMin; cur_y <= yMax; cur_y++)
            {
                if (HexRange(location.coordinate, new Vector2(cur_x, cur_y), range))
                {
                    Hex cur_hex;
                    hex_dictionary.TryGetValue(cur_x + "," + cur_y, out cur_hex);
                    if (cur_hex.occupying_unit != null && faction.IsEnemy(cur_hex.occupying_unit))
                    {
                        hexes_in_range.Add(cur_hex);
                    }
                }
            }
        }

        return hexes_in_range;*/
    }
    public List<Hex> HexesWithinRangeContainingAllies(Hex location, int range, Faction faction)
    {
        List<Hex> hexes_in_range = new List<Hex>();

        foreach (Unit enemy in faction.GetAllAllyUnits())
        {
            if (this.InRange(location, enemy.location, range))
            {
                hexes_in_range.Add(enemy.location);
            }
        }

        return hexes_in_range;
        /*
        List<Hex> hexes_in_range = new List<Hex>();

        int x = (int)location.coordinate.x;
        int xMin = Mathf.Max(0, x - range * 2);
        int xMax = Mathf.Min(x_max, x + range * 2);

        int y = (int)location.coordinate.y;
        int yMin = Mathf.Max(0, y - range * 2);
        int yMax = Mathf.Min(y_max, y + range * 2);

        for (int cur_x = xMin; cur_x <= xMax; cur_x++)
        {
            for (int cur_y = yMin; cur_y <= yMax; cur_y++)
            {
                Hex cur_hex;
                hex_dictionary.TryGetValue(cur_x + "," + cur_y, out cur_hex);
                if (cur_hex.occupying_unit != null && faction.IsAlly(cur_hex.occupying_unit))
                    hexes_in_range.Add(cur_hex);
            }
        }

        return hexes_in_range;*/
    }


    public void WarpUnitTo(Unit unit, Hex destination)
    {
        unit.SetLocation(destination);
        unit.transform.position = destination.transform.position;
        unit.transform.position = new Vector3(unit.transform.position.x, unit.transform.position.y, 0);
    }


    // Returns the nearest unoccupied passable hex to the destination
    public Hex Nearest_Unoccupied_Passable_Hex(Hex destination)
    {
        if (!destination.impassable && destination.occupying_unit == null)
            return destination;

        for (int radius = 1; radius < 10; radius++)
        {
            foreach (Hex hex in HexesWithinRange(destination, radius))
            {
                if (!hex.impassable && hex.occupying_unit == null)
                    return hex;
            }
        }

        Debug.Log("Nearest_Unoccupied_Passable_Hex couldn't find free hex around " + destination.coordinate);
        return destination;
    }


    // Destroys all game objects associated with the hex map
    public void UnloadHexMap()
    {

    }


    public void UndarkenDeploymentHexes()
    {
        all_hexes.ForEach(c => { if (c.deployment_zone) c.gameObject.transform.FindChild("UndeployableHex").gameObject.SetActive(false); });
    }


    public void UndarkenAllHexes()
    {
        all_hexes.ForEach(c => { c.gameObject.transform.FindChild("UndeployableHex").gameObject.SetActive(false); });
    }
}
