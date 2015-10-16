using UnityEngine;
using System.Collections;

public class OverworldMenu : MonoBehaviour {

    public GameObject overUi;
    public Transform menuUI;
	// Use this for initialization
	void Start () {
        overUi = GameObject.Find("OverworldUI");
        menuUI = gameObject.transform.FindChild("OverworldMenu");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    if(Input.GetKey(KeyCode.Escape))
        {
            overUi.SetActive(false);
            menuUI.gameObject.SetActive(true);
            gameObject.GetComponent<EventManagement>().Pause();
        }
	}

    public void Resume()
    {
        overUi.SetActive(true);
        menuUI.gameObject.SetActive(false);
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
    }
}
