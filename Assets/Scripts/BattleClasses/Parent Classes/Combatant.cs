using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

abstract public class Combatant : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera idle_cam;
    public string combatant_name = "Teste";
    public Attack default_attack;
    public Attack default_guard;
    public Vector3 target_arrow_location;
    public GameObject pawn;

    //List of attacks the combatant has
    public List<Attack> attacks;
    public Vector3 menu_pos;
    public Vector3 offset;
    public Character_Stats stats;//format for later
    public Animator animator;
    public Sprite battle_sprite;
    public Vector3 attacked_pos_offset; //when someone attacks this, this is where they do the attack animation.

    public Character_Stats GetCharacter_Stats(){
        return stats;
    }

    public Vector3 Get_Menu_Position(){
        return pawn.transform.position + menu_pos;
    }

    public void Set_Attacks_By_String(string[] attacks){
        this.attacks = Game_Manager.Instance.action_registry.Get_Attacks_By_Names(attacks);
    }

    public void Set_Attacks_From_Stats(){
        Set_Attacks_By_String(stats.skills);
    }

    public Vector3 Get_Attacked_Pos_Offset(){
        return Get_Pawn_Transform().position + attacked_pos_offset;
    }

    public Transform Get_Pawn_Transform(){
        return pawn.transform;
    }

    //Moves pawn to target using MoveTowards, if speed is 0, then it teleports, 
    public void Move_Pawn(Vector3 target, float speed){
        if(speed == 0){
            pawn.transform.position = target;
        }
        pawn.transform.position = Vector3.MoveTowards(pawn.transform.position, target, speed*Time.deltaTime);
    }

    public bool Pawn_At_Loc(Vector3 loc){
        if(pawn.transform.position == loc)
            return true;
        return false;
    }


    //We need to build the billboarding logic for changing sprites here
    //As well as build the logic for changing the sprites for attacks
    //Basically we call all of the above methods in the specific combatant, and then in the child class we call
    //logic for any character specific sprite ruitines, like special attacks. 
}
