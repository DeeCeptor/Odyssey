using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveAndLoad
{
    public static List<World> savedGames = new List<World>();
    public static void Save(string Filename)
    {
        Load();
        World.curWorld.getCurrent();
        savedGames.Add(World.curWorld);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.sv");
        bf.Serialize(file, SaveAndLoad.savedGames);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.sv"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.sv", FileMode.Open);
            SaveAndLoad.savedGames = bf.Deserialize(file) as List<World>;
            file.Close();
        }
    }
}
