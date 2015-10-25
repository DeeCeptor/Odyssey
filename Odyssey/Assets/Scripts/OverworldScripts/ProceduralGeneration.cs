using UnityEngine;
using System.Collections;

public class ProceduralGeneration : MonoBehaviour {
//Objects to spawn
public GameObject ocean;
public GameObject player;
public GameObject[] islands;
public GameObject goal;
public GameObject[] npcs;
public GameObject eventController;
public GameObject ui;
public GameObject UniversalParent;

public int maxIslands = 40;
public float islandSpace = 50;
public float goalSpace = 200;
public int maxIterationFactor = 5000;
float oceanXWidth;
float oceanYWidth;
float islandXWidth;
float islandYWidth;
int islandCounter = 0;
Vector3 curPosition;
GameObject curIsland;
    public GameObject[] islandsPlaced;


	
	//if the position is valid
	bool noGood = false;

	// Use this for initialization
	void Start () {
        islandsPlaced = new GameObject[maxIslands + 1];
        UniversalParent = GameObject.FindGameObjectWithTag("UniversalParent");
	    GenerateWorld();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void GenerateWorld()
	{
        GameObject newUI = (GameObject)Instantiate(ui, transform.position, transform.rotation);
        newUI.transform.parent = UniversalParent.transform;
        ocean = (GameObject)Instantiate(ocean, transform.position, transform.rotation);
        ocean.transform.parent = UniversalParent.transform;
        ocean.transform.Rotate(transform.right, 90, Space.Self);
        PlaceGoal();
	    PlaceIslands();
	    PlacePlayer();
    	GameObject eventHandler = (GameObject)Instantiate(eventController);
        eventHandler.GetComponent<EventManagement>().islands = islandsPlaced;
	    
    }
	
	public void PlaceGoal()
	{
        oceanXWidth = ocean.GetComponent<Renderer>().bounds.extents.x;
        oceanYWidth = ocean.GetComponent<Renderer>().bounds.extents.y;
        islandXWidth = goal.GetComponent<Renderer>().bounds.extents.x;
        islandYWidth = goal.GetComponent<Renderer>().bounds.extents.y;
        curPosition = new Vector3(Random.Range(-oceanXWidth + islandXWidth, oceanXWidth - islandXWidth), Random.Range(-oceanYWidth + islandYWidth, oceanYWidth - islandYWidth) ,transform.position.z + 5);
        goal = (GameObject)Instantiate(goal, curPosition, transform.rotation);
        goal.transform.parent = UniversalParent.transform;
        islandCounter = islandCounter + 1;
        islandsPlaced[0] = goal;
    }

    public void PlacePlayer()
	{
        //place player far from goal
	noGood = true;
	while(noGood)
		{
			noGood = false;
			curIsland = islands[Random.Range (0,islands.Length)];
			islandXWidth = curIsland.GetComponent<Renderer>().bounds.extents.x;
			islandYWidth = curIsland.GetComponent<Renderer>().bounds.extents.y;
			curPosition = new Vector3(Random.Range(-oceanXWidth+islandXWidth,oceanXWidth-islandXWidth), Random.Range(-oceanYWidth + islandYWidth, oceanYWidth - islandYWidth), transform.position.z);
			
			Collider2D[] objectsNear = Physics2D.OverlapCircleAll(curPosition, islandSpace/5);
			for(int x = 0; x< objectsNear.Length;x++)
			{
				if (objectsNear[x].gameObject.tag.Equals("Island"))
				{
					noGood = true;
				}
			}

            if((goal.transform.position - curPosition).magnitude < goalSpace)
            {
                noGood = true;
            }

            if (!noGood)
			{
				GameObject pShip = (GameObject)Instantiate(player,curPosition,transform.rotation);
				pShip.transform.Rotate(transform.forward,Random.Range(0,360),Space.Self);
                pShip.transform.parent = UniversalParent.transform;
            }
		}
	}
	
	public void PlaceEnemies()
	{
	}
	
	public void PlaceIslands()
	{
	
	//just to make sure it has an endpoint if number of islands is greater than can fit
	int i = 0;
	while(islandCounter<maxIslands && i < maxIslands*maxIterationFactor)
	{
		curIsland = islands[Random.Range (0,islands.Length)];
		islandXWidth = curIsland.GetComponent<Renderer>().bounds.extents.x;
		islandYWidth = curIsland.GetComponent<Renderer>().bounds.extents.y;
		curPosition = new Vector3(Random.Range(-oceanXWidth+islandXWidth,oceanXWidth-islandXWidth), Random.Range(-oceanYWidth + islandYWidth, oceanYWidth - islandYWidth), transform.position.z + 5);

            for (int x = 0; x < islandsPlaced.Length; x++)
            {
                if (islandsPlaced[x] != null)
                {
                    if ((islandsPlaced[x].transform.position - curPosition).magnitude <= islandSpace)
                    {
                        noGood = true;
                    }
                }
            }
        if (!noGood)
		{
                GameObject newIsland = (GameObject)Instantiate(curIsland, curPosition, transform.rotation);
                newIsland.transform.parent = UniversalParent.transform;
                islandCounter = islandCounter+1;
                islandsPlaced[islandCounter] = newIsland;
		}
		
		noGood = false;
		i++;
	}
	
	
	
	}
}
