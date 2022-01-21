using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
Parent class for party member object that can be accessed across all scenes
*/
abstract public class Player_Character : MonoBehaviour
{
    //this gets pulled out of the class into the field, stores attacks and various sprites and such
    //We want this to be a child of Player_Combatant
    //When we load the game save most of the player data goes into the combatant class which we can read from there
    //that then goes into an object we display in the scene, we need a method to populate that object with the new stuff
    public string character_name;
    public string long_description = "Details about character should be put here, there should be flags set across the story that change this string to fit the context of the game's story";
    public Player_Combatant battle_object;
    public Sprite full_body_sprite;
    public List<Attack> Get_All_Attacks(){
        return battle_object.attacks;
    }
    public Character_Stats Get_Stats(){
        return battle_object.stats;
    }
    public Item Get_Weapon(){
        return battle_object.weapon;
    }
    public Item Get_Trinket(){
        return battle_object.trinket;
    }
    public Item Get_Armor(){
        return battle_object.armor;
    }    
    public void Equip_Weapon(Item item){
        battle_object.weapon = item;
        if(item != null) //if unequiping
            battle_object.stats.equipment.weapon = item.name;
        else
            battle_object.stats.equipment.weapon = "";
    }
    public void Equip_Armor(Item item){
        battle_object.armor = item;
                if(item != null) //if unequiping
            battle_object.stats.equipment.armor = item.name;
        else
            battle_object.stats.equipment.armor = "";
    }
    public void Equip_Trinket(Item item){
        battle_object.trinket = item;
                if(item != null) //if unequiping
            battle_object.stats.equipment.trinket = item.name;
        else
            battle_object.stats.equipment.trinket = "";
    }
}
