using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuSwitch2 : MonoBehaviour {
    public GameObject suppliesMenu;
    public GameObject unitMenu;
    //1 is supplies, 2 is unit
    public int menuOn = 1;
    public Text playerInput;
    public Text foodInput;
    public Text waterInput;
    public Text sailorInput;

    public string unitSelected;
    public int cost = 1;
    public GameObject errorMessage;
    public GameObject notEnoughGoldMessage;
    public GameObject notEnoughUnitsMessage;
    public GameObject UnitPanel;
    public Dictionary<string,int> unitsAvailable;
    public ChangeUnitStats changer;
    public GameObject notEnoughRoomMessage;
    public GameObject Content;

    // Use this for initialization
    void Start()
    {
        unitsAvailable = EventManagement.gameController.islandEventIsOn.GetComponent<PortIslandEventScript>().units;
    }

    public void SetUnitsAvailable(Dictionary<string,int> unitList)
    {
        unitsAvailable = unitList;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (menuOn == 1)
            {
                UnitOn();
            }

            else if (menuOn == 2)
            {
                SuppliesOn();
            }
        }
    }

    public void UnitOn()
    {
        menuOn = 2;
        suppliesMenu.SetActive(false);
        unitMenu.SetActive(true);
    }

    public void SuppliesOn()
    {
        menuOn = 1;
        suppliesMenu.SetActive(true);
        unitMenu.SetActive(false);
    }

    public void buyFood()
    {
        float result = 0;
        cost = 1;
        if (!float.TryParse(foodInput.text, out result))
        {
            errorMessage.SetActive(true);
            return;
        }

        if (result * cost > ResourceManager.playerResources.gold)
        {
            notEnoughGoldMessage.SetActive(true);
            return;
        }

        if(ResourceManager.playerResources.checkIfOver(result))
        {
            notEnoughRoomMessage.SetActive(true);
            return;
        }


        ResourceManager.playerResources.gold = ResourceManager.playerResources.gold - ((int)result * cost);
        ResourceManager.playerResources.food = ResourceManager.playerResources.food + result;
    }

    public void buyWater()
    {
        int result = 0;
        cost = 1;
        if (!int.TryParse(waterInput.text, out result))
        {
            errorMessage.SetActive(true);
            return;
        }

        if (result * cost > ResourceManager.playerResources.gold)
        {
            notEnoughGoldMessage.SetActive(true);
            return;
        }

        if (ResourceManager.playerResources.checkIfOver(result))
        {
            notEnoughRoomMessage.SetActive(true);
            return;
        }

        ResourceManager.playerResources.gold = ResourceManager.playerResources.gold - (result * cost);
        ResourceManager.playerResources.food = ResourceManager.playerResources.water + result;
    }

    public void buySailors()
    {
        int result = 0;
        cost = 5;
        int modCost = cost - (1 * TroopManager.playerTroops.getHaggling());
        if (!int.TryParse(sailorInput.text, out result))
        {
            errorMessage.SetActive(true);
            return;
        }

        if (result * cost > ResourceManager.playerResources.gold)
        {
            notEnoughGoldMessage.SetActive(true);
            return;
        }

        if (ResourceManager.playerResources.sailors + result > ResourceManager.playerResources.maxSailors)
        {
            notEnoughRoomMessage.SetActive(true);
            return;
        }

        ResourceManager.playerResources.gold = ResourceManager.playerResources.gold - (result * modCost);
        ResourceManager.playerResources.sailors = ResourceManager.playerResources.sailors + result;
    }

    public void BuyUnits()
    {
        string unitName = UnitPanel.GetComponent<ChangeUnitStats>().unitPrefab;
        GameObject unit = (GameObject)Resources.Load("Battles/Units/" + unitName);
        Unit unitStats = unit.GetComponent<Unit>();
        cost = unitStats.cost;
        int modCost = cost - (1*TroopManager.playerTroops.getHaggling());
        int result = 0;
        if(!int.TryParse(playerInput.text,out result))
        {
            errorMessage.SetActive(true);
            return;
        }

        if(result*modCost > ResourceManager.playerResources.gold)
        {
            notEnoughGoldMessage.SetActive(true);
            return;
        }

        if (result> unitsAvailable[unitName])
        {
            notEnoughUnitsMessage.SetActive(true);
            return;
        }

        ResourceManager.playerResources.gold = ResourceManager.playerResources.gold - (result*modCost);
        unitsAvailable[unitName] = unitsAvailable[unitName] - result;
        TroopManager.playerTroops.healthy[unitName] = TroopManager.playerTroops.healthy[unitName] + result;
        Content.GetComponent<PopulateShop>().Refresh();
        Debug.Log(TroopManager.playerTroops.healthy[unitName].ToString());

    }   

    public void Leave()
    {
        EventManagement.gameController.EndEvent();
        Destroy(gameObject);
    }
}
