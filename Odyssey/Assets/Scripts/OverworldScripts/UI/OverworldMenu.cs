﻿using UnityEngine;
using System.Collections;

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
	void Update () {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            overUi.SetActive(false);
            menuUI.gameObject.SetActive(true);
            mainUI.gameObject.SetActive(true);
            EventManagement.gameController.Pause();
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
        EventManagement.gameController.Unpause();
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
        Time.timeScale = 1f;
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
