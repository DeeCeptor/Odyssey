using UnityEngine;
using System.Collections;

public class Main_Menu : MonoBehaviour 
{
    public void Show_Credits()
    {
        SceneManager.scene_manager.Start_Conversation(GameObject.Find("SwitchToCredits"));
    }


    public void Start_Game()
    {
        SceneManager.scene_manager.Start_Conversation(GameObject.Find("StartGame"));
    }


    public void To_Kickstarter()
    {
        Application.OpenURL("https://kickstarter.com");
    }


    public void To_SteamGreenlight()
    {
        Application.OpenURL("https://steamcommunity.com/greenlight/");
    }


    public void To_Feedback()
    {
        Application.OpenURL("https://google.com/drive");
    }


    public void Quit()
    {
        Application.Quit();
    }


    void Start () 
	{
	
	}
	
	void Update () 
	{
	
	}
}
