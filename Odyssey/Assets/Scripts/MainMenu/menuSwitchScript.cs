using UnityEngine;
using System.Collections;

public class menuSwitchScript : MonoBehaviour {
    public GameObject mainMenu;
    public GameObject unitMenu;
    public GameObject shipMenu;
    public GameObject heroesMenu;
    //1 is main, 2 is unit, 3 is ship
    public int menuOn = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(menuOn == 1)
            {
                UnitOn();
            }

            else if (menuOn == 2)
            {
                ShipOn();
            }

            else if (menuOn == 3)
            {
                HeroesOn();
            }

            else if (menuOn == 4)
            {
                MainOn();
            }
        }
	}

    public void UnitOn()
    {
        menuOn = 2;
        mainMenu.SetActive(false);
        shipMenu.SetActive(false);
        unitMenu.SetActive(true);
        heroesMenu.SetActive(false);
    }

    public void ShipOn()
    {
        menuOn = 3;
        mainMenu.SetActive(false);
        shipMenu.SetActive(true);
        unitMenu.SetActive(false);
        heroesMenu.SetActive(false);
    }

    public void MainOn()
    {
        menuOn = 1;
        mainMenu.SetActive(true);
        shipMenu.SetActive(false);
        unitMenu.SetActive(false);
        heroesMenu.SetActive(false);
    }

    public void HeroesOn()
    {
        menuOn = 4;
        mainMenu.SetActive(false);
        shipMenu.SetActive(false);
        unitMenu.SetActive(false);
        heroesMenu.SetActive(true);
    }
}
