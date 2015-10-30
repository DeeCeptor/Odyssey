using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public GameObject[] gatherEventList;
    public GameObject[] islands;
    public GameObject persistentBattleObject;
    public PersistentBattleSettings persistentBattleSettings;

    //variables to store battleReward
    public bool rewardClaimed = false;
    public float rewardFood = 0f;
    public float rewardWater = 0f;
    public int rewardGold = 0;
    public int rewardSailors = 0;
    public Dictionary<string,int>unitsReward;
    public GameObject heroReward;
    public bool gameOverIfLose = false;
    public bool victoryIfWin = false;
	
	public int framesPerCheckRegular = 240;
	public int framesPerCheck = 240;
	private int frameIterator = 0;
	public float eventChance = 25f;
	public bool paused = false;
    public static EventManagement gameController;
    public GameObject OverworldParent;

    public bool battleChecked = true;

	// Use this for initialization
	void Start () {
	player = GameObject.FindGameObjectWithTag("Player");
	resourceController = player.GetComponent<ResourceManager>();
	playerController = player.GetComponent<PlayerBoatController>();
	enemies = GameObject.FindGameObjectsWithTag("OverworldEnemy");
        gameController = this;
        OverworldParent = GameObject.FindGameObjectWithTag("UniversalParent");
        unitsReward = new Dictionary<string, int>();
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
                    if (!resourceController.gathering)
                    {
                        HaveEvent(seaEventList[Random.Range(0, seaEventList.Length)]);
                    }
                    else
                    {
                        HaveEvent(gatherEventList[Random.Range(0, seaEventList.Length)]);
                    }
				}
				
				}
			frameIterator = frameIterator + 1;
			}
	}
	
	public void HaveEvent(GameObject eventToHave)
	{
		currentEvent = Instantiate(eventToHave);
        currentEvent.transform.parent = GameObject.FindGameObjectWithTag("UniversalParent").transform;
		Pause();
	}
	
	
	
	public void HaveIslandEvent(GameObject eventToHave,GameObject island)
	{
		islandEventIsOn = island;
		currentEvent = Instantiate(eventToHave);
        currentEvent.transform.parent = GameObject.FindGameObjectWithTag("UniversalParent").transform;
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
            if (enemies[i] != null)
            {
                enemies[i].GetComponent<OverworldEnemyScript>().Unpause();
            }
        }
        player.GetComponentInChildren<WeatherAndEnemySpawnScript>().Unpause();
    }

    public void StartBattle(string battleToStart,bool retreat,bool mustUsehero,int deployNumber)
    {
        rewardClaimed = false;
        battleChecked = false;
        Instantiate(Resources.Load("ScrollTransitionCanvas"));
        OverworldParent.SetActive(false);
        persistentBattleObject = (GameObject)Instantiate(Resources.Load("Battles/PersistentBattleSettings"));
        persistentBattleSettings = persistentBattleObject.GetComponent<PersistentBattleSettings>();
        persistentBattleSettings.path_to_battle_file = battleToStart;
        persistentBattleSettings.number_of_deployable_units = deployNumber + TroopManager.playerTroops.getLeadership();
        persistentBattleSettings.must_include_main_hero = mustUsehero;
        persistentBattleSettings.can_retreat = retreat;
        Application.LoadLevelAdditive("TacticalBattle");
        paused = true;
    }

    public void StartBattle(string battleToStart, bool retreat, bool mustUsehero, int deployNumber,bool firstTurn,int AIagressiveness)
    {
        rewardClaimed = false;
        battleChecked = false;
        Instantiate(Resources.Load("ScrollTransitionCanvas"));
        OverworldParent.SetActive(false);
        persistentBattleObject = (GameObject)Instantiate(Resources.Load("Battles/PersistentBattleSettings"));
        persistentBattleSettings = persistentBattleObject.GetComponent<PersistentBattleSettings>();
        persistentBattleSettings.path_to_battle_file = battleToStart;
        persistentBattleSettings.number_of_deployable_units = deployNumber + TroopManager.playerTroops.getLeadership();
        persistentBattleSettings.must_include_main_hero = mustUsehero;
        persistentBattleSettings.can_retreat = retreat;
        persistentBattleSettings.player_goes_first = firstTurn;
        persistentBattleSettings.enemy_agressiveness = AIagressiveness;
        Application.LoadLevelAdditive("TacticalBattle");
        paused = true;
    }

    public void EndBattle()
    {
        //destroy battle
        battleChecked = true;
        paused = false;
        OverworldParent.SetActive(true);
        if(persistentBattleSettings.game_over)
        {
            GameOver();
        }
        Dictionary<string, Casualty>.KeyCollection keys = persistentBattleSettings.casualties[0].Keys;
        Dictionary<string, Casualty>.ValueCollection woundedUnits = persistentBattleSettings.casualties[0].Values;
        Casualty[] woundedTroopNums = new Casualty[woundedUnits.Count];
        string[] keyArray = new string[keys.Count];
        string curKey;
        woundedUnits.CopyTo(woundedTroopNums, 0);
        keys.CopyTo(keyArray, 0);
        for(int i = 0; i < woundedTroopNums.Length;i++)
        {
            curKey = keyArray[i];
            TroopManager.playerTroops.healthy[curKey] = TroopManager.playerTroops.healthy[curKey] - woundedTroopNums[i].num_killed - woundedTroopNums[i].num_wounded;
            TroopManager.playerTroops.wounded[curKey] = TroopManager.playerTroops.wounded[curKey] + woundedTroopNums[i].num_wounded;
        }

        if(!rewardClaimed && persistentBattleSettings.victory)
        {
            rewardClaimed = true;
            resourceController.AddFood(rewardFood);
            resourceController.AddWater(rewardWater);
            resourceController.AddTreasure(rewardGold);
            resourceController.AddTreasure(rewardGold);
            TroopManager.playerTroops.AddHero(heroReward);
            Dictionary<string, int>.KeyCollection rewardKeys = unitsReward.Keys;
            keyArray = new string[keys.Count];
            rewardKeys.CopyTo(keyArray, 0);
            for (int i = 0; i < unitsReward.Count; i++)
            {
                curKey = keyArray[i];
                if(TroopManager.playerTroops.healthy.ContainsKey(curKey))
                {
                    TroopManager.playerTroops.healthy[curKey] += unitsReward[curKey];
                }
                else
                {
                    TroopManager.playerTroops.healthy.Add(curKey, unitsReward[curKey]);
                }
                
            }
            rewardFood = 0f;
            rewardWater = 0f;
            rewardGold = 0;
            rewardSailors = 0;
            unitsReward.Clear();
            heroReward = null;
            
        }
        
    }
	
    public void GameOver()
    {

    }
}
