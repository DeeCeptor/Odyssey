using UnityEngine;
using System.Collections;

[System.Serializable]
public class World : MonoBehaviour {
    public GameObject[] Islands;
    public GameObject player;
    public GameObject[] weather;
    public GameObject[] npcs;

    public static World curWorld;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void getCurrent()
    {
        Islands = GameObject.FindGameObjectsWithTag("Island");
        player = GameObject.FindGameObjectWithTag("Player");
        weather = GameObject.FindGameObjectsWithTag("Weather");
        npcs = GameObject.FindGameObjectsWithTag("OverworldEnemy");
    }

    public World()
    {
        Islands = GameObject.FindGameObjectsWithTag("Island");
        player = GameObject.FindGameObjectWithTag("Player");
        weather = GameObject.FindGameObjectsWithTag("Weather");
        npcs = GameObject.FindGameObjectsWithTag("OverworldEnemy");
    }

}
