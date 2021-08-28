using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

abstract public class Combatant : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera idle_cam;
    public Transform camera_transform;
    public billboarder billboarder;
    public string combatant_name = "Teste";
    public Sprite portrait;
    public bool debug;
    public Attack default_attack;
    public Attack default_guard;
    public Action special_action;
    public Vector3 target_arrow_location;
    public GameObject pawn;
    public GameObject pawn_prefab;
    //List of attacks the combatant has
    public List<Attack> attacks;
    public Vector3 menu_pos;
    public Vector3 offset;
    public Character_Stats stats;//format for later
    public Animator animator;
    public Sprite battle_sprite;
    public Vector3 attacked_pos_offset; //when someone attacks this, this is where they do the attack animation.
    public Vector3 damage_value_offset;
    public Vector3 stage_pos_offset; // offset for when the character is being staged
    public bool is_opponent;
    public bool is_dead;
    public bool die_with_sprite;
    public bool b_anim_walk;
    public bool b_anim_spec;
    public bool b_anim_dying;
    public bool b_anim_casting;
    public bool b_anim_attk;
    public bool b_anim_hurt;
    public bool b_anim_idle;
    public bool b_anim_dead;

    public Character_Stats GetCharacter_Stats(){
        return stats;
    }

    public Vector3 Get_Menu_Position(){
        return Get_Pawn_Transform().position + menu_pos;
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

    public Vector3 Get_Damage_Display_Offset(){
        return Get_Pawn_Transform().position + damage_value_offset;
    }
    public Vector3 Get_Target_Pos_Offset(){
        return Get_Pawn_Transform().position + target_arrow_location;
    }
    public Vector3 Get_Stage_Pos_Offset(){
        return stage_pos_offset;
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

    public Animator Get_Pawn_Animator(){
        return pawn.GetComponent<Animator>();
    }

    public void Take_Damage(int damage,Battle_Controller controller){
        stats.cur_hp -= damage;
        stats.cur_hp = Mathf.Clamp(stats.cur_hp,0,999999);
        if(!is_opponent)
            controller.UIController.bannerController.GetPartyMemberByIdx(stats.party_order).Fill_Combatant_Info(this);
        if(stats.cur_hp <= 0){
            Die();
            //die  but we need a way for multiple characters to die at once.
        }
        else{
            AnimHurt();
        }
    }

    public void Die(){
        AnimDying();
        //shrug
    }



    //As well as build the logic for changing the sprites for attacks
    //Basically we call all of the above methods in the specific combatant, and then in the child class we call
    //logic for any character specific sprite ruitines, like special attacks. 
    //Update

    abstract  public void Animation_Control();
    abstract  public void AnimAttack();
    abstract  public void AnimHurt();
    abstract  public void AnimDying();
    abstract  public void AnimDead();
    abstract  public void AnimMove();
    abstract  public void AnimSpecial();
    abstract  public void AnimIdle();
    abstract  public void AnimCast();
}
