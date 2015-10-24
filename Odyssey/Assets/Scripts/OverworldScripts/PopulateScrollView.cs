using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PopulateScrollView : MonoBehaviour {
    public int scrollItemHeight;
    public GameObject buttonPrefab;
    public float buttonHeight = 40;
    public float buttonSpace = 5;
    //space to move button left
    public float leftAdjust = 50;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnEnable () {
        
        Dictionary<string, int>.KeyCollection keys = TroopManager.playerTroops.healthy.Keys;
        Dictionary<string, int>.ValueCollection healthyUnits = TroopManager.playerTroops.healthy.Values;
        Dictionary<string, int>.ValueCollection woundedUnits = TroopManager.playerTroops.wounded.Values;
        string[] keyArray = new string[keys.Count];
        string curKey;
        int[] healthyTroopNums = new int[healthyUnits.Count];
        int[] woundedTroopNums = new int[woundedUnits.Count];
        woundedUnits.CopyTo(woundedTroopNums, 0);
        healthyUnits.CopyTo(healthyTroopNums, 0);
        keys.CopyTo(keyArray, 0);
        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x,(buttonHeight + buttonSpace)*keys.Count);
        for (int i = 0; i < keys.Count;i++)
        {
            curKey = keyArray[i];
            GameObject button = (GameObject)Instantiate(buttonPrefab,new Vector3(-leftAdjust,(-buttonHeight/2)+((-buttonHeight - buttonSpace)*i),0), transform.rotation);
            button.transform.SetParent(transform,false);
            GameObject unit = (GameObject)Resources.Load("Battles/Units/" + curKey);
            Unit unitStats = unit.GetComponent<Unit>();
            button.GetComponent<Button>().onClick.AddListener(() => ChangeUnitStats.unitPanel.changeStats(curKey));
        }


    }
}
