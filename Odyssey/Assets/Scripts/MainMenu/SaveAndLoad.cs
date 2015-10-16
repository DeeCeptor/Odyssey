using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveAndLoad
{
    public static List<World> savedGames = new List<World>();
    public static World savedWorld;
    public static void Save()
    {
        World.curWorld.getCurrent();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.sv");
        bf.Serialize(file, World.curWorld);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.sv"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.sv", FileMode.Open);
            savedWorld = bf.Deserialize(file) as World;
            file.Close();
        }
    }
}
