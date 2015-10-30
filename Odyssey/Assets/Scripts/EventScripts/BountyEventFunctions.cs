using UnityEngine;
using System.Collections;

public class BountyEventFunctions : MonoBehaviour {
    public float waterToGain = 20f;
    public float foodToGain = 20f;
	// Use this for initialization
	void Start () {
        ResourceManager.playerResources.AddWater(waterToGain);
        ResourceManager.playerResources.AddFood(foodToGain);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
