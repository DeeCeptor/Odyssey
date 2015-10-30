using UnityEngine;
using System.Collections;

public class WolvesEventScript : MonoBehaviour {

    public int menToLoseMax = 3;
	// Use this for initialization
	void Start () {
        TroopManager.playerTroops.killRandom(Random.Range(1,menToLoseMax));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
