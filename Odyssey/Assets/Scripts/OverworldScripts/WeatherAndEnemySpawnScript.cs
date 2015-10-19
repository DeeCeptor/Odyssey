using UnityEngine;
using System.Collections;
public class WeatherAndEnemySpawnScript : MonoBehaviour {
    public GameObject[] weather;
    public GameObject[] npcs;
    public GameObject[] enemies;
    public float minNPCDistance = 20f;
    public float maxNPCDistance = 50f;
    public float maxWeatherDistance = 20f;
    public bool paused = false;
    public Collider[] collidingWith;
    //random int to get a random element of an array
    private int randInt;

    private int weatherCounter = 0;
    public int weatherSpawnDelay = 600;

    private int npcCounter = 0;
    public int npcSpawnDelay = 300;
    //max npcs
    public int npcNumber = 8;
    // a vector to store random unit sphere vectors
    private Vector3 rando;
    private LayerMask mask;


    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!paused)
        {
            if (weatherCounter > weatherSpawnDelay)
            {
                SpawnWeather();
            }

            if (npcCounter > npcSpawnDelay)
            {
                SpawnNPC();
            }
           
            weatherCounter++;
            npcCounter++;
        }
	}

    public void SpawnNPC()
    {
        enemies = GameObject.FindGameObjectsWithTag("OverworldEnemy");
        if(enemies.Length >= npcNumber)
        {
            return;
        }
        npcCounter = 0;
        rando = Random.insideUnitSphere;
        randInt = Random.Range(0, npcs.Length);
        mask = 1 << 10;
        mask = ~(mask);
        Vector3 randPoint = transform.position + (new Vector3(rando.x, 0, rando.z).normalized * Random.Range(minNPCDistance, maxNPCDistance));
        if (npcs.Length > 0)
        {
            collidingWith = Physics.OverlapSphere(randPoint, npcs[randInt].GetComponent<Collider>().bounds.size.x, mask);
        }
        if (collidingWith.Length > 0)
        {
            SpawnNPC();
            return;
        }
        Vector3 screenPoint = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToViewportPoint(randPoint);
        if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
        {

            SpawnNPC();
            return;
        }
        GameObject npc = (GameObject)Instantiate(npcs[randInt],randPoint, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
        npc.transform.parent = GameObject.FindGameObjectWithTag("UniversalParent").transform;
        
    }

    public void SpawnWeather()
    {
        weatherCounter = 0;
        rando = Random.insideUnitSphere;
        randInt = Random.Range(0, weather.Length);
        mask = 1 << 10;
        Vector3 randPoint = transform.position + (new Vector3(rando.x, 0, rando.z).normalized * Random.Range(0, maxWeatherDistance));
        collidingWith = Physics.OverlapSphere(randPoint, weather[randInt].GetComponent<Collider>().bounds.size.x, mask);
        if (collidingWith.Length > 0)
        {
            return;
        }
        GameObject weatherSpawned = (GameObject)Instantiate(weather[randInt], randPoint + new Vector3(0, weather[randInt].GetComponent<WeatherScript>().weatherHeight,0), Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
        weatherSpawned.transform.parent = GameObject.FindGameObjectWithTag("UniversalParent").transform;

    }

    public void Pause()
    {
        paused = true;
    }

    public void Unpause()
    {
        paused = false;
    }
}
