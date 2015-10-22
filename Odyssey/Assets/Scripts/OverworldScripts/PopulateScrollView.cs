using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopulateScrollView : MonoBehaviour {
    public int scrollItemHeight;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnEnable () {
        string[] keyArray = new string[] { };
        string curKey;
        int[] healthyTroopNums = new int[] { };
        int[] woundedTroopNums = new int[] { };
        Dictionary<string, int>.KeyCollection keys = TroopManager.playerTroops.healthy.Keys;
        keys.CopyTo(keyArray, 0);
        Dictionary<string, int>.ValueCollection healthyUnits = TroopManager.playerTroops.healthy.Values;
        healthyUnits.CopyTo(healthyTroopNums, 0);
        Dictionary<string, int>.ValueCollection woundedUnits = TroopManager.playerTroops.wounded.Values;
        woundedUnits.CopyTo(woundedTroopNums,0);
        


    }
}
