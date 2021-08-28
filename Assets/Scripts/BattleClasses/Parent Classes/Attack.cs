using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

abstract public class Attack : Action
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
    protected float timer = 0;
    public float hang_time = 1.5f;
    public float wait_time = 0.5f;
    public bool crit = false;
    override abstract public void Do_Action();
    
    public void Swap_Actor_Pos(){
        var tempPos = Actor_Start_Pos;
        Actor_Start_Pos = Target_Start_Pos;
        Target_Start_Pos = tempPos;
    }
    public void Spawn_Damage_Counter(int value){
        Battle_Number_Display cur = Instantiate(damage_display,cur_targets[0].Get_Damage_Display_Offset(),transform.rotation).GetComponent<Battle_Number_Display>();
        cur.Set_Value(value);
        cur.Set_Damage_Gradient();
    }

    public void Die(){
        Destroy(this.gameObject);
    }

    override public void Reset_Cameras(){
        foreach(CinemachineVirtualCamera cam in set.cameras){
            cam.m_Priority = 0;
        }
    }

}
