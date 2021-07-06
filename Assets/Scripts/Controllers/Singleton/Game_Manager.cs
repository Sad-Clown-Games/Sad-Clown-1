using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
/*
Singleton responsible for scene changes, saving data, and pulling data.
Add this to every scene.
*/
public class Game_Manager : MonoBehaviour
{

    //Static reference
    public static Game_Manager Instance;

    [Header("Player Data")]
    public Player_Data player_data;

    [Header("Characters")]
    //we need a way to represent active party members, and also access stats at all times in a reasonable way
    //with no duplicate "player character" classes
    public GameObject placeholder1;
    public GameObject placeholder2;
    public GameObject placeholder3;
    public GameObject placeholder4;
    public GameObject placeholder5;
    public GameObject placeholder6;
    public GameObject placeholder7;
    public GameObject placeholder8;

    public List<Player_Character> party_list;

    void Awake()
    {
        //Let the gameobject persist over the scenes
        DontDestroyOnLoad(gameObject);
        //Check if the Instance instance is null
        if (Instance == null)
        {
            //This instance becomes the single instance available
            Instance = this;
        }
    }

    void Start(){
        Startup();
    }

    public void Startup(){
        //Instantiate all the party members
        Set_Party_List();
        Copy_Data();
    }
    public void Copy_Data(){

    }

    public void Set_Party_List(){
        //ADD IN ORDER, IF YOU ADD A NEW CHARACTER APPEND IT TO THE BOTTOM OF THE LIST
        //MAKE SURE YOU CHANGE THE PARTY ORDER IN THE INSPECTOR TO BE 0 - X NUM 
        //I WAS DOING LEAD AND DRINKING METH WATER WHEN I DESIGNED THIS WHAT THE FUCK
        party_list.Clear();
        party_list.Add(Instantiate(placeholder1).GetComponent<Player_Character>());
        party_list.Add(Instantiate(placeholder2).GetComponent<Player_Character>());
        party_list.Add(Instantiate(placeholder3).GetComponent<Player_Character>());
        party_list.Add(Instantiate(placeholder4).GetComponent<Player_Character>());
        party_list.Add(Instantiate(placeholder5).GetComponent<Player_Character>());
        party_list.Add(Instantiate(placeholder6).GetComponent<Player_Character>());
        party_list.Add(Instantiate(placeholder7).GetComponent<Player_Character>());
        party_list.Add(Instantiate(placeholder8).GetComponent<Player_Character>());
        //NEVERMIND I'M FUCKING DUMB
        party_list = party_list.OrderBy(w => w.battle_object.stats.party_order).ToList();
    }

    public void Swap_Party_Order(int a, int b){
        //update game data
        party_list[a].battle_object.is_being_switched = false;
        party_list[b].battle_object.is_being_switched = false; //set flag to false
        int to = party_list[a].battle_object.stats.party_order;
        int from = party_list[b].battle_object.stats.party_order;
        party_list[a].battle_object.stats.party_order =  from;
        party_list[b].battle_object.stats.party_order = to;
        //sort
        Debug.Log("Swapped "+ a + " and " + b);
        //make sure you sort the order before you call it again, but not during a bulk swapping

    }
    
    //get all the battle objects the player can use, in order
    public List<Player_Combatant> Get_Combat_Party_By_Order(){
        party_list = party_list.OrderBy(w => w.battle_object.stats.party_order).ToList();
        List<Player_Combatant> combat_party = new List<Player_Combatant>();
        foreach(Player_Character a in party_list){
            if(a.battle_object.stats.is_unlocked)
                combat_party.Add(a.battle_object);
        }
        return combat_party;
    }
    //get all the characters the player can use, in order
    public List<Player_Character> Get_Active_Party_By_Order(){
        party_list = party_list.OrderBy(w => w.battle_object.stats.party_order).ToList();
        List<Player_Character> combat_party = new List<Player_Character>();
        foreach(Player_Character a in party_list){
            if(a.battle_object.stats.is_unlocked)
                combat_party.Add(a);
        }
        return combat_party;
    }
    
    //flips a flag
    public void Flip_Switch_Flag(int idx){
        if(party_list[idx].battle_object.is_being_switched == true)
            party_list[idx].battle_object.is_being_switched = false;
        else
            party_list[idx].battle_object.is_being_switched = true;
    }
}
