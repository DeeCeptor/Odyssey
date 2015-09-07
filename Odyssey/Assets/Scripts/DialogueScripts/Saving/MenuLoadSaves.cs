using UnityEngine;
using System.Collections;

public class MenuLoadSaves : MonoBehaviour 
{

	void Start () 
	{
		SaveLoad.Load();
		Populate_Load_Menu();
	}


	// Populates the load file menu with the save files
	void Populate_Load_Menu()
	{

	}

	
	void Update () 
	{
	
	}
}
