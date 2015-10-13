using UnityEngine;
using System.Collections;

public class RandomWeatherVector : WeatherScript {

    public float weatherSpeed = 0.5f;
	// Use this for initialization
	void Start () {
        weatherDirection = Random.insideUnitCircle.normalized * weatherSpeed;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
	}
}
