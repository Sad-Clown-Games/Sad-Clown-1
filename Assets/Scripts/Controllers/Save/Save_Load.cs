using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
/*
Singleton responsible for scene changes, saving data, and pulling data.
Add this to every scene.
*/
public class Save_Load{


    public void Save(Game_Data data){
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game Saved");
    }

    public Game_Data Load(){
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
        Game_Data data = (Game_Data)bf.Deserialize(file);
        file.Close();
        Debug.Log("Game Save Deserialized");
        return data;

    }
}