using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
Parent class for party member object that can be accessed across all scenes
*/
public class PartyMember_PlaceHolder5 : Player_Character{

    Player_Combatant combatant;

    private void Start() {
        combatant = GetComponent<Player_Combatant>();
    }

    public Character_Stats Get_Stats(){
        return combatant.GetCharacter_Stats();
    }
    
}