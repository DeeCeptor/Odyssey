using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {
    //island data
   public Transform[] Islands;
    public string[] islandPrefabs;
    public float[] gatheringPercents;
    public bool[] explored;

    //playerData
    public Transform playerBoat;
    public string playerPrefab;
    public float food;
    public float water;
    public float treasure;
    public float wood;
    public float hull;
    public int sailors;
    public float stamina;
    public float morale;
    public List<string> upgradesBought;
    public float poseidonsFavour;
    public float zeusFavour;
    public float hadesFavour;
    public float athenasFavour;
    public float aresFavour;
    public float health;

    //weatherData
    public Transform[] weather;
    public string[] weatherPrefabs;
    public float[] dieTime;

    //npcData
    public Transform[] npcs;
    public string[] npcPrefabs;

    //event Controller data
    public int eventTimer;
    public int eventTimelimit;

    //  public GameObject player;
    //  public GameObject[] weather;
    //  public GameObject[] npcs;
    //    public GameObject eventManager;

    public static World curWorld;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void getCurrent()
    {
        //eventManager = GameObject.FindGameObjectWithTag("EventController");
      //  player = GameObject.FindGameObjectWithTag("Player");
      //  weather = GameObject.FindGameObjectsWithTag("Weather");
       // npcs = GameObject.FindGameObjectsWithTag("OverworldEnemy");
    }

    public World()
    {
      //  curWorld = this;
  //      player = GameObject.FindGameObjectWithTag("Player");
     //   weather = GameObject.FindGameObjectsWithTag("Weather");
     //   npcs = GameObject.FindGameObjectsWithTag("OverworldEnemy");
    }

}
