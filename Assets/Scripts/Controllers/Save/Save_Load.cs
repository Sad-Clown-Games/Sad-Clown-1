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


    public void Save(Game_Data data,string file_name){
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + file_name + ".save");
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game Saved");
    }

    public Game_Data Load(string file_name){
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/" + file_name + ".save", FileMode.Open);
        Game_Data data = (Game_Data)bf.Deserialize(file);
        file.Close();
        return data;

    }
}