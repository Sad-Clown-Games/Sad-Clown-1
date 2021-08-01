using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Action_Registry : MonoBehaviour
{
    //list of all attack_registry in game
    public  List<Attack> attack_registry;

    //list of all items in game
    public  List<Item> item_registry;
    public  Party_Switch party_switch;

    public  void Register_Attack(Attack attack){
        attack_registry.Add(attack);
    }

    public  void Register_Item(Item item){
        item_registry.Add(item);
    }

    public  Attack Get_Attack_By_Name(string name){

        Attack attack = attack_registry.Find(x=> x.gameObject.name == name);
        if(!attack)
            attack = attack_registry[0];
        return attack;
    }

    
    public  List<Attack> Get_Attacks_By_Names(string[] name){

        List<Attack> return_attack = new List<Attack>();

        foreach(string n in name){
            return_attack.Add(attack_registry.Find(x=> x.gameObject.name == n));
        }        
        return return_attack;
    }

    public  Item Get_Item_By_Name(string name){

        Item item = item_registry.Find(x=> x.gameObject.name == name);
        if(!item)
            item = item_registry[0];
        return item;
    }

    
    public  List<Item> Get_Items_By_Names(string[] name){

        List<Item> return_item = new List<Item>();

        foreach(string n in name){
            return_item.Add(item_registry.Find(x=> x.gameObject.name == n));
        }        
        return return_item;
    }

    public Party_Switch Get_Switch(){
        return party_switch;
    }
}
