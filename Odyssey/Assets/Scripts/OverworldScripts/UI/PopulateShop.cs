using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PopulateShop : MonoBehaviour {
    public int scrollItemHeight;
    public GameObject buttonPrefab;
    public float buttonHeight = 40;
    public float buttonSpace = 5;
    //space to move button left
    public float leftAdjust = 50;
    // Use this for initialization
    public GameObject MenuHead;
    void Start()
    {

    }

    // Update is called once per frame
    void OnEnable()
    {
        var children = new List<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        Dictionary<string, int> unitList;
        unitList = MenuHead.GetComponent<MenuSwitch2>().unitsAvailable;

        Dictionary<string, int>.KeyCollection keys = unitList.Keys;
        Dictionary<string, int>.ValueCollection healthyUnits = unitList.Values;
        Dictionary<string, int>.ValueCollection woundedUnits = unitList.Values;
        string[] keyArray = new string[keys.Count];
        string curKey;
        int[] healthyTroopNums = new int[healthyUnits.Count];
        int[] woundedTroopNums = new int[woundedUnits.Count];
        woundedUnits.CopyTo(woundedTroopNums, 0);
        healthyUnits.CopyTo(healthyTroopNums, 0);
        keys.CopyTo(keyArray, 0);
        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, (buttonHeight + buttonSpace) * keys.Count);
        for (int i = 0; i < keys.Count; i++)
        {
            curKey = keyArray[i];
            GameObject button = (GameObject)Instantiate(buttonPrefab, new Vector3(-leftAdjust, (-buttonHeight / 2) + ((-buttonHeight - buttonSpace) * i), 0), transform.rotation);
            button.transform.SetParent(transform, false);
            GameObject unit = (GameObject)Resources.Load("Battles/Units/" + curKey);
            Unit unitStats = unit.GetComponent<Unit>();
            string unitName = curKey;
            button.GetComponent<Button>().onClick.AddListener(() => ChangeUnitStats.unitPanel.changeStats(unitName));
            button.GetComponent<Image>().sprite = unitStats.portrait;
            button.transform.FindChild("UnitName").GetComponent<Text>().text = unitStats.u_name;
            button.transform.FindChild("TroopNums").GetComponent<Text>().text = unitList[curKey].ToString();
            button.transform.FindChild("Cost").GetComponent<Text>().text = unitStats.cost.ToString();
        }


    }

    public void Refresh()
    {
        OnEnable();
    }
}
