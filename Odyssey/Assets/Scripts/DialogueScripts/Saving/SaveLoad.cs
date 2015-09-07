using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

// Handles loading serialized save files, and storing save files
public static class SaveLoad
{
	public static List<GameState> savedGames = new List<GameState>();
	

	// Loads our savedGames.gd file stored in our applications persistent data path
	public static void Load() 
	{
		if (File.Exists(Application.persistentDataPath + "/savedGames.gd")) 
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);	// Open the file
			SaveLoad.savedGames = (List<GameState>)bf.Deserialize(file);	// Read from the file
			file.Close();
		}
	}


	// Saves our savedGames.gd file stored in our applications persistent data path
	public static void Save() 
	{
		savedGames.Add(GameState.current);

		// Record the date we're saving
		GameState.current.date_last_saved = System.DateTime.Now.ToString();

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/savedGames.gd");	// Create the file
		bf.Serialize(file, SaveLoad.savedGames);	// Write to it
		file.Close();
	}
}
