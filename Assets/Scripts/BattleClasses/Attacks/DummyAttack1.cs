 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DummyAttack1 : Attack
{
    // Start is called before the first frame update

    public Vector3 Camera_1_Start_Pos;
    public Vector3 Camera_1_End_Pos;
    private Vector3 Actor_Original_Pos;
    public Vector3 Actor_Start_Pos;
    public Vector3 Actor_End_Pos;
    public Combatant cur_actor;
    public List<Combatant> cur_targets;
    public float speed = 1;
    public CinemachineVirtualCamera camera_1;
    public Vector3 camera1_offset;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //simple move to location then leave
        if(is_active){
            cur_actor.Move_Pawn(Actor_End_Pos,speed);
            if(cur_actor.Pawn_At_Loc(Actor_End_Pos)){
                //normally we'd do an animaton here, and then have a condition on that animation being done,
                //or switch to another camera and then set some bools
                Exit_Action();
            }
        }
    }

    public override void Do_Action(Combatant actor, List<Combatant> targets)
    {
        transform.position = actor.Get_Pawn_Transform().position;
        Debug.Log(actor.combatant_name + "Is attacking ");
        Debug.Log(targets);
        Debug.Log("With " + attack_name);
        Debug.Log("With Base Damage:" + damage);
        Actor_Original_Pos = actor.Get_Pawn_Transform().position;
        Actor_Start_Pos = Actor_Original_Pos;
        Actor_End_Pos = targets[0].Get_Attacked_Pos_Offset();
        is_active = true;
        cur_actor = actor;
        cur_targets = targets;

        cur_actor.Get_Pawn_Transform().position = Actor_Start_Pos;
        camera_1.gameObject.SetActive(true); 
        //camera_1.transform.position = actor.Get_Pawn_Transform().position + camera1_offset;
        camera_1.m_LookAt = actor.Get_Pawn_Transform();//targets[0].transform; //we just index 0 since there's no multitarget attacks / this is a single targets
        camera_1.m_Follow = actor.Get_Pawn_Transform();
        camera_1.m_Priority = 101; //above the priorities of the menu cameras
    }

    public void Exit_Action(){
        //return everything
        is_active = false;
        cur_actor.pawn.transform.position = Actor_Original_Pos;
        camera_1.m_Priority = 0; //below the priorities of the menu cameras
        //camera_1.gameObject.SetActive(false); //disable the camera for memory or smth lol
        Die();
    }

    public void Die(){
        Destroy(this.gameObject);
    }

    override public void Reset_Cameras(){
        camera_1.m_Priority = 0;
    }
    override public void Set_Camera_Order(int x){
        camera_1.m_Priority += x;
    }
    
}
