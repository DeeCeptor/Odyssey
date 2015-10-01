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

public int maxIslands = 40;
public float islandSpace = 50;
public float goalSpace = 200;
public int maxIterationFactor = 5000;
float oceanXWidth;
float oceanZWidth;
float islandXWidth;
float islandZWidth;
int islandCounter = 0;
Vector3 curPosition;
GameObject curIsland;

	
	//if the position is valid
	bool noGood = false;

	// Use this for initialization
	void Start () {
	GenerateWorld();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void GenerateWorld()
	{
	    Instantiate(ocean);
        PlaceGoal();
	    PlaceIslands();
	    PlacePlayer();
    	Instantiate(eventController);
	    GameObject newUI = (GameObject)Instantiate(ui,transform.position,transform.rotation);
	
	}
	
	public void PlaceGoal()
	{
        oceanXWidth = ocean.GetComponent<Renderer>().bounds.extents.x;
        oceanZWidth = ocean.GetComponent<Renderer>().bounds.extents.z;
        islandXWidth = goal.GetComponent<Renderer>().bounds.extents.x;
        islandZWidth = goal.GetComponent<Renderer>().bounds.extents.z;
        curPosition = new Vector3(Random.Range(-oceanXWidth + islandXWidth, oceanXWidth - islandXWidth), transform.position.y, Random.Range(-oceanZWidth + islandZWidth, oceanZWidth - islandZWidth));
        goal = (GameObject)Instantiate(goal, curPosition, transform.rotation);
        islandCounter = islandCounter + 1;

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
			islandZWidth = curIsland.GetComponent<Renderer>().bounds.extents.z;
			curPosition = new Vector3(Random.Range(-oceanXWidth+islandXWidth,oceanXWidth-islandXWidth),transform.position.y,Random.Range(-oceanZWidth+islandZWidth,oceanZWidth-islandZWidth));
			
			Collider[] objectsNear = Physics.OverlapSphere(curPosition, islandSpace/5);
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
				pShip.transform.Rotate(transform.up,Random.Range(0,360),Space.Self);
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
		islandZWidth = curIsland.GetComponent<Renderer>().bounds.extents.z;
		curPosition = new Vector3(Random.Range(-oceanXWidth+islandXWidth,oceanXWidth-islandXWidth),transform.position.y-5,Random.Range(-oceanZWidth+islandZWidth,oceanZWidth-islandZWidth));
		
		Collider[] objectsNear = Physics.OverlapSphere(curPosition, islandSpace);
		for(int x = 0; x< objectsNear.Length;x++)
		{
			if (objectsNear[x].gameObject.tag.Equals("Island"))
			{
				noGood = true;
			}
		}
		
		if(!noGood)
		{
		Instantiate(curIsland,curPosition,transform.rotation);
		islandCounter = islandCounter+1;
		}
		
		noGood = false;
		i++;
	}
	
	
	
	}
}
