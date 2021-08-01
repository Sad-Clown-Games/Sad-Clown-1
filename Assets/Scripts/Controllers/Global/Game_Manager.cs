using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
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
    public Action_Registry action_registry;
    //Object containing all helper functions script

    public List<Player_Character> party_list;
    private List<Player_Character> data_ordered_party_list; //stupid ass implimentation but basically matches the order thatt Player Data has

    //debug save game in inspector;

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
        Startup();
    }

    void Start(){
        
    }

    public void Startup(){
        //Instantiate all the party members
        Instantiate_Party_List();
        //Copy data from our 
        Load_Game(); //remove later
        Copy_Data();
        Order_Party_By_Party_Order();
    }
    public void Copy_Data(){
        //copy data from Player_Data into each party member
        
        data_ordered_party_list[0].battle_object.stats = (player_data.save_data.party_data.placeholder1.stats);
        data_ordered_party_list[1].battle_object.stats = (player_data.save_data.party_data.placeholder2.stats);
        data_ordered_party_list[2].battle_object.stats = (player_data.save_data.party_data.placeholder3.stats);
        data_ordered_party_list[3].battle_object.stats = (player_data.save_data.party_data.placeholder4.stats);
        data_ordered_party_list[4].battle_object.stats = (player_data.save_data.party_data.placeholder5.stats);
        data_ordered_party_list[5].battle_object.stats = (player_data.save_data.party_data.placeholder6.stats);
        data_ordered_party_list[6].battle_object.stats = (player_data.save_data.party_data.placeholder7.stats);
        data_ordered_party_list[7].battle_object.stats = (player_data.save_data.party_data.placeholder8.stats);
        //load attacks into the 
        foreach(Player_Character p in party_list){
            p.battle_object.Set_Attacks_From_Stats();
        }
    }

    public void Instantiate_Party_List(){
        party_list.Clear();
        party_list.Add(Instantiate(placeholder1).GetComponent<Player_Character>());
        party_list.Add(Instantiate(placeholder2).GetComponent<Player_Character>());
        party_list.Add(Instantiate(placeholder3).GetComponent<Player_Character>());
        party_list.Add(Instantiate(placeholder4).GetComponent<Player_Character>());
        party_list.Add(Instantiate(placeholder5).GetComponent<Player_Character>());
        party_list.Add(Instantiate(placeholder6).GetComponent<Player_Character>());
        party_list.Add(Instantiate(placeholder7).GetComponent<Player_Character>());
        party_list.Add(Instantiate(placeholder8).GetComponent<Player_Character>());
        data_ordered_party_list = party_list;
    }

    public void Order_Party_By_Party_Order(){
        party_list = party_list.OrderBy(w => w.battle_object.stats.party_order).ToList();
    }

    //it'll really break some shit if the order ever gets disrupted so this is a super headass way to do this,
    //But I dug this hole, and it's honestly pretty shallow, this is probably as bad as it'll get.
    public void Update_Party_Data(){
        player_data.save_data.party_data.placeholder1.stats = data_ordered_party_list[0].battle_object.GetCharacter_Stats();
        player_data.save_data.party_data.placeholder2.stats = data_ordered_party_list[1].battle_object.GetCharacter_Stats();
        player_data.save_data.party_data.placeholder3.stats = data_ordered_party_list[2].battle_object.GetCharacter_Stats();
        player_data.save_data.party_data.placeholder4.stats = data_ordered_party_list[3].battle_object.GetCharacter_Stats();
        player_data.save_data.party_data.placeholder5.stats = data_ordered_party_list[4].battle_object.GetCharacter_Stats();
        player_data.save_data.party_data.placeholder6.stats = data_ordered_party_list[5].battle_object.GetCharacter_Stats();
        player_data.save_data.party_data.placeholder7.stats = data_ordered_party_list[6].battle_object.GetCharacter_Stats();
        player_data.save_data.party_data.placeholder8.stats = data_ordered_party_list[7].battle_object.GetCharacter_Stats();
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

    public void Save_Game(){
        Transfer_Active_Party_Data_To_Saved_Party_Data();
        Save_Load save = new Save_Load();
        save.Save(player_data.save_data);
    }
    public void Load_Game(){
        Save_Load load = new Save_Load();
        player_data.save_data = load.Load();
        player_data.items = action_registry.Get_Items_By_Names(player_data.save_data.saved_items.items);
    }

    public void Transfer_Active_Party_Data_To_Saved_Party_Data(){
        Update_Party_Data();
    }

}
