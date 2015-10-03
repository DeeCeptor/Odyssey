using UnityEngine;
using System.Collections;

public class WeatherScript : MonoBehaviour {
    public string weatherType;
    //damage to hull if ship not anchored
    public int weatherDamage = 0;

    private float counterToDie = 0;
    public int LifeSpan = 240;
    public GameObject ReplaceWith;
    public bool paused = false;

    //scalar float to multiply speed by
    public float weatherSpeedDown = 0;
    public float weatherStaminaDrain = 0;
    public Vector3 weatherDirection;
    public float weatherMoraleDrain = 0;
    //height to spawn weather at
    public float weatherHeight = 40f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!paused)
        {
            if (counterToDie > LifeSpan)// && (!this.GetComponent<Renderer>().isVisible))
            {
                if (ReplaceWith != null)
                {
                    Instantiate(ReplaceWith, transform.position, transform.rotation);
                }
                Destroy(gameObject);
            }
            counterToDie = counterToDie + 1;
        }
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
