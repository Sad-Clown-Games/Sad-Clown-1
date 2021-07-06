using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Game_Data
{
    public Save_Location save_location; //for loading correct scene and loaction
    public Saved_Items saved_items; //list of items
    
    //storing party order as a list of ints that corresponds to the index that the placeholders are in
    //IT IS VERY IMPORTANT THAT THE CHARACTERS ARE ORDERED THE SAME IN EVERY LIST IN THE INSPECTOR, OTHERWISE WE GET OUT OF ORDER SHIT
    //When we change party order, we just swap values, simple as that.
    public Character_Data party_data;
    public Game_Flags game_flags; //flags for scenes and global
}

[System.Serializable]
public struct Save_Location{
    public string name;
    public string scene_name;
    public Vector3 transform;
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
    public string[] status_effects;
    public bool is_unlocked;
    public int party_order; //we gotta make sure we audit the party so there's no dupes
}

[System.Serializable]
public struct Saved_Equipment{
    public string head;
    public string body;
    public string weapon;
}

[System.Serializable]
public struct Character_Data{
    public CharData_Placeholder1 placeholder1;
    public CharData_Placeholder2 placeholder2;
    public CharData_Placeholder3 placeholder3;
    public CharData_Placeholder4 placeholder4;
    public CharData_Placeholder1 placeholder5;
    public CharData_Placeholder2 placeholder6;
    public CharData_Placeholder3 placeholder7;
    public CharData_Placeholder4 placeholder8;
}

//Each party member gets a struct
[System.Serializable]
public struct CharData_Placeholder1{
    public Character_Stats stats;
    public Saved_Equipment equipment;
    public bool unique_flag;
    public int unique_stat;
}

[System.Serializable]
public struct CharData_Placeholder2{
    public Character_Stats stats;
    public Saved_Equipment equipment;
    public bool unique_flag;
    public int unique_stat;
}

[System.Serializable]
public struct CharData_Placeholder3{
    public Character_Stats stats;
    public Saved_Equipment equipment;
    public bool unique_flag;
    public int unique_stat;
}

[System.Serializable]
public struct CharData_Placeholder4{
    public Character_Stats stats;
    public Saved_Equipment equipment;
    public bool unique_flag;
    public int unique_stat;
}

[System.Serializable]
public struct CharData_Placeholder5{
    public Character_Stats stats;
    public Saved_Equipment equipment;
    public bool unique_flag;
    public int unique_stat;
}

[System.Serializable]
public struct CharData_Placeholder6{
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
    public Test_Scene_Flags test_scene;
}

//flags that apply to every scene, like if we had some constant visual effect or smthg
[System.Serializable]
public struct Global_Flags{
    public bool flag1;
    public bool flag2;
    public bool flag3;
}
//scene flags would be "is x dead" "is door unlocked?" "is rock moved", when a scene is loaded we go through all of them and 
//make sure it's in the correct state
[System.Serializable]
public struct Test_Scene_Flags{
    public bool flag1;
    public bool flag2;
    public bool flag3;
}

