using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
	public void Show_Credits()
	{
		SceneManager.scene_manager.Start_Conversation(GameObject.Find("Credits"));
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
