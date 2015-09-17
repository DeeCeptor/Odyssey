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
