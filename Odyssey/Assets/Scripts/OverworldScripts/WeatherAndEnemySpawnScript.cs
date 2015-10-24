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
    public Collider2D[] collidingWith;
    public Collider2D[] collidingWithWeather;
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
        rando = Random.insideUnitCircle;
        randInt = Random.Range(0, npcs.Length);
        mask = 1 << 10;
        mask = ~(mask);
        Vector3 randPoint = transform.position + (new Vector3(rando.x, rando.y, 0).normalized * Random.Range(minNPCDistance, maxNPCDistance));
        if (npcs.Length > 0)
        {
            collidingWith = Physics2D.OverlapCircleAll(randPoint, npcs[randInt].GetComponent<Collider2D>().bounds.size.x, mask);
        }
        if (collidingWith.Length > 0)
        {
            SpawnNPC();
            return;
        }
        GameObject npc = (GameObject)Instantiate(npcs[randInt],randPoint, transform.rotation);
        npc.transform.Rotate(transform.forward, Random.Range(0, 360), Space.Self);
        npc.transform.parent = GameObject.FindGameObjectWithTag("UniversalParent").transform;
        
    }

    public void SpawnWeather()
    {
        weatherCounter = 0;
        rando = Random.insideUnitCircle;
        randInt = Random.Range(0, weather.Length);
        mask = 1 << 10;
        Vector3 randPoint = transform.position + (new Vector3(rando.x, rando.y, 0).normalized * Random.Range(0, maxWeatherDistance));
        collidingWithWeather = Physics2D.OverlapCircleAll(randPoint, weather[randInt].GetComponent<Collider2D>().bounds.size.x, mask);
        if (collidingWith.Length > 0)
        {
            return;
        }
        GameObject weatherSpawned = (GameObject)Instantiate(weather[randInt], randPoint + new Vector3(0, 0, weather[randInt].GetComponent<WeatherScript>().weatherHeight), Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward));
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
