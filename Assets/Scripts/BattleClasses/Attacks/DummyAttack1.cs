 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DummyAttack1 : Attack
{
    // Start is called before the first frame update

    public Vector3 Camera_1_Start_Pos;
    public Vector3 Camera_1_End_Pos;
    public Vector3 Actor_Original_Pos;
    public Vector3 Target_Original_Pos;
    public Vector3 Actor_Start_Pos;
    public Vector3 Target_Start_Pos;
    public Vector3 Actor_End_Pos;
    public Vector3 Stage_Location;
    public camera_set set;
    public Vector3 camera1_offset;
    public bool wait_done;
    public bool movement_done;
    public bool actor_animation_done;
    public bool target_animation_done;
    public bool damage_shown;
    private float timer = 0;
    public float hang_time = 1.5f;
    public float wait_time = 0.5f;
    public bool crit = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //simple move to location then leave
        if(is_active){
            if(!wait_done ){
                timer += Time.deltaTime;
                if(timer > wait_time){
                    timer = 0;
                    wait_done = true;
                }
            }
            if(wait_done && !movement_done)
                cur_actor.Move_Pawn(Actor_End_Pos,speed);
            if(cur_actor.Pawn_At_Loc(Actor_End_Pos)){
                //normally we'd do an animaton here, and then have a condition on that animation being done,
                //or switch to another camera and then set some bools
                movement_done = true;
            }
            if(movement_done && !actor_animation_done){
                cur_actor.Get_Pawn_Animator().Play("BasicAttack");
                actor_animation_done = true;
            }
            if(actor_animation_done){
                if(!target_animation_done){
                    cur_targets[0].Get_Pawn_Animator().Play("NormalHurt");
                    set.cameras[0].m_LookAt = cur_targets[0].Get_Pawn_Transform();
                    int final_damage = Calculate_Damage();
                    cur_targets[0].Take_Damage(final_damage);//final_damage);
                    Spawn_Damage_Counter(final_damage);

                    target_animation_done = true;
                }
                timer += Time.deltaTime;
            }
            if(timer > hang_time){
                timer = 0;
                Exit_Action();
            }

        }
    }

    public override void Do_Action()
    {
        Debug.Log(cur_actor.combatant_name + "Is attacking ");
        Debug.Log(cur_targets);
        Debug.Log("With " + attack_name);
        Debug.Log("With Base Damage:" + damage);
        is_active = true;
        //camera_1.transform.position = actor.Get_Pawn_Transform().position + camera1_offset;
    }

    public void Exit_Action(){
        //return everything
        cur_actor.Get_Pawn_Transform().position = Actor_Original_Pos;
        cur_targets[0].pawn.transform.position =  Target_Original_Pos;
        is_active = false;
        Reset_Cameras();
        if(controller.Is_Party_Dead() || controller.Is_Enemies_Dead()){
                next_action = null;
        }
        if(next_action)
            next_action.Stage_Action();
        Die();
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
        cur_actor.Get_Pawn_Transform().position = Actor_Start_Pos + Stage_Location;
        cur_targets[0].Get_Pawn_Transform().position = Target_Start_Pos + Stage_Location; //set target pos
        //set movement location
        Actor_End_Pos = cur_targets[0].Get_Attacked_Pos_Offset();
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
        Debug.Log("Does Damage:" + calc_damage);
        return calc_damage;
    }

    public void Die(){
        Destroy(this.gameObject);
    }

    override public void Reset_Cameras(){
        set.cameras[0].m_Priority = 0;
    }
    override public void Set_Camera_Order(int x){
        //sets up the cameras so that they change in the right order;
        set.cameras[0].m_Priority = x;
    }

    public void Spawn_Damage_Counter(int value){
        Battle_Number_Display cur = Instantiate(damage_display,cur_targets[0].Get_Damage_Display_Offset(),transform.rotation).GetComponent<Battle_Number_Display>();
        cur.Set_Value(value);
        cur.Set_Damage_Gradient();

    }

    public void Swap_Actor_Pos(){
        var tempPos = Actor_Start_Pos;
        Actor_Start_Pos = Target_Start_Pos;
        Target_Start_Pos = tempPos;
         
    }
    
}
