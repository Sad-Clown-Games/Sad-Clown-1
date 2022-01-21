using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Game_Data
{
    public Scene_Loc save_scene; //for loading correct scene and loaction
    public Scene_Loc last_scene;
    public Scene_Loc next_scene;
    public Saved_Items saved_items; //list of items
    public int location_state;
    
    //storing party order as a list of ints that corresponds to the index that the placeholders are in
    //IT IS VERY IMPORTANT THAT THE CHARACTERS ARE ORDERED THE SAME IN EVERY LIST IN THE INSPECTOR, OTHERWISE WE GET OUT OF ORDER SHIT
    //When we change party order, we just swap values, simple as that.
    public Character_Data party_data;
    public Game_Flags game_flags; //flags for scenes and global

    public void Set_Last_Scene(Scene_Loc loc){
        last_scene = loc;
    }
    public void Set_Save_Scene(Scene_Loc loc){
        save_scene = loc;
    }
    public void Set_Next_Scene(Scene_Loc loc){
        next_scene = loc;
    }
}

//for storing info on when we go into battle
[System.Serializable]
public struct Scene_Loc{
    public SerializableVector3 location;
    public string scene_name;
}


[System.Serializable]
public struct Saved_Items{
    public string[] items;
}

[System.Serializable]
public struct Character_Stats{
    public int max_hp;
    public int max_mp;
    public int cur_hp;
    public int cur_mp;
    public int atk;
    public int def;
    public int luc;
    public int spd;
    public int exp;
    public int level;
    public float crit_dmg;
    public string[] status_effects;
    public Saved_Equipment equipment;
    public string[] skills;
    public bool is_unlocked;
    public int party_order; //we gotta make sure we audit the party so there's no dupes
}

[System.Serializable]
public struct Saved_Equipment{
    public string weapon;
    public string armor;
    public string trinket;
}

[System.Serializable]
public struct Character_Data{
    public CharData_Alpha alpha;
    public CharData_Mimosa mimosa;
    public CharData_Conner conner;
    public CharData_Asher asher;
    public CharData_Judas judas;
    public CharData_Ferdinand ferdinand;
    public CharData_Placeholder7 placeholder7;
    public CharData_Placeholder8 placeholder8;
}

//Each party member gets a struct
[System.Serializable]
public struct CharData_Alpha{
    public Character_Stats stats;
    public bool unique_flag;
    public int unique_stat;
}

[System.Serializable]
public struct CharData_Mimosa{
    public Character_Stats stats;
    public Saved_Equipment equipment;
    public bool unique_flag;
    public int unique_stat;
}

[System.Serializable]
public struct CharData_Conner{
    public Character_Stats stats;
    public Saved_Equipment equipment;
    public bool unique_flag;
    public int unique_stat;
}

[System.Serializable]
public struct CharData_Asher{
    public Character_Stats stats;
    public Saved_Equipment equipment;
    public bool unique_flag;
    public int unique_stat;
}

[System.Serializable]
public struct CharData_Judas{
    public Character_Stats stats;
    public Saved_Equipment equipment;
    public bool unique_flag;
    public int unique_stat;
}

[System.Serializable]
public struct CharData_Ferdinand{
    public Character_Stats stats;
    public Saved_Equipment equipment;
    public bool unique_flag;
    public int unique_stat;
}

[System.Serializable]
public struct CharData_Placeholder7{
    public Character_Stats stats;
    public Saved_Equipment equipment;
    public bool unique_flag;
    public int unique_stat;
}

[System.Serializable]
public struct CharData_Placeholder8{
    public Character_Stats stats;
    public Saved_Equipment equipment;
    public bool unique_flag;
    public int unique_stat;
}


//Just list all the flags
[System.Serializable]
public struct Game_Flags{
    public Global_Flags global;
    public Test_Scene_Data test_scene;
}

//flags that apply to every scene, like if we had some constant visual effect or smthg, 
[System.Serializable]
public struct Global_Flags{
    public bool flag1;
    public bool flag2;
    public bool flag3;
}
//scene flags would be "is x dead" "is door unlocked?" "is rock moved", when a scene is loaded we go through all of them and 
//make sure it's in the correct state
//Also for storing puzzle information like the locations of puzzle rocks 
[System.Serializable]
public struct Test_Scene_Data{
    public bool puzzle_complete;
    public SerializableVector3 puzzlerock1_location;
    public SerializableVector3 puzzlerock2_location;
}

