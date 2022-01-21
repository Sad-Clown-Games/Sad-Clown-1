using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
Singleton responsible for scene changes, saving data, and pulling data.
Add this to every scene.
*/
public class Party_Controller : MonoBehaviour
{
    public List<Player_Character> active_party_list;
    public List<Player_Character> data_ordered_party_list; //stupid ass implimentation but basically matches the order thatt Player Data has


    private void Start() {
        Game_Manager.Instance.party_controller = this;
    }
    public void InitializeLists(List<Player_Character> party){
        active_party_list = party;
        data_ordered_party_list = party;
    }

    public void Populate_Party_Data(Character_Data data){
        //copy data from Player_Data into each party member
        data_ordered_party_list[0].battle_object.stats = (data.alpha.stats);
        data_ordered_party_list[1].battle_object.stats = (data.mimosa.stats);
        data_ordered_party_list[2].battle_object.stats = (data.conner.stats);
        data_ordered_party_list[3].battle_object.stats = (data.judas.stats);
        data_ordered_party_list[4].battle_object.stats = (data.asher.stats);
        data_ordered_party_list[5].battle_object.stats = (data.ferdinand.stats);
        data_ordered_party_list[6].battle_object.stats = (data.placeholder7.stats);
        data_ordered_party_list[7].battle_object.stats = (data.placeholder8.stats);
        //load attacks into the 

    }

    public void Swap_Party_Order(int a, int b){
        //update game data
        active_party_list[a].battle_object.is_being_switched = false;
        active_party_list[b].battle_object.is_being_switched = false; //set flag to false
        int to = active_party_list[a].battle_object.stats.party_order;
        int from = active_party_list[b].battle_object.stats.party_order;
        active_party_list[a].battle_object.stats.party_order =  from;
        active_party_list[b].battle_object.stats.party_order = to;
        //sort
        Debug.Log("Swapped "+ a + " and " + b);
        //make sure you sort the order before you call it again, but not during a bulk swapping

    }

        //get all the battle objects the player can use, in order
    public List<Player_Combatant> Get_Combat_Party_By_Order(){
        active_party_list = active_party_list.OrderBy(w => w.battle_object.stats.party_order).ToList();
        List<Player_Combatant> combat_party = new List<Player_Combatant>();
        foreach(Player_Character a in active_party_list){
            if(a.battle_object.stats.is_unlocked) //pay attention here
                combat_party.Add(a.battle_object);
        }
        return combat_party;
    }
    //get all the characters the player can use, in order
    public List<Player_Character> Get_Active_Party_By_Order(){
        active_party_list = active_party_list.OrderBy(w => w.battle_object.stats.party_order).ToList();
        List<Player_Character> combat_party = new List<Player_Character>();
        foreach(Player_Character a in active_party_list){
            if(a.battle_object.stats.is_unlocked)
                combat_party.Add(a);
        }
        return combat_party;
    }

        //flips a flag
    public void Flip_Switch_Flag(int idx){
        if(active_party_list[idx].battle_object.is_being_switched == true)
            active_party_list[idx].battle_object.is_being_switched = false;
        else
            active_party_list[idx].battle_object.is_being_switched = true;
    }

}