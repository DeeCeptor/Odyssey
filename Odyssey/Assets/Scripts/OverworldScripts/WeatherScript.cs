using UnityEngine;
using System.Collections;

public class WeatherScript : MonoBehaviour {
    public string weatherType;
    //damage to hull if ship not anchored
    public int weatherDamage = 0;

    //scalar float to multiply speed by
    public float weatherSpeedDown = 0;
    public float weatherStaminaDrain = 0;
    public Vector3 weatherDirection;
    public float weatherMoraleDrain = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
