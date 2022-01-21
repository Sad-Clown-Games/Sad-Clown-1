using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DummyAttack1 : Attack
{
    // Start is called before the first frame update

    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("BattleController").GetComponent<Battle_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        //simple move to location then leave
        if(is_active){
            if(!wait_done){
                timer += Time.deltaTime;
                if(timer > wait_time){
                    timer = 0;
                    wait_done = true;
                }
            }
            if(wait_done && !movement_done){
                cur_actor.AnimMove();
                cur_actor.Move_Pawn(Actor_End_Pos,speed);
            }
            if(cur_actor.Pawn_At_Loc(Actor_End_Pos)){
                //normally we'd do an animaton here, and then have a condition on that animation being done,
                //or switch to another camera and then set some bools
                movement_done = true;
            }
            if(movement_done && !actor_animation_done){
                cur_actor.AnimAttack();
                actor_animation_done = true;
            }
            if(actor_animation_done){
                if(!target_animation_done){
                    set.cameras[0].m_LookAt = cur_targets[0].Get_Pawn_Transform();
                    int final_damage = Calculate_Damage();
                    cur_targets[0].Take_Damage(final_damage,controller); //animation is done here, since they may die.
                    Spawn_Damage_Counter(final_damage);
                    target_animation_done = true;
                }
                timer += Time.deltaTime;
            }
            if(target_animation_done && timer > hang_time){
                cur_actor.AnimIdle();
                if(!cur_targets[0].is_dead)
                    cur_targets[0].AnimIdle();
                timer = 0;
                Exit_Action();
            }

        }
    }

    public override void Do_Action()
    {
        if(cur_targets[0].is_dead){
            controller.HideAttackText();
            cur_actor.Get_Pawn_Transform().position = Actor_Original_Pos;
            cur_targets[0].pawn.transform.position =  Target_Original_Pos;
            is_active = false;
            Reset_Cameras();
            if(!Retarget()){ //if retargeting fails, which shouldn't happen since that would mean all are dead
                Exit_Action();
                return;
            }
            Stage_Action();
        }
        if(cur_actor.is_dead){
            Exit_Action();
            return;
            //ruh roh;
        }
        if(debug_on){
            Debug.Log(cur_actor.combatant_name + "Is attacking ");
            Debug.Log(cur_targets);
            Debug.Log("With " + action_name);
            Debug.Log("With Base Damage:" + damage);
        }
        if(controller == null) //fecal funny moment #1
            controller = GameObject.FindGameObjectWithTag("BattleController").GetComponent<Battle_Controller>();
        controller.ShowAttackText(TextHelper.GenerateBattleString(
            cur_targets[0].combatant_name,
            cur_actor.combatant_name,
            description));
        is_active = true;
        //camera_1.transform.position = actor.Get_Pawn_Transform().position + camera1_offset;
    }

    public void Exit_Action(){
        //return everything
        controller.HideAttackText();
        cur_actor.Get_Pawn_Transform().position = Actor_Original_Pos;
        cur_targets[0].pawn.transform.position =  Target_Original_Pos;
        is_active = false;
        Reset_Cameras();
        Next_Action();
    }


    public override void Stage_Action()
    {
        //set camerass
        set.cameras[0].gameObject.SetActive(true); 
        set.cameras[0].m_LookAt = cur_actor.Get_Pawn_Transform();
        set.cameras[0].m_Follow = cur_actor.Get_Pawn_Transform();
        set.cameras[0].m_Priority = 101; 
        if(cur_actor.is_opponent){
            set.Flip_Set();
            Swap_Actor_Pos();
        }
        //store original positions
        Actor_Original_Pos = cur_actor.Get_Pawn_Transform().position;
        Target_Original_Pos = cur_targets[0].Get_Pawn_Transform().position;
        //set starting positions
        cur_actor.Get_Pawn_Transform().position = Actor_Start_Pos + Stage_Location + cur_actor.Get_Stage_Pos_Offset();
        cur_targets[0].Get_Pawn_Transform().position = Target_Start_Pos + Stage_Location+ cur_targets[0].Get_Stage_Pos_Offset(); //set target pos
        //set movement location
        Actor_End_Pos = cur_targets[0].Get_Attacked_Pos_Offset();
        Actor_End_Pos.y = Actor_Original_Pos.y; //make sure we dont move vertically
        transform.position = Stage_Location;
    }


    //damage should be between 0-999
    //level 1 basic attack should do 3-5 dmg
    //max level basic attack should do  < 200 on attk characters
    public int Calculate_Damage(){
        float variation = Random.Range(1f,1.2f); //vary between 
        int calc_damage = Mathf.RoundToInt((damage * cur_actor.stats.atk)/2 * (variation));
        if(Random.Range(1,20) * (0.05f*cur_actor.stats.luc) > 20 ){ //d20 * luck/10
            calc_damage = Mathf.RoundToInt(calc_damage * cur_actor.stats.crit_dmg);
            Debug.Log("Crit!!");
        }
        if(debug_on){
            Debug.Log("Does Damage:" + calc_damage);
        }
        return calc_damage;
    }

    override public void Set_Camera_Order(int x){
        //sets up the cameras so that they change in the right order;
        set.cameras[0].m_Priority = x;
    }



    
}
