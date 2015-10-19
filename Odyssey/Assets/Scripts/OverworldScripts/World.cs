using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {
   public Transform[] Islands;
    public string[] islandPrefabs;

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
