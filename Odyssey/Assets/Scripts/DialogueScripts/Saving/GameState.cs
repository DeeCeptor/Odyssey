using UnityEngine;
using System.Collections;


// Contains all progress made by the player
// Will be serialized as a file when the player saves, and loaded when the player loads
[System.Serializable]
public class GameState : MonoBehaviour
{


	public static GameState current;	// Current save file we're using

	// Strings used to describe the save file from the load save menu
	public string date_last_saved;	// Contains the date and time this file was last saved
	public string chapter;	// English description of how far in the game you are
	public string location;	// Where you saved
}
