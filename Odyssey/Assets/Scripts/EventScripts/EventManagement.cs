using UnityEngine;
using System.Collections;

public class EventManagement : MonoBehaviour {
	
	public GameObject player;
	public GameObject[] enemies;
    public GameObject[] weather;
	public ResourceManager resourceController;
	public PlayerBoatController playerController;
	public GameObject currentEvent;
	public GameObject islandEventIsOn;

	public GameObject[] seaEventList;
	public GameObject[] campingEventList;
	public GameObject[] forestExploreEventList;
	public GameObject[] desertExploreEventList;
	public GameObject[] tropicalExploreEventList;
	
	public int framesPerCheckRegular = 240;
	public int framesPerCheck = 240;
	private int frameIterator = 0;
	public float eventChance = 25f;
	public bool paused = false;
	
	// Use this for initialization
	void Start () {
	player = GameObject.FindGameObjectWithTag("Player");
	resourceController = player.GetComponent<ResourceManager>();
	playerController = player.GetComponent<PlayerBoatController>();
	enemies = GameObject.FindGameObjectsWithTag("OverworldEnemy");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(paused == false)
			{
			if(frameIterator>framesPerCheck)
				{
				frameIterator = 0;
				framesPerCheck = framesPerCheckRegular + Random.Range(0,121);
				if(Random.Range (0,100) <eventChance)
				{
					HaveEvent(seaEventList[Random.Range (0,seaEventList.Length)]);
				}
				
				}
			frameIterator = frameIterator + 1;
			}
	}
	
	public void HaveEvent(GameObject eventToHave)
	{
        
		currentEvent = Instantiate(eventToHave);
		Pause();
	}
	
	
	
	public void HaveIslandEvent(GameObject eventToHave,GameObject island)
	{
		islandEventIsOn = island;
		currentEvent = Instantiate(eventToHave);
		Pause();
	}
	
	public void ExploreForestIsland()
	{
		islandEventIsOn.GetComponent<IslandEventScript>().explored = true;
		HaveEvent(forestExploreEventList[Random.Range (0,forestExploreEventList.Length)]);
	}
	
	public void ExploreDesertIsland()
	{
		islandEventIsOn.GetComponent<IslandEventScript>().explored = true;
		HaveEvent(desertExploreEventList[Random.Range (0,desertExploreEventList.Length)]);
	}
	
	public void ExploreTropicalIsland()
	{
		islandEventIsOn.GetComponent<IslandEventScript>().explored = true;
		HaveEvent(tropicalExploreEventList[Random.Range (0,tropicalExploreEventList.Length)]);
	}
	
	public void CampEvent()
	{
		HaveEvent(campingEventList[Random.Range (0,campingEventList.Length)]);
	}
	
	public void EndEvent()
	{
	Unpause();
	Destroy(currentEvent);
	}

	public void Pause()
	{
        weather = GameObject.FindGameObjectsWithTag("Weather");
        enemies = GameObject.FindGameObjectsWithTag("OverworldEnemy");
        paused = true;
		resourceController.Pause();
		playerController.Pause();
		for(int i = 0; i < enemies.Length;i++)
		{
		enemies[i].GetComponent<OverworldEnemyScript>().Pause();
		}
        for (int i = 0; i < weather.Length; i++)
        {
            weather[i].GetComponent<WeatherScript>().Pause();
        }
        player.GetComponentInChildren<WeatherAndEnemySpawnScript>().Pause();
    }
	
	public void Unpause()
	{
        weather = GameObject.FindGameObjectsWithTag("Weather");
        enemies = GameObject.FindGameObjectsWithTag("OverworldEnemy");
        paused = false;
		resourceController.Unpause();
		if(!resourceController.anchored)
		{
		playerController.Unpause();
		}
		for(int i = 0; i < weather.Length;i++)
		{
            weather[i].GetComponent<WeatherScript>().Unpause();
        }
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<OverworldEnemyScript>().Unpause();
        }
        player.GetComponentInChildren<WeatherAndEnemySpawnScript>().Unpause();
    }

    public void StartBattle(string battleToStart,bool retreat,bool mustUsehero,int deployNumber)
    {
        GameObject.FindGameObjectWithTag("UniversalParent").SetActive(false);
        GameObject battleSettings = (GameObject)Instantiate(Resources.Load("/Resources/Battles/PersistentBattleSettings"));
        PersistentBattleSettings battleScript = battleSettings.GetComponent<PersistentBattleSettings>();
        battleScript.path_to_battle_file = battleToStart;
        battleScript.number_of_deployable_units = deployNumber;
        battleScript.must_include_main_hero = mustUsehero;
        battleScript.can_retreat = retreat;
        Pause();
    }

    public void EndBattle()
    {
        //destroy battle
        Unpause();
        GameObject.FindGameObjectWithTag("UniversalParent").SetActive(true);
    }
	
}
