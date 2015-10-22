using UnityEngine;
using System.Collections;

[System.Serializable]
public class OverworldMenu : MonoBehaviour {

    public GameObject overUi;
    public Transform menuUI;
    public GameObject mainUI;
    public Transform errorPopup;
	// Use this for initialization
	void Start () {
        overUi = GameObject.FindGameObjectWithTag("OverworldUI");
       // menuUI = gameObject.transform.FindChild("OverworldMenu");
      //  errorPopup = menuUI.FindChild("Error");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    if(Input.GetKey(KeyCode.Escape))
        {
            overUi.SetActive(false);
            menuUI.gameObject.SetActive(true);
            mainUI.gameObject.SetActive(true);
            gameObject.GetComponent<EventManagement>().Pause();
        }
        if(errorPopup.gameObject.activeInHierarchy ==true && Input.anyKey)
        {
            errorPopup.gameObject.SetActive(false);
        }
	}

    public void Resume()
    {
        overUi.SetActive(true);
        menuUI.gameObject.SetActive(false);
        mainUI.gameObject.SetActive(false);
        gameObject.GetComponent<EventManagement>().Unpause();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Save()
    {
        SaveAndLoad.Save();
    }

    public void Load()
    {
        SaveAndLoad.Load();
        if (SaveAndLoad.savedWorld == null)
        {
            Application.LoadLevel("LoadGameScene");
        }
        else
        {
            errorPopup.gameObject.SetActive(true);
        }
    }
}
