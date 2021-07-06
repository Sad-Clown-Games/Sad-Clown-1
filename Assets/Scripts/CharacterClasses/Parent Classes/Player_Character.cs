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
    public Player_Combatant battle_object;
    
}
